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
    public class PatientTest : AbstractTest
    {
        public PatientTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ID_GetSet()
        {
            // set
            var patient = new Patient();
            var expected = 1;

            // act
            patient.ID = expected;

            // assert
            Assert.Equal(expected, patient.ID);
        }

        [Fact]
        public void Acronym_GetSet()
        {
            // set
            var patient = new Patient();
            var expected = "test";

            // act
            patient.Acronym = expected;

            // assert
            Assert.Equal(expected, patient.Acronym);
        }

        [Fact]
        public void ContentDir_GetSet()
        {
            // set
            var patient = new Patient();
            var expected = "test";

            // act
            patient.ContentDir = expected;

            // assert
            Assert.Equal(expected, patient.ContentDir);
        }

        [Fact]
        public void NiftiFilePath_GetSet()
        {
            // set
            var patient = new Patient();
            var expected = "test";

            // act
            patient.NiftiFilePath = expected;

            // assert
            Assert.Equal(expected, patient.NiftiFilePath);
        }

        [Fact]
        public void NiftiImagePath_GetSet()
        {
            // set
            var patient = new Patient();
            var expected = "test";

            // act
            patient.MriImagePath = expected;

            // assert
            Assert.Equal(expected, patient.MriImagePath);
        }

        [Fact]
        public void Recordings_GetSet()
        {
            // set
            var patient = new Patient();
            var expected = new List<Recording>()
            {
                new Recording()
            };

            // act
            patient.Recordings = expected;

            // assert
            Assert.Equal(expected, patient.Recordings);
        }

        [Fact]
        public void Acronym_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("Acronym", typeof(Patient)));
        }

        [Fact]
        public void ContentDir_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("ContentDir", typeof(Patient)));
        }

        [Fact]
        public void NiftiFilePath_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("NiftiFilePath", typeof(Patient)));
        }

        [Fact]
        public void MriImagePath_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("MriImagePath", typeof(Patient)));
        }
    }
}
