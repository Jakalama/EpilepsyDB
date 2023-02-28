using EpilepsieDB.EDF;
using EpilepsieDB.Models;

namespace EpilepsieDB.Services
{
    public interface IEdfService
    {
        EdfFile ReadFile(string path);

        bool WriteToScan(Scan scan, string path);
    }
}
