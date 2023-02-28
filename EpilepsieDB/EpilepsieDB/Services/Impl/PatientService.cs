using System;
using System.Threading.Tasks;
using EpilepsieDB.Models;
using EpilepsieDB.Repositories;
using EpilepsieDB.Source;

namespace EpilepsieDB.Services.Impl
{
    public class PatientService : AService<Patient>, IPatientService
    {
        private readonly IFileService _fileService;

        public PatientService(
            IRepository<Patient> patientRepository,
            IFileService fileService) 
            : base(patientRepository)
        {
            _fileService = fileService;
        }

        public async Task Create(Patient patient, IFileStream nifti, IFileStream mri)
        {
            if (nifti == null || mri == null)
                return;

            string contentDir = _fileService.CreateDirectory("");
            patient.ContentDir = contentDir;

            patient.NiftiFilePath = await _fileService.CreateFile(nifti, patient.ContentDir);
            patient.MriImagePath = await _fileService.CreateWebResource(mri);

            await base.Create(patient);
        }

        public async Task<bool> Update(Patient model, IFileStream nifti, IFileStream mri)
        {
            if (await _repository.Exists(model.ID))
            {
                await UpdateFiles(model, nifti, mri);

                await _repository.Update(model);
                return true;
            }

            return false;
        }

        public new async Task<bool> Delete(int id)
        {
            Patient patient = await _repository.Get(id);
            if (patient != null && patient != default(Patient))
            {
                await _repository.Delete(patient);
                _fileService.DeleteDirectory(patient.ContentDir);
                _fileService.DeleteWebResource(patient.MriImagePath);
                return true;
            }

            return false;
        }

        private async Task UpdateFiles(Patient patient, IFileStream nifti, IFileStream mri)
        {
            Patient oldPatient = await _repository.GetNoTracking(patient.ID);

            if (nifti != null)
            {
                _fileService.DeleteFile(oldPatient.NiftiFilePath);
                patient.ContentDir = oldPatient.ContentDir;
                patient.NiftiFilePath = await _fileService.CreateFile(nifti, patient.ContentDir);
            }

            if (mri != null)
            {
                _fileService.DeleteWebResource(oldPatient.MriImagePath);
                patient.MriImagePath = await _fileService.CreateWebResource(mri);
            }
        }
    }
}
