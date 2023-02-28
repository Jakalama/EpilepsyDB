using EpilepsieDB.Data;
using EpilepsieDB.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EpilepsieDB.Repositories.Impl
{
    public class SignalRepository : ARepository<Signal>
    {
        public SignalRepository(IAppDbContext context) : base(context)
        {
        }
    }
}
