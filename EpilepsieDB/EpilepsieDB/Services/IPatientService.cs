using EpilepsieDB.Models;
using EpilepsieDB.Source;
using System.Threading.Tasks;

namespace EpilepsieDB.Services
{
    public interface IPatientService : IService<Patient>
    {
        Task Create(Patient patient, IFileStream nifti, IFileStream mri);

        Task<bool> Update(Patient model, IFileStream nifti, IFileStream mri);
    }
}
