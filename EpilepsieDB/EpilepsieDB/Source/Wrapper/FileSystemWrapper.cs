using System.IO;
using System.IO.Abstractions;

namespace EpilepsieDB.Source.Wrapper
{
    public class FileSystemWrapper : IFileSystemWrapper
    {
        private readonly IFileSystem _fileSystem;

        public FileSystemWrapper()
        {
            _fileSystem = new FileSystem();
        }

        public FileSystemWrapper(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }


        public virtual string PathCombine(params string[] paths)
        {
            return _fileSystem.Path.Combine(paths);
        }

        public virtual string PathJoin(params string[] paths)
        {
            return _fileSystem.Path.Join(paths);
        }

        public virtual string PathGetRandomFileName()
        {
            return _fileSystem.Path.GetRandomFileName();
        }

        public virtual string PathGetDirectoryName(string path)
        {
            return _fileSystem.Path.GetDirectoryName(path);
        }

        public virtual string PathGetExtension(string path)
        {
            return _fileSystem.Path.GetExtension(path);
        }


        public virtual Stream FileCreate(string path, FileMode mode)
        {
            return _fileSystem.FileStream.New(path, mode);
        }

        public virtual void FileCopy(string source, string destination)
        {
            _fileSystem.File.Copy(source, destination);
        }

        public virtual bool FileExists(string path)
        {
            return _fileSystem.File.Exists(path);
        }

        public virtual void FileDelete(string path)
        {
            _fileSystem.File.Delete(path);
        }

        public virtual void FileWriteAllText(string path, string text)
        {
            _fileSystem.File.WriteAllText(path, text);
        }

        public void FileWriteAllBytes(string path, byte[] data)
        {
            _fileSystem.File.WriteAllBytes(path, data);
        }

        public virtual byte[] FileReadAllBytes(string path)
        {
            return _fileSystem.File.ReadAllBytes(path);
        }


        public virtual bool DirectoryExists(string path)
        {
            return _fileSystem.Directory.Exists(path);
        }

        public virtual void DirectoryDelete(string path, bool recursive)
        {
            _fileSystem.Directory.Delete(path, recursive);
        }

        public virtual void DirectoryCreateDirectory(string path)
        {
            _fileSystem.Directory.CreateDirectory(path);
        }

        public virtual string[] DirectoryGetDirectories(string path)
        {
            return _fileSystem.Directory.GetDirectories(path);
        }

        public virtual void DirectoryCopyDirectory(string source, string destination)
        {
            _fileSystem.Directory.CreateDirectory(destination);

            // create all of the new directories
            foreach (string dirPath in Directory.GetDirectories(source, "*", SearchOption.AllDirectories))
            {
                _fileSystem.Directory.CreateDirectory(dirPath.Replace(source, destination));
            }

            // copy all the files & replace any files with the same name
            foreach (string newPath in _fileSystem.Directory.GetFiles(source, "*.*", SearchOption.AllDirectories))
            {
                _fileSystem.File.Copy(newPath, newPath.Replace(source, destination), true);
            }
        }


        public virtual void ZipFileCreateFromDirectory(string source, string destination)
        {
            System.IO.Compression.ZipFile.CreateFromDirectory(source, destination);
        }
    }
}
