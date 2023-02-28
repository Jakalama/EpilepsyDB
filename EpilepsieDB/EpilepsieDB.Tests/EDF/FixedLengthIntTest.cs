using EpilepsieDB.EDF;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.EDF
{
    public class FixedLengthIntTest : AbstractTest
    {
        public FixedLengthIntTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Value_ReturnsCorrect()
        {
            // set
            short expected = 123;
            var header = new FixedLengthInt(EdfField.Version);
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
            short value = 123;
            var expected = "123";
            var header = new FixedLengthInt(EdfField.Version);
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
            short value = 123;
            var expected = "123     ";
            var header = new FixedLengthInt(EdfField.Version);
            header.Value = value;

            // act
            var result = header.ToAscii();

            // assert
            Assert.Equal(expected, result);
        }
    }
}
