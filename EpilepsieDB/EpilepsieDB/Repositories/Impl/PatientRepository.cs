using EpilepsieDB.Data;
using EpilepsieDB.Models;

namespace EpilepsieDB.Repositories.Impl
{
    public class PatientRepository : ARepository<Patient>
    {
        public PatientRepository(IAppDbContext context) : base(context) { }
    }
}
