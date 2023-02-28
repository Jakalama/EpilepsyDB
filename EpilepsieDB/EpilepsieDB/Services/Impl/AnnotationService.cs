using EpilepsieDB.Models;
using EpilepsieDB.Repositories;

namespace EpilepsieDB.Services.Impl
{
    public class AnnotationService : AService<Annotation>, IAnnotationService
    {
        public AnnotationService(IRepository<Annotation> repository) : base(repository)
        {
        }
    }
}
