using EpilepsieDB.Models;
using EpilepsieDB.Repositories;
using EpilepsieDB.Source;
using EpilepsieDB.Source.Wrapper;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EpilepsieDB.Services.Impl
{
    public class DownloadService : IDownloadService
    {
        private readonly IRepository<Patient> _patientRepository;
        private readonly IRepository<Recording> _recordingRepository;
        private readonly IRepository<Scan> _scanRepository;
        private readonly IFileService _fileService;

        private readonly IFileSystemWrapper _fileSystem;

        private readonly string webRootPath;

        public DownloadService(
            IRepository<Patient> patientRepository,
            IRepository<Recording> recordingRepository,
            IRepository<Scan> scanRepository,
            IFileService fileService,
            IFileSystemWrapper fileSystem,
            string webRootPath
            )
        {
            _patientRepository = patientRepository;
            _recordingRepository = recordingRepository;
            _scanRepository = scanRepository;
            _fileService = fileService;

            _fileSystem = fileSystem;

            this.webRootPath = webRootPath;
        }

        public async Task<FileRequest> GetFilesFromPatient(int id)
        {
            Patient patient = await _patientRepository.Get(id);
            if (patient == null || patient == default(Patient))
                return null;

            string dirName = patient.Acronym + "_" + _fileSystem.PathGetRandomFileName().Split(".")[0];
            string destination = _fileSystem.PathCombine(webRootPath, dirName);

            // copy scans & nifti
            _fileService.CopyDirectory(patient.ContentDir, destination);

            // copy mri
            string mriImage = _fileSystem.PathCombine(webRootPath, patient.MriImagePath);
            _fileSystem.FileCopy(mriImage, _fileSystem.PathCombine(destination, "mri" + _fileSystem.PathGetExtension(mriImage)));

            // create zip
            string zipName = patient.Acronym + "_" + _fileSystem.PathGetRandomFileName().Split(".")[0] + ".zip";
            string zipDir = _fileSystem.PathCombine(webRootPath, zipName);
            byte[] content = _fileService.ReadAllBytesFromDir(destination, zipDir);

            // delete temp copy dir
            _fileSystem.DirectoryDelete(destination, true);

            FileRequest result = new FileRequest()
            {
                Name = patient.Acronym + ".zip",
                Data = content
            };

            return result;
        }

        public async Task<FileRequest> GetScanFilesByRecording(int recordingID)
        {
            Recording recording = (await _recordingRepository.Get(filter: r => r.ID == recordingID, includeProperties: "Patient")).FirstOrDefault();
            if (recording == null || recording == default(Recording))
                return null;

            string fileName = recording.Patient.Acronym + "_" + recording.RecordingNumber + "_" + _fileSystem.PathGetRandomFileName().Split(".")[0] + ".zip";
            string destination = _fileSystem.PathCombine(webRootPath, fileName);
            byte[] content = _fileService.ReadAllBytesFromDir(recording.ContentDir, destination);

            FileRequest result = new FileRequest()
            {
                Name = recording.Patient.Acronym + "_" + recording.RecordingNumber + ".zip",
                Data = content
            };

            return result;
        }

        public async Task<FileRequest> GetScanFile(int id)
        {
            Scan scan = await _scanRepository.Get(id);
            if (scan != null && scan != default(Scan))
            {
                byte[] content = _fileService.ReadAllBytesFromFile(scan.EdfFilePath);

                FileRequest result = new FileRequest()
                {
                    Name = scan.EdfDisplayName,
                    Data = content
                };

                return result;
            }

            return null;
        }

        public async Task<FileRequest> GetNiftiFile(int id)
        {
            Patient patient = await _patientRepository.Get(id);
            if (patient != null && patient != default(Patient))
            {
                byte[] content = _fileService.ReadAllBytesFromFile(patient.NiftiFilePath);

                FileRequest result = new FileRequest()
                {
                    Name = patient.Acronym + ".nii",
                    Data = content
                };

                return result;
            }

            return null;
        }
    }
}
