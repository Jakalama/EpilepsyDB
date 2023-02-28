using EpilepsieDB.Models;
using System;
using System.ComponentModel.DataAnnotations;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Models
{
    public class BaseModelTest : AbstractTest
    {
        public BaseModelTest(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void ID_ReturnsCorrect(int expected)
        {
            // set
            var block = new Block();
            block.ID = expected;

            // act
            var result = block.ID;

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ID_IsKey()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<KeyAttribute>("ID", typeof(BaseModel)));
        }
    }
}
