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
    public class EdfSignalHeaderTest : AbstractTest
    {
        public EdfSignalHeaderTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Label_HasCorrectAsciiLength()
        {
            // set
            var header = new EdfSignalHeader();

            // act
            var result = header.Label.AsciiLength;

            // assert
            Assert.Equal(EdfFields.Map[EdfField.Labels], result);
        }

        [Fact]
        public void TransducerType_HasCorrectAsciiLength()
        {
            // set
            var header = new EdfSignalHeader();

            // act
            var result = header.TransducerType.AsciiLength;

            // assert
            Assert.Equal(EdfFields.Map[EdfField.TransducerType], result);
        }

        [Fact]
        public void PhysicalDimension_HasCorrectAsciiLength()
        {
            // set
            var header = new EdfSignalHeader();

            // act
            var result = header.PhysicalDimension.AsciiLength;

            // assert
            Assert.Equal(EdfFields.Map[EdfField.PhysicalDimension], result);
        }

        [Fact]
        public void PhysicalMinimum_HasCorrectAsciiLength()
        {
            // set
            var header = new EdfSignalHeader();

            // act
            var result = header.PhysicalMinimum.AsciiLength;

            // assert
            Assert.Equal(EdfFields.Map[EdfField.PhysicalMinimum], result);
        }

        [Fact]
        public void PhysicalMaximum_HasCorrectAsciiLength()
        {
            // set
            var header = new EdfSignalHeader();

            // act
            var result = header.PhysicalMaximum.AsciiLength;

            // assert
            Assert.Equal(EdfFields.Map[EdfField.PhysicalMaximum], result);
        }

        [Fact]
        public void DigitalMinimum_HasCorrectAsciiLength()
        {
            // set
            var header = new EdfSignalHeader();

            // act
            var result = header.DigitalMinimum.AsciiLength;

            // assert
            Assert.Equal(EdfFields.Map[EdfField.DigitalMinimum], result);
        }

        [Fact]
        public void DigitalMaximum_HasCorrectAsciiLength()
        {
            // set
            var header = new EdfSignalHeader();

            // act
            var result = header.DigitalMaximum.AsciiLength;

            // assert
            Assert.Equal(EdfFields.Map[EdfField.DigitalMaximum], result);
        }

        [Fact]
        public void Prefiltering_HasCorrectAsciiLength()
        {
            // set
            var header = new EdfSignalHeader();

            // act
            var result = header.Prefiltering.AsciiLength;

            // assert
            Assert.Equal(EdfFields.Map[EdfField.Prefiltering], result);
        }

        [Fact]
        public void NumberOfSamples_HasCorrectAsciiLength()
        {
            // set
            var header = new EdfSignalHeader();

            // act
            var result = header.NumberOfSamples.AsciiLength;

            // assert
            Assert.Equal(EdfFields.Map[EdfField.NumberOfSamplesInDataRecord], result);
        }

        [Fact]
        public void Reserved_HasCorrectAsciiLength()
        {
            // set
            var header = new EdfSignalHeader();

            // act
            var result = header.Reserved.AsciiLength;

            // assert
            Assert.Equal(EdfFields.Map[EdfField.SignalsReserved], result);
        }
    }
}
