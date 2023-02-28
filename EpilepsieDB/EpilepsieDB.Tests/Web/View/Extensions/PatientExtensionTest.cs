using EpilepsieDB.Models;
using EpilepsieDB.Web.View.Extensions;
using EpilepsieDB.Web.View.ViewModels;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Web.View.Extensions
{
    public class PatientExtensionTest : AbstractTest
    {
        public PatientExtensionTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ToDto()
        {
            int id = 1;
            string acronym = "123";

            // set
            Patient patient = new Patient()
            {
                ID = id,
                Acronym = acronym
            };

            // act
            PatientDto dto = patient.ToDto();

            // assert
            Assert.Equal(id, dto.PatientID);
            Assert.Equal(acronym, dto.Acronym);
        }

        [Fact]
        public void ToModel()
        {
            int id = 1;
            string acronym = "123";

            // set
            PatientDto dto = new PatientDto()
            {
                PatientID = id,
                Acronym = acronym
            };

            // act
            Patient patient = dto.ToModel();

            // assert
            Assert.Equal(id, patient.ID);
            Assert.Equal(acronym, patient.Acronym);
        }
    }
}
