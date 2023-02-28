using EpilepsieDB.Source;
using System.Threading.Tasks;

namespace EpilepsieDB.Services
{
    public interface IDownloadService
    {
        Task<FileRequest> GetScanFile(int id);
        Task<FileRequest> GetScanFilesByRecording(int id);
        Task<FileRequest> GetFilesFromPatient(int id);

        Task<FileRequest> GetNiftiFile(int id);

    }
}
