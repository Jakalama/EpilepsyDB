using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using EpilepsieDB.Models;

namespace EpilepsieDB.Data
{
    public class EpilepsieDBContext : IdentityDbContext, IAppDbContext
    {
        public EpilepsieDBContext(DbContextOptions<EpilepsieDBContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Recording> Recordings { get; set; }
        public virtual DbSet<Scan> Scans { get; set; }
        public virtual DbSet<Signal> Signals { get; set; }
        public virtual DbSet<Block> Blocks { get; set; }
        public virtual DbSet<Annotation> Annotations { get; set; }
    }
}
