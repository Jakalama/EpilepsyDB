using EpilepsieDB.Models;
using EpilepsieDB.Repositories;

namespace EpilepsieDB.Services.Impl
{
    public class BlockService : AService<Block>, IBlockService
    {
        public BlockService(IRepository<Block> repository) : base(repository)
        {
        }
    }
}
