using EpilepsieDB.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Data
{
    public class SeedDataTest : AbstractTest
    {
        public SeedDataTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Initialize_CallsSaveChanges()
        {
            // set
            var context = new Mock<IAppDbContext>();

            // act
            await SeedData.Initialize(context.Object);

            // assert
            context.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken
                >()), Times.Never());
        }
    }
}
