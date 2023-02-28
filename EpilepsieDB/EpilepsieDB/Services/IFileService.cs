using EpilepsieDB.Source;
using System.Threading.Tasks;

namespace EpilepsieDB.Services
{
    public interface IFileService
    {
        string CreateDirectory(params string[] directoryPath);
        void DeleteDirectory(params string[] directoryPath);
        void CopyDirectory(string source, string destination);

        Task<string> CreateFile(IFileStream stream, params string[] contentDir);
        void DeleteFile(string relativePath);

        string GetFullFilePath(string relativePath);

        byte[] ReadAllBytesFromFile(string relativePath);

        byte[] ReadAllBytesFromDir(string source, string tempDirAbsPath);

        //void CreateZip(string source, string destination);

        Task<string> CreateWebResource(IFileStream stream);

        void DeleteWebResource(string filename);
    }
}
