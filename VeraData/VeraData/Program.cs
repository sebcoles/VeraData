using AutoMapper;
using CommandLine;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VeracodeService.Models;
using VeracodeService.Repositories;
using VeracodeWebhooks.Configuration;
using VeraData.DataAccess;
using VeraData.DataAccess.Models;

namespace VeraData
{
    class Program
    {
        private static IServiceProvider _serviceProvider;
        static void Main(string[] args)
        {
            IConfiguration Configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
#if DEBUG
                .AddJsonFile($"appsettings.Development.json", false)
#else
                .AddJsonFile("appsettings.json", false)
#endif
                .Build();

            var serviceCollection = new ServiceCollection();
            var connection = Configuration.GetConnectionString("DefaultConnection");

            serviceCollection.AddDbContext<ApplicationDbContext>
                (options => options.UseSqlServer(connection));

            serviceCollection.Configure<VeracodeConfiguration>(options => Configuration.GetSection("Veracode").Bind(options));
            serviceCollection.AddScoped<IVeracodeRepository, VeracodeRepository>();
            serviceCollection.AddScoped<IGenericRepository<Application>, GenericRepository<Application>>();
            serviceCollection.AddScoped<IGenericRepository<UploadFile>, GenericRepository<UploadFile>>();
            serviceCollection.AddScoped<IGenericRepository<Flaw>, GenericRepository<Flaw>>();
            serviceCollection.AddScoped<IGenericRepository<Module>, GenericRepository<Module>>();
            serviceCollection.AddScoped<IGenericRepository<ModuleFile>, GenericRepository<ModuleFile>>();
            serviceCollection.AddScoped<IGenericRepository<PreScanError>, GenericRepository<PreScanError>>();
            serviceCollection.AddScoped<IGenericRepository<Scan>, GenericRepository<Scan>>();
            serviceCollection.AddScoped<IGenericRepository<Sandbox>, GenericRepository<Sandbox>>();
            serviceCollection.AddScoped<IGenericRepository<SourceFile>, GenericRepository<SourceFile>>();
            serviceCollection.AddScoped<IGenericRepository<UploadFile>, GenericRepository<UploadFile>>();
            serviceCollection.AddScoped<IGenericRepository<Category>, GenericRepository<Category>>();
            serviceCollection.AddScoped<IGenericRepository<Cwe>, GenericRepository<Cwe>>();
            serviceCollection.AddScoped<IMappingService, MappingService>();
            serviceCollection.AddAutoMapper(typeof(Program));

            _serviceProvider = serviceCollection.BuildServiceProvider();

            Parser.Default.ParseArguments<RunOptions>(args)
                .MapResult((
                    RunOptions options) => Run(options),
                    errs => HandleParseError(errs));
        }

