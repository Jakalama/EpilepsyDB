using EpilepsieDB.Models;
using EpilepsieDB.Source;
using EpilepsieDB.Source.Wrapper;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace EpilepsieDB.Services.Impl
{
    public class FileService : IFileService
    {
        private readonly IFileSystemWrapper _fileSystem;

        private readonly string workingDir;
        private const string baseDir = "patientData";

        private readonly string webRootPath;
        
        public FileService(IFileSystemWrapper fileSystem, string baseDir, string webRootPath) 
        {
            _fileSystem = fileSystem;

            this.workingDir = _fileSystem.PathCombine(baseDir, FileService.baseDir);
            this.webRootPath = webRootPath;
        }

        public string CreateDirectory(params string[] directoryPath)
        {
            string randomName = _fileSystem.PathGetRandomFileName().Split(".")[0];
            string[] randomPath = directoryPath.Append(randomName).ToArray();
            string combinedPath = _fileSystem.PathJoin(randomPath);
            string newDir = _fileSystem.PathCombine(workingDir, combinedPath);

            if (!_fileSystem.DirectoryExists(newDir))
            {
                try
                {
                    _fileSystem.DirectoryCreateDirectory(newDir);
                }
                catch
                {
                    Console.WriteLine("Could not create Directory: " + combinedPath);
                    combinedPath = CreateDirectory(directoryPath);
                }

            }
            else
            {
                // there is a naming conflict
                combinedPath = CreateDirectory(directoryPath);
            }

            return combinedPath;
        }

        public void DeleteDirectory(params string[] directoryPath)
        {
            string combinedPath = _fileSystem.PathJoin(directoryPath);
            string deletedDir = _fileSystem.PathCombine(workingDir, combinedPath);

            if (_fileSystem.DirectoryExists(deletedDir))
            {
                _fileSystem.DirectoryDelete(deletedDir, true);
            }
        }

        public void CopyDirectory(string source, string destination)
        {
            source = _fileSystem.PathCombine(workingDir, source);

            _fileSystem.DirectoryCopyDirectory(source, destination);
        }

        public async Task<string> CreateFile(IFileStream stream, params string[] contentDir)
        {
            string fileExtension = _fileSystem.PathGetExtension(stream.FileName).ToLowerInvariant().Trim();

            string combinedPath = _fileSystem.PathJoin(contentDir);
            string fileName = _fileSystem.PathGetRandomFileName().Split(".")[0] + fileExtension;
            string relFilePath = _fileSystem.PathCombine(combinedPath, fileName);
            string totalFilePath = _fileSystem.PathCombine(workingDir, relFilePath);

            if (_fileSystem.FileExists(totalFilePath))
                return await CreateFile(stream, contentDir);

            using (Stream file = _fileSystem.FileCreate(totalFilePath, FileMode.CreateNew))
            {
                await stream.CopyToAsync(file);
            }

            return relFilePath;
        }

        public void DeleteFile(string relativePath)
        {
            string fullPath = GetFullFilePath(relativePath);
            if (_fileSystem.FileExists(fullPath))
            {
                _fileSystem.FileDelete(fullPath);
            }
        }

        public string GetFullFilePath(string relativePath)
        {
            return _fileSystem.PathCombine(workingDir, relativePath);
        }

        public byte[] ReadAllBytesFromFile(string relativePath)
        {
            string fullPath = GetFullFilePath(relativePath);
            if (_fileSystem.FileExists(fullPath))
            {
                return _fileSystem.FileReadAllBytes(fullPath);
            }

            return new byte[0];
        }

        public byte[] ReadAllBytesFromDir(string source, string tempDirAbsPath)
        {
            source = _fileSystem.PathCombine(workingDir, source);

            if (_fileSystem.DirectoryExists(source))
            {
                CreateZip(source, tempDirAbsPath);
                byte[] content = _fileSystem.FileReadAllBytes(tempDirAbsPath);

                if (_fileSystem.FileExists(tempDirAbsPath))
                    _fileSystem.FileDelete(tempDirAbsPath);

                return content;
            }

            return new byte[0];
        }

        private void CreateZip(string source, string destination)
        {
            _fileSystem.DirectoryCreateDirectory(_fileSystem.PathGetDirectoryName(destination));
            _fileSystem.ZipFileCreateFromDirectory(source, destination);
        }

        public async Task<string> CreateWebResource(IFileStream stream)
        {
            string fileExtension = _fileSystem.PathGetExtension(stream.FileName).ToLowerInvariant().Trim();

            string combinedPath = _fileSystem.PathJoin(webRootPath);
            string fileName = _fileSystem.PathGetRandomFileName().Split(".")[0] + fileExtension;
            fileName = _fileSystem.PathCombine("nifti", fileName);
            string filePath = _fileSystem.PathCombine(combinedPath, fileName);

            if (_fileSystem.FileExists(filePath))
                return await CreateWebResource(stream);

            _fileSystem.DirectoryCreateDirectory(_fileSystem.PathCombine(combinedPath, "nifti"));

            using (Stream file = _fileSystem.FileCreate(filePath, FileMode.CreateNew))
            {
                await stream.CopyToAsync(file);
            }

            return fileName;
        }

        public void DeleteWebResource(string filename)
        {
            filename = _fileSystem.PathCombine(webRootPath, filename);
            _fileSystem.FileDelete(filename);
        }
    }
}
