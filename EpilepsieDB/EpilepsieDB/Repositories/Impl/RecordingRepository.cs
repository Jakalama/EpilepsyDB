using EpilepsieDB.Data;
using EpilepsieDB.Models;

namespace EpilepsieDB.Repositories.Impl
{
    public class RecordingRepository : ARepository<Recording>
    {
        public RecordingRepository(IAppDbContext context) : base(context)
        {
        }
    }
}
