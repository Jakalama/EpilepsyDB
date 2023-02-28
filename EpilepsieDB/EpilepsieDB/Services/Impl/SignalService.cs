using EpilepsieDB.Models;
using EpilepsieDB.Repositories;

namespace EpilepsieDB.Services.Impl
{
    public class SignalService : AService<Signal>, ISignalService
    {
        public SignalService(IRepository<Signal> repository) : base(repository)
        {
        }
    }
}
