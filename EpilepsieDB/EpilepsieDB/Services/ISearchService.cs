using EpilepsieDB.Models;
using EpilepsieDB.Services.Impl;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EpilepsieDB.Services
{
    public interface ISearchService
    {
        Task<IEnumerable<Scan>> Search(SearchQuery query);
    }

    public class SearchQuery
    {
        public string FreeFormSearch = "";
        public string Name = "";
        public string Rec = "";
        public string Scan = "";
        public string Ver = "";
        public string PInfo = "";
        public string RecInfo = "";
        public string Date = "";
        public string Time = "";
        public string Label = "";
        public string Type = "";
        public string Dim = "";
        public string Annot = "";
    }
}
