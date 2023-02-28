using EpilepsieDB.Source.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests
{
    public class RandomNameTest : AbstractTest
    {
        public RandomNameTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Generate_ReturnsPseudoRandomName_OfAtLeast10Chars()
        {
            // set
            var length = 10;

            // act
            var result = RandomName.Generate(1);

            // assert
            Assert.Equal(length, result.Length);
        }


        [Fact]
        public void Generate_ReturnsPseudoRandomName_WithAtLeast3Digits()
        {
            // set
            var numDigits = 3;

            // act
            var result = RandomName.Generate();
            var digits = result.Count(Char.IsDigit);

            // assert
            Assert.Equal(numDigits, digits);
        }

        [Fact]
        public void Generate_ReturnsPseudoRandomName_WithAtLeast4Chars()
        {
            // set
            var numChars = 4;

            // act
            var result = RandomName.Generate();
            var chars = result.Count(c => Char.IsLetter(c));

            // assert
            Assert.Equal(numChars, chars);
        }

        [Fact]
        public void Generate_ReturnsPseudoRandomName_WithAtLeast3Abst()
        {
            // set
            var numChars = 3;

            // act
            var result = RandomName.Generate();
            var chars = result.Count(c => !Char.IsLetterOrDigit(c));

            // assert
            Assert.Equal(numChars, chars);
        }

        [Fact]
        public void Generate_ReturnsPseudoRandomName_WithLength()
        {
            // set
            var length = 123;

            // act
            var result = RandomName.Generate(length);

            // assert
            Assert.Equal(length, result.Length);
        }

        [Fact]
        public void Generate_ReturnsPseudoRandomName_WhichAreNotEqual()
        {
            // act
            var result1 = RandomName.Generate();
            var result2 = RandomName.Generate();

            // assert
            Assert.NotEqual(result1, result2);
        }
    }
}
