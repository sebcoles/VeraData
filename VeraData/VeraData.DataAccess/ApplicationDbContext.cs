using Microsoft.EntityFrameworkCore;
using VeraData.DataAccess.Models;

namespace VeraData.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Application> Applications { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cwe> Cwes { get; set; }
        public DbSet<Flaw> Flaws { get; set; }
        public DbSet<MitigationStatus> MitigationStatuses { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<PreScanMessage> PreScanError { get; set; }
        public DbSet<RemediationStatus> RemediationStatuses { get; set; }
        public DbSet<Sandbox> Sandboxes { get; set; }
        public DbSet<Scan> Scans { get; set; }
        public DbSet<Severity> Severities { get; set; }
        public DbSet<SourceFile> SourceFiles { get; set; }
        public DbSet<UploadFile> UploadFiles { get; set; }
        public DbSet<UploadFileStatus> UploadFileStatuses { get; set; }
        public DbSet<MitigationAction> MitigationActions { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Severity>().HasData(
                new Severity { Id = 1, VeracodeId = 0, Title = "None" },
                new Severity { Id = 2, VeracodeId = 1, Title = "Informational" },
                new Severity { Id = 3, VeracodeId = 2, Title = "Low" },
                new Severity { Id = 4, VeracodeId = 3, Title = "Medium" },
                new Severity { Id = 5, VeracodeId = 4, Title = "High" },
                new Severity { Id = 6, VeracodeId = 5, Title = "Very High" }
                );

            modelBuilder.Entity<UploadFileStatus>().HasData(
                new UploadFileStatus { Id = 1, Status = "PendingUpload" },
                new UploadFileStatus { Id = 2, Status = "Uploading" },
                new UploadFileStatus { Id = 3, Status = "Purged" },
                new UploadFileStatus { Id = 4, Status = "Uploaded" },
                new UploadFileStatus { Id = 5, Status = "Missing" },
                new UploadFileStatus { Id = 6, Status = "Partial" },
                new UploadFileStatus { Id = 7, Status = "InvalidChecksum" },
                new UploadFileStatus { Id = 8, Status = "InvalidArchive" },
                new UploadFileStatus { Id = 9, Status = "ArchiveFileWithinAnotherArchive" },
                new UploadFileStatus { Id = 10, Status = "ArchiveFilewithUnsupportedCompression" },
                new UploadFileStatus { Id = 11, Status = "ArchiveFileisPasswordProtected" });
        }
    }
}
