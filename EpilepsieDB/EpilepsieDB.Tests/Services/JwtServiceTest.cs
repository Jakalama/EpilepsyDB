using EpilepsieDB.Services.Impl;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Services
{
    public class JwtServiceTest : AbstractTest
    {
        public JwtServiceTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor()
        {
            // set
            var configMock = new Mock<IConfiguration>();

            // act
            var service = new JwtService(configMock.Object);
        }
    }
}
