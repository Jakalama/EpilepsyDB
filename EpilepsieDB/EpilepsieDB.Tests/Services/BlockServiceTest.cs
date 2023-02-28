using EpilepsieDB.Models;
using EpilepsieDB.Repositories;
using EpilepsieDB.Services.Impl;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Services
{
    public class BlockServiceTest : AbstractTest
    {
        public BlockServiceTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor()
        {
            // set
            var repoMock = new Mock<IRepository<Block>>();

            // act
            var service = new BlockService(repoMock.Object);
        }
    }
}
