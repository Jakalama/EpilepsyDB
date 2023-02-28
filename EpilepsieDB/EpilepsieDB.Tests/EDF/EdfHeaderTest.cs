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
    public class EdfHeaderTest : AbstractTest
    {
        public EdfHeaderTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void IsEdfPlus_IsInitialized()
        {
            // act
            var header = new EdfHeader();

            // assert
            Assert.False(header.IsEdfPlus);
        }

        [Fact]
        public void IsInterupted_IsInitialized()
        {
            // act
            var header = new EdfHeader();

            // assert
            Assert.False(header.IsInterupted);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsEdfPlus_ReturnsCorrect(bool expected)
        {
            // set
            var header = new EdfHeader();
            header.IsEdfPlus = expected;

            // act
            var result = header.IsEdfPlus;

            // assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsInterupted_ReturnsCorrect(bool expected)
        {
            // set
            var header = new EdfHeader();
            header.IsInterupted = expected;

            // act
            var result = header.IsInterupted;

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SignalHeaders_ReturnsCorrect()
        {
            // set
            var expected = new EdfSignalHeader[0];
            var header = new EdfHeader();
            header.SignalHeaders = expected;

            // act
            var result = header.SignalHeaders;

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Version_HasCorrectAsciiLength()
        {
            // set
            var header = new EdfHeader();

            // act
            var result = header.Version.AsciiLength;

            // assert
            Assert.Equal(EdfFields.Map[EdfField.Version], result);
        }

        [Fact]
        public void PatientID_HasCorrectAsciiLength()
        {
            // set
            var header = new EdfHeader();

            // act
            var result = header.PatientID.AsciiLength;

            // assert
            Assert.Equal(EdfFields.Map[EdfField.PatientID], result);
        }

        [Fact]
        public void RecordID_HasCorrectAsciiLength()
        {
            // set
            var header = new EdfHeader();

            // act
            var result = header.RecordID.AsciiLength;

            // assert
            Assert.Equal(EdfFields.Map[EdfField.RecordID], result);
        }

        [Fact]
        public void StartDate_HasCorrectAsciiLength()
        {
            // set
            var header = new EdfHeader();

            // act
            var result = header.StartDate.AsciiLength;

            // assert
            Assert.Equal(EdfFields.Map[EdfField.StartDate], result);
        }

        [Fact]
        public void StartTime_HasCorrectAsciiLength()
        {
            // set
            var header = new EdfHeader();

            // act
            var result = header.StartTime.AsciiLength;

            // assert
            Assert.Equal(EdfFields.Map[EdfField.StartTime], result);
        }

        [Fact]
        public void NumberOfBytesInHeader_HasCorrectAsciiLength()
        {
            // set
            var header = new EdfHeader();

            // act
            var result = header.NumberOfBytesInHeader.AsciiLength;

            // assert
            Assert.Equal(EdfFields.Map[EdfField.NumberOfBytesInHeader], result);
        }

        [Fact]
        public void Reserved_HasCorrectAsciiLength()
        {
            // set
            var header = new EdfHeader();

            // act
            var result = header.Reserved.AsciiLength;

            // assert
            Assert.Equal(EdfFields.Map[EdfField.Reserved], result);
        }

        [Fact]
        public void NumberOfDataRecords_HasCorrectAsciiLength()
        {
            // set
            var header = new EdfHeader();

            // act
            var result = header.NumberOfDataRecords.AsciiLength;

            // assert
            Assert.Equal(EdfFields.Map[EdfField.NumberOfDataRecords], result);
        }

        [Fact]
        public void DurationOfDataRecord_HasCorrectAsciiLength()
        {
            // set
            var header = new EdfHeader();

            // act
            var result = header.DurationOfDataRecord.AsciiLength;

            // assert
            Assert.Equal(EdfFields.Map[EdfField.DurationOfDataRecord], result);
        }

        [Fact]
        public void NumberOfSignals_HasCorrectAsciiLength()
        {
            // set
            var header = new EdfHeader();

            // act
            var result = header.NumberOfSignals.AsciiLength;

            // assert
            Assert.Equal(EdfFields.Map[EdfField.NumberOfSignals], result);
        }
    }
}
