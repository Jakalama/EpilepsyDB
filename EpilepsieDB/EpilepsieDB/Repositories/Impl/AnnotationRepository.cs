using EpilepsieDB.Data;
using EpilepsieDB.Models;

namespace EpilepsieDB.Repositories.Impl
{
    public class AnnotationRepository : ARepository<Annotation>
    {
        public AnnotationRepository(IAppDbContext context) : base(context)
        {
        }
    }
}
