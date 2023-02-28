using EpilepsieDB.Models;
using EpilepsieDB.Repositories;
using EpilepsieDB.Services.Impl;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Services
{
    public class AnnotationServiceTest : AbstractTest
    {
        public AnnotationServiceTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor()
        {
            // set
            var repoMock = new Mock<IRepository<Annotation>>();

            // act
            var service = new AnnotationService(repoMock.Object);
        }
    }
}
