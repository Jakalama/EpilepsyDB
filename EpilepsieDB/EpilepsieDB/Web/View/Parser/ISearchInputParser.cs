using EpilepsieDB.Services;

namespace EpilepsieDB.Web.View.Parser
{
    public interface ISearchInputParser
    {
        SearchQuery Parse(string searchString);
    }
}
