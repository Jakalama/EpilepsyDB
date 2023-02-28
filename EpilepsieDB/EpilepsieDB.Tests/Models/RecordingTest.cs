using EpilepsieDB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Models
{
    public class RecordingTest : AbstractTest
    {
        public RecordingTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ID_GetSet()
        {
            // set
            var recording = new Recording();
            var expected = 1;

            // act
            recording.ID = expected;

            // assert
            Assert.Equal(expected, recording.ID);
        }

        [Fact]
        public void PatientID_GetSet()
        {
            // set
            var recording = new Recording();
            var expected = 1;

            // act
            recording.PatientID = expected;

            // assert
            Assert.Equal(expected, recording.PatientID);
        }

        [Fact]
        public void Patient_GetSet()
        {
            // set
            var recording = new Recording();
            var expected = new Patient();

            // act
            recording.Patient = expected;

            // assert
            Assert.Equal(expected, recording.Patient);
        }

        [Fact]
        public void RecordingNumber_GetSet()
        {
            // set
            var recording = new Recording();
            var expected = "test";

            // act
            recording.RecordingNumber = expected;

            // assert
            Assert.Equal(expected, recording.RecordingNumber);
        }

        [Fact]
        public void ContentDir_GetSet()
        {
            // set
            var recording = new Recording();
            var expected = "test";

            // act
            recording.ContentDir = expected;

            // assert
            Assert.Equal(expected, recording.ContentDir);
        }

        [Fact]
        public void Scans_GetSet()
        {
            // set
            var recording = new Recording();
            var expected = new List<Scan>()
            {
                new Scan()
            };

            // act
            recording.Scans = expected;

            // assert
            Assert.Equal(expected, recording.Scans);
        }

        [Fact]
        public void PatientID_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("PatientID", typeof(Recording)));
        }

        [Fact]
        public void RecordingNumber_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("RecordingNumber", typeof(Recording)));
        }

        [Fact]
        public void ContentDir_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("ContentDir", typeof(Recording)));
        }
    }
}