        static int Run(RunOptions options)
        {
            var mappingService = _serviceProvider.GetService<IMappingService>();
            var veracodeRepo = _serviceProvider.GetService<IVeracodeRepository>();
            var buildRepo = _serviceProvider.GetService<IGenericRepository<Scan>>();
            var appRepo = _serviceProvider.GetService<IGenericRepository<Application>>();
            var categoryRepo = _serviceProvider.GetService<IGenericRepository<Category>>();
            var cweRepo = _serviceProvider.GetService<IGenericRepository<Cwe>>();

            var appinfo = veracodeRepo.GetAppDetail(options.AppId);
            var app = appRepo.GetAll().SingleOrDefault(x => x.VeracodeId == Int32.Parse(options.AppId));
            if (app == null)
            {
                app = appRepo.Create(new Application
                {
                    Name = appinfo.application[0].app_name,
                    VeracodeId = appinfo.application[0].app_id
                });
            }

            var newbuildlist = veracodeRepo.GetAllBuildsForApp(options.AppId)
                .Select(x => (int)x.build_id)
                .ToArray();

            var currentBuilds = buildRepo.GetAll()
                .Select(x => x.Id)
                .ToArray();

            var missingBuilds = newbuildlist.Except(currentBuilds);

            Console.WriteLine($"App: {appinfo.application[0].app_name}\nScans in Veracode: {newbuildlist.Count()}\nScans in Database: {currentBuilds.Count()}\nThere are {missingBuilds.Count()} new builds available");
            if (options.BuildLimit == null)
                Console.WriteLine("No limit specified, all builds will be retrieved. [Press Y to continue]");
            else
                Console.WriteLine($"Preparing to retrieve {options.BuildLimit}. [Press Y to continue]");

            if(!options.Force)
                if (Console.ReadLine().ToLower() != "y")
                    return 1;

            var builds = new List<BuildInfoBuildType>();
            foreach (var missedBuild in missingBuilds)
                builds.Add(veracodeRepo.GetBuildDetail(options.AppId, $"{missedBuild}").build);

            // scans
            SaveScans(builds, ref app);
            appRepo.Update(app);

            // sandboxes
            var sandboxes = veracodeRepo.GetSandboxesForApp(options.AppId).ToList();
            SaveSandboxes(sandboxes, ref app);
            appRepo.Update(app);

            // upload files
            foreach (var scan in app.Scans)
            {
                var uploadFiles = veracodeRepo.GetFilesForBuild(options.AppId, $"{scan.VeracodeId}").ToList();
                scan.UploadFiles = mappingService.UploadFiles(uploadFiles.ToArray()).ToList();
                var modules = veracodeRepo.GetModules(options.AppId, $"{scan.VeracodeId}").ToList();
                scan.Modules = mappingService.Modules(modules.ToArray()).ToList();

                foreach(var module in scan.Modules)
                {
                    var flaws = veracodeRepo.GetFlaws($"{scan.VeracodeId}");
                    var oldCats = categoryRepo.GetAll().ToArray();
                    var oldCwe = cweRepo.GetAll().ToArray();

                    var categories = flaws
                        .Where(x => !oldCats.Any(y => $"{y.Id}".Equals(x.categoryid)))
                        .Select(x => new Category { Description = x.categoryname, VeracodeId = Int32.Parse(x.categoryid) })
                        .GroupBy(x => x.VeracodeId)
                        .Select(x => x.First())
                        .ToList();

                    var cwes = flaws
                        .Where(x => !oldCwe.Any(y => $"{y.Id}".Equals(x.cweid)))
                        .Select(x => new Cwe { 
                            RemediationEffort = Int32.Parse(x.remediationeffort),
                            Description = x.remediation_desc,
                            Exploitability = x.exploitdifficulty,
                            VeracodeId = Int32.Parse(x.cweid) })
                        .GroupBy(x => x.VeracodeId)
                        .Select(x => x.First())
                        .ToList();

                    module.SourceFiles = mappingService.SourceFiles(flaws)
                        .GroupBy(x => new { x.Path, x.Name })
                        .Select(x => x.First())
                        .ToList();

                    foreach (var sourceFile in module.SourceFiles)
                    {
                        var fileFlaws = flaws
                            .Where(x => x.sourcefile == sourceFile.Name 
                            && x.sourcefilepath == sourceFile.Path).ToArray();

                        sourceFile.Flaws = mappingService.Flaws(fileFlaws).ToList();

                        foreach (var flaw in sourceFile.Flaws)
                        {
                            flaw.Category = categories.SingleOrDefault(x => x.VeracodeId == flaw.VeracodeCategoryId);
                            flaw.Cwe = cwes.SingleOrDefault(x => x.VeracodeId == flaw.VeracodeCweId);
                        }
                    }               

                }

                scan.UploadFiles = mappingService.UploadFiles(uploadFiles.ToArray()).ToList();
            }

            appRepo.Update(app);
            return 1;
        }

        private static void SaveScans(List<BuildInfoBuildType> entites, ref Application app)
        {
            var mapper = _serviceProvider.GetService<IMappingService>();
            var mappedScans = mapper.Scans(entites.ToArray()).ToList();
            app.Scans = mappedScans;
        }

        private static void SaveSandboxes(List<SandboxType> entites, ref Application app)
        {
            var sandboxRepo = _serviceProvider.GetService<IGenericRepository<Sandbox>>();
            var mapper = _serviceProvider.GetService<IMappingService>();
            var sandboxes = mapper.Sandboxes(entites.ToArray()).ToList();
            app.Sandboxes = sandboxes;
        }
        static int HandleParseError(IEnumerable<Error> errs)
        {
            return 1;
        }
    }
}
