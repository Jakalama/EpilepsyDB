using EpilepsieDB.Models;
using EpilepsieDB.Repositories;
using System.Threading.Tasks;

namespace EpilepsieDB.Services.Impl
{
    public class RecordingService : AService<Recording>, IRecordingService
    {
        private readonly IRepository<Patient> _patientRepository;
        private readonly IFileService _fileService;

        public RecordingService(
            IRepository<Recording> recordingRepository,
            IRepository<Patient> patientRepository,
            IFileService fileService) 
            : base(recordingRepository)
        {
            _patientRepository = patientRepository;

            _fileService = fileService;
        }

        public new async Task Create(Recording recording)
        {
            Patient patient = await _patientRepository.Get(recording.PatientID);

            if (patient == null)
                return;

            recording.ContentDir = _fileService.CreateDirectory(patient.ContentDir);

            await base.Create(recording);
        }

        public new async Task<bool> Update(Recording recording)
        {
            // ToDo: find better way to update only changed values
            Recording stored = await _repository.Get(recording.ID);
            stored.RecordingNumber = recording.RecordingNumber;

            return await base.Update(stored);
        }

        public new async Task<bool> Delete(int id)
        {
            Recording recording = await _repository.Get(id);
            if (recording != null && recording != default(Recording))
            {
                await _repository.Delete(recording);

                Patient patient = await _patientRepository.Get(recording.PatientID);

                _fileService.DeleteDirectory( recording.ContentDir);
                return true;
            }

            return false;
        }
    }
}
