using Microsoft.EntityFrameworkCore;
using EpilepsieDB.Models;
using System.Threading.Tasks;
using System.Threading;

namespace EpilepsieDB.Data
{
    // may need to be append the user specific stuff
    public interface IAppDbContext
    {
        DbSet<Patient> Patients { get; set; }
        DbSet<Recording> Recordings { get; set; }
        DbSet<Scan> Scans { get; set; }
        DbSet<Block> Blocks { get; set; }
        DbSet<Signal> Signals { get; set; }
        DbSet<Annotation> Annotations { get; set; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

    }
}
