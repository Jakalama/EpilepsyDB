using EpilepsieDB.Data;
using EpilepsieDB.Repositories.Impl;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Repositories
{
    public class SignalRepositoryTest : AbstractTest
    {
        public SignalRepositoryTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor()
        {
            // set
            var contextMock = new Mock<IAppDbContext>();

            // act
            var repo = new SignalRepository(contextMock.Object);
        }
    }
}
