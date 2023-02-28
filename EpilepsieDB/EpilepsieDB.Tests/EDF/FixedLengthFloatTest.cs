using EpilepsieDB.EDF;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.EDF
{
    public class FixedLengthFloatTest : AbstractTest
    {
        public FixedLengthFloatTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Value_ReturnsCorrect()
        {
            // set
            var expected = 0.123f;
            var header = new FixedLengthFloat(EdfField.Version);
            header.Value = expected;

            // act
            var result = header.Value;

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToAscii_ContainsValue()
        {
            // set
            var value = 0.123f;
            var expected = "0,123";
            var header = new FixedLengthFloat(EdfField.Version);
            header.Value = value;

            // act
            var result = header.ToAscii();

            // assert
            Assert.Equal(expected, result.Trim());
        }

        [Fact]
        public void ToAscii_IsCorrectlyPadded()
        {
            // set
            var value = 0.123f;
            var expected = "0,123   ";
            var header = new FixedLengthFloat(EdfField.Version);
            header.Value = value;

            // act
            var result = header.ToAscii();

            // assert
            Assert.Equal(expected, result);
        }
    }
}
