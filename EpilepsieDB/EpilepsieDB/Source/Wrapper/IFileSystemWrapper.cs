using System.IO;

namespace EpilepsieDB.Source.Wrapper
{
    public interface IFileSystemWrapper
    {
        string PathCombine(params string[] paths);
        string PathJoin(params string[] paths);
        string PathGetRandomFileName();
        string PathGetDirectoryName(string path);
        string PathGetExtension(string path);

        Stream FileCreate(string path, FileMode mode);
        void FileCopy(string source, string destination);
        bool FileExists(string path);
        void FileDelete(string path);
        void FileWriteAllText(string path, string text);
        void FileWriteAllBytes(string path, byte[] data);
        byte[] FileReadAllBytes(string path);

        void DirectoryCreateDirectory(string path);
        bool DirectoryExists(string path);
        void DirectoryDelete(string path, bool recursive);
        string[] DirectoryGetDirectories(string path);
        void DirectoryCopyDirectory(string source, string destination);

        void ZipFileCreateFromDirectory(string source, string destination);
    }
}
