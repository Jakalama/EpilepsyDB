using EpilepsieDB.Models;
using EpilepsieDB.Repositories;
using EpilepsieDB.Source;
using System;
using System.Threading.Tasks;

namespace EpilepsieDB.Services.Impl
{
    public partial class ScanService : AService<Scan>, IScanService
    {
        private readonly IRepository<Recording> _recordingRepository;
        private readonly IFileService _fileService;
        private readonly IEdfService _edfService;

        public ScanService(
            IRepository<Scan> scanRepository,
            IRepository<Recording> recordingRepository,
            IFileService fileService,
            IEdfService edfService)
            : base(scanRepository)
        {
            _recordingRepository = recordingRepository;
            _fileService = fileService;
            _edfService = edfService;
        }

        public async Task Create(Scan scan, IFileStream edf)
        {
            if (edf == null)
                return;

            Recording recording = await _recordingRepository.Get(scan.RecordingID);

            if (recording == null)
                return;

            string filePath = await _fileService.CreateFile(edf, recording.ContentDir);
            string fullFilePath = _fileService.GetFullFilePath(filePath);

            scan.EdfDisplayName = edf.FileName;
            scan.EdfFilePath = filePath;

            bool success = _edfService.WriteToScan(scan, fullFilePath);

            if (success)
                await _repository.Add(scan);
            else
            {
                _fileService.DeleteFile(filePath);
            }
        }

        public new async Task<bool> Update(Scan scan)
        {
            // ToDo: find better way to update only changed values
            Scan stored = await _repository.Get(scan.ID);
            stored.ScanNumber = scan.ScanNumber;

            return await base.Update(stored);
        }

        public new async Task<bool> Delete(int id)
        {
            Scan scan = await _repository.Get(id);
            if (scan != null && scan != default(Scan))
            {
                await _repository.Delete(scan);
                _fileService.DeleteFile(scan.EdfFilePath);
                return true;
            }

            return false;
        }
    }
}
