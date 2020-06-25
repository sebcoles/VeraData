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
            serviceCollection.AddScoped<IGenericRepository<PreScanMessage>, GenericRepository<PreScanMessage>>();
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

            if (!options.Force)
                if (Console.ReadLine().ToLower() != "y")
                    return 1;

            var scans = new List<BuildInfoBuildType>();
            foreach (var missedBuild in missingBuilds)
                scans.Add(veracodeRepo.GetBuildDetail(options.AppId, $"{missedBuild}").build);

            app.Scans = mappingService.Scans(scans.ToArray()).ToList();

            var sandboxes = veracodeRepo.GetSandboxesForApp(options.AppId).ToList();
            app.Sandboxes = mappingService.Sandboxes(sandboxes.ToArray()).ToList();
           
            var cats = categoryRepo.GetAll().ToList();
            var cwes = cweRepo.GetAll().ToList();
            var newCatagories = new List<Category>();
            var newCwes = new List<Cwe>();

            foreach (var scan in app.Scans)
            {
                var uploadFiles = veracodeRepo.GetFilesForBuild(options.AppId, $"{scan.VeracodeId}").ToList();
                var modules = veracodeRepo.GetModules(options.AppId, $"{scan.VeracodeId}").ToList();
                var entryPoints = veracodeRepo.GetEntryPoints($"{scan.VeracodeId}");
                var mitgations = veracodeRepo.GetAllMitigationsForBuild($"{scan.VeracodeId}");
                var flaws = veracodeRepo.GetFlaws($"{scan.VeracodeId}");

                scan.UploadFiles = mappingService.UploadFiles(uploadFiles.ToArray()).ToList();
                scan.Modules = mappingService.Modules(modules.ToArray()).ToList();

                foreach (var module in scan.Modules)
                    module.EntryPoint = entryPoints.Any(x => x.name == module.Name);

                scan.Flaws = mappingService.Flaws(flaws).ToList();
                scan.UploadFiles = mappingService.UploadFiles(uploadFiles.ToArray()).ToList();
                scan.SourceFiles = mappingService.SourceFiles(flaws)
                    .GroupBy(x => new { x.Path, x.Name })
                    .Select(x => x.First())
                    .ToList();

                var currentCategories = flaws
                    .Where(x => !cats.Any(y => $"{y.VeracodeId}".Equals(x.categoryid)))
                    .Where(x => !newCatagories.Any(y => $"{y.VeracodeId}".Equals(x.categoryid)))
                    .GroupBy(x => x.categoryid)
                    .Select(x => x.First())
                    .ToArray();
                var mappedCategories = mappingService.Cetegories(currentCategories).ToList();
                newCatagories.AddRange(mappedCategories);

                var currentCwes = flaws
                    .Where(x => !cwes.Any(y => $"{y.VeracodeId}".Equals(x.cweid)))
                    .Where(x => !newCwes.Any(y => $"{y.VeracodeId}".Equals(x.cweid)))
                    .GroupBy(x => x.cweid)
                    .Select(x => x.First())
                    .ToArray();
                var mappedCwes = mappingService.Cwe(currentCwes).ToList();
                newCwes.AddRange(mappedCwes);

                var convertedFlaws = mitgations.Select(x => new { MitigationActions = mappingService.Mitigations(x.mitigation_action), VeracodeId = x.flaw_id }).ToArray();

                foreach (var flaw in scan.Flaws.Where(x => convertedFlaws.Any(z => x.VeracodeId == z.VeracodeId)))
                    flaw.MitigationActions = convertedFlaws
                        .Single(x => x.VeracodeId == flaw.VeracodeId)
                        .MitigationActions
                        .ToList();
            }

            categoryRepo.Create(newCatagories);
            cweRepo.Create(newCwes);

            foreach(var scan in app.Scans)
            {
                foreach(var flaw in scan.Flaws)
                {
                    flaw.Category = newCatagories.SingleOrDefault(x => x.VeracodeId == flaw.VeracodeCategoryId);
                    flaw.Cwe = newCwes.SingleOrDefault(x => x.VeracodeId == flaw.VeracodeCweId);
                }
            }

            appRepo.Update(app);
            return 1;
        }

        static int HandleParseError(IEnumerable<Error> errs)
        {
            return 1;
        }
    }
}
