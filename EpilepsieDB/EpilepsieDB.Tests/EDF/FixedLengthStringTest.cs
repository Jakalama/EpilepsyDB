using EpilepsieDB.EDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.EDF
{
    public class FixedLengthStringTest : AbstractTest
    {
        public FixedLengthStringTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Value_ReturnsValue_Trimmed()
        {
            // set
            var valueString = "  Huhu  ";
            var header = new FixedLengthString(EdfField.Version);
            header.Value = valueString;

            // act
            var result = header.Value;

            // assert
            Assert.Equal(valueString.Trim(), result);
        }

        [Fact]
        public void ToAscii_ContainsValue_IfSet()
        {
            // set
            var valueString = "Huhu";
            var header = new FixedLengthString(EdfField.Version);
            header.Value = valueString;

            // act
            var result = header.ToAscii();

            // assert
            Assert.Equal(valueString, result.Trim());
        }

        [Fact]
        public void ToAscii_ReturnsEmpty_IfValueNotSet()
        {
            // set
            var header = new FixedLengthString(EdfField.Version);

            // act
            var result = header.ToAscii();

            // assert
            Assert.Equal("", result.Trim());
        }

        [Fact]
        public void ToAscii_IsCorretlyPadded_IfValueSet()
        {
            // set
            var valueString = "Huhu";
            var header = new FixedLengthString(EdfField.Version);
            header.Value = valueString;

            // act
            var result = header.ToAscii();

            // assert
            Assert.Equal(8, result.Length);
        }

        [Fact]
        public void ToAscii_IsCorretlyPadded_IfValueNotSet()
        {
            // set
            var header = new FixedLengthString(EdfField.Version);

            // act
            var result = header.ToAscii();

            // assert
            Assert.Equal(8, result.Length);
        }
    }
}
