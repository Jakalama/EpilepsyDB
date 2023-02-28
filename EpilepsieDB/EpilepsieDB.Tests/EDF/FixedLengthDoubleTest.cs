using EpilepsieDB.EDF;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.EDF
{
    public class FixedLengthDoubleTest : AbstractTest
    {
        public FixedLengthDoubleTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Value_ReturnsCorrect()
        {
            // set
            var expected = 0.123;
            var header = new FixedLengthDouble(EdfField.Version);
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
            var value = 0.123;
            var expected = "0,123";
            var header = new FixedLengthDouble(EdfField.Version);
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
            var value = 0.123;
            var expected = "0,123   ";
            var header = new FixedLengthDouble(EdfField.Version);
            header.Value = value;

            // act
            var result = header.ToAscii();

            // assert
            Assert.Equal(expected, result);
        }
    }
}
