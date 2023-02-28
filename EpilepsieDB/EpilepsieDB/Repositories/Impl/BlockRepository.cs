using EpilepsieDB.Data;
using EpilepsieDB.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EpilepsieDB.Repositories.Impl
{
    public class BlockRepository : ARepository<Block>
    {
        public BlockRepository(IAppDbContext context) : base(context)
        {
        }

        public override async Task<Block> Get(int id)
        {
            return (await Get(filter: s => s.ID == id, includeProperties: "Annotations")
                ).FirstOrDefault();
        }

        public override async Task<List<Block>> GetAll()
        {
            return (await Get(includeProperties: "Annotations")).ToList();
        }
    }
}
