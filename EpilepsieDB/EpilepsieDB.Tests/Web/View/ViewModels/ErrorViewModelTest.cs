using EpilepsieDB.Web.View.ViewModels;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Web.View.ViewModels
{
    public class ErrorViewModelTest : AbstractTest
    {
        public ErrorViewModelTest(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [InlineData("321")]
        [InlineData("123")]
        public void RequestId_ReturnsCorrect(string expected)
        {
            // set
            var model = new ErrorViewModel();
            model.RequestId = expected;

            // act
            var result = model.RequestId;

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ShowRequestId_ReturnsTrue_IfRequestIdIsSet()
        {
            // set
            var model = new ErrorViewModel();
            model.RequestId = "123";

            // act
            var result = model.ShowRequestId;

            // assert
            Assert.True(result);
        }

        [Fact]
        public void ShowRequestId_ReturnsFalse_IfRequestIdIsNotSet()
        {
            // set
            var model = new ErrorViewModel();
            model.RequestId = null;

            // act
            var result = model.ShowRequestId;

            // assert
            Assert.False(result);
        }
    }
}
