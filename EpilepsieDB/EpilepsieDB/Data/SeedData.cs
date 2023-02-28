using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EpilepsieDB.Data
{
    public static class SeedData
    {
        private static IAppDbContext _context;

        public static async Task Initialize(IAppDbContext context)
        {
            _context = context;

            await AddData();
        }

        private static async Task AddData()
        {
            // add initial data here if needed

            // save with this call
            //if (!_context.DATAFIELD.Any())
            //{
            //    _context.DATAFIELD.AddRange(array);
            //    await _context.SaveChangesAsync();
            //}
        }
    }
}
