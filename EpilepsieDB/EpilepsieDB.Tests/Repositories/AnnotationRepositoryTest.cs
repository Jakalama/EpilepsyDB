using EpilepsieDB.Data;
using EpilepsieDB.Repositories.Impl;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Repositories
{
    public class AnnotationRepositoryTest : AbstractTest
    {
        public AnnotationRepositoryTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor()
        {
            // set
            var contextMock = new Mock<IAppDbContext>();

            // act
            var repo = new AnnotationRepository(contextMock.Object);
        }
    }
}
