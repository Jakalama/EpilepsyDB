using EpilepsieDB.Models;
using System.Collections.Generic;

namespace EpilepsieDB.Web.View.ViewModels
{
    public class SearchResult
    {
        public Dictionary<Patient, Dictionary<Recording, IEnumerable<Scan>>> Results = new Dictionary<Patient, Dictionary<Recording, IEnumerable<Scan>>>();
    }
}
