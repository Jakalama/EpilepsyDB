using EpilepsieDB.Data;
using EpilepsieDB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EpilepsieDB.Repositories.Impl
{
    public class ScanRepository : ARepository<Scan>
    {
        public ScanRepository(IAppDbContext context) : base(context)
        { }

        public override async Task<Scan> Get(int id)
        {
            return (await Get(filter: s => s.ID == id, includeProperties: "Recording")
                ).FirstOrDefault();
        }

        public override async Task<List<Scan>> GetAll()
        {
            return (await Get(includeProperties: "Recording")).ToList();
        }
    }
}
