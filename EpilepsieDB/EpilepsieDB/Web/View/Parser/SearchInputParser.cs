using EpilepsieDB.Services;
using System.Collections.Generic;
using System.Reflection;

namespace EpilepsieDB.Web.View.Parser
{
    public class SearchInputParser : ISearchInputParser
    {
        public SearchQuery Parse(string searchString)
        {
            // contains searchable properties
            SearchQuery query = new SearchQuery();

            if (searchString == null)
                return query;

            searchString = searchString.ToLower();
            List<string> parsedParts = new List<string>();

            // loop over all searchable criterias
            foreach (FieldInfo info in typeof(SearchQuery).GetFields())
            {
                if (info == typeof(SearchQuery).GetField(nameof(SearchQuery.FreeFormSearch)))
                    continue;

                string criteria = info.Name.ToLower() + ":";

                // if seach contains this criteria
                if (searchString.Contains(criteria))
                {
                    // split at criteria
                    string criteriaValue = searchString.Split(criteria)[1];

                    // get rid of trailing search parts irrelevant for this criteria
                    criteriaValue = criteriaValue.Split(" ")[0];

                    string cleanValue = criteriaValue.Replace("-", " ");
                    info.SetValue(query, cleanValue.ToLower());

                    // add to already search to remove these later from the search string
                    parsedParts.Add(criteria + criteriaValue);
                }
            }

            return query;
        }
    }
}
