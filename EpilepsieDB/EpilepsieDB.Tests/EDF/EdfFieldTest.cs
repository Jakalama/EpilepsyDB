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
    public class EdfFieldTest : AbstractTest
    {
        public EdfFieldTest(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [InlineData(EdfField.Version, 8)]
        [InlineData(EdfField.PatientID, 80)]
        [InlineData(EdfField.RecordID, 80)]
        [InlineData(EdfField.StartDate, 8)]
        [InlineData(EdfField.StartTime, 8)]
        [InlineData(EdfField.NumberOfBytesInHeader, 8)]
        [InlineData(EdfField.Reserved, 44)]
        [InlineData(EdfField.NumberOfDataRecords, 8)]
        [InlineData(EdfField.DurationOfDataRecord, 8)]
        [InlineData(EdfField.NumberOfSignals, 4)]
        [InlineData(EdfField.Labels, 16)]
        [InlineData(EdfField.TransducerType, 80)]
        [InlineData(EdfField.PhysicalDimension, 8)]
        [InlineData(EdfField.PhysicalMinimum, 8)]
        [InlineData(EdfField.PhysicalMaximum, 8)]
        [InlineData(EdfField.DigitalMinimum, 8)]
        [InlineData(EdfField.DigitalMaximum, 8)]
        [InlineData(EdfField.Prefiltering, 80)]
        public void EdfFieldDictionary_Contains(EdfField field, int expected)
        {
            // act
            var result = EdfFields.Map[field];

            // assert
            Assert.Equal(expected, result);
        }
    }
}
