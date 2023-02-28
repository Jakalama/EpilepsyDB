using EpilepsieDB.Models;
using EpilepsieDB.Source;
using System.Threading.Tasks;

namespace EpilepsieDB.Services
{
    public interface IScanService : IService<Scan>
    {
        Task Create(Scan scan, IFileStream edf);
    }
}
