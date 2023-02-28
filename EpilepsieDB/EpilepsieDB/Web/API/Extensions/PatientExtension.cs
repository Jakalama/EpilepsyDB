using EpilepsieDB.Models;
using EpilepsieDB.Web.API.APIModels;
using System.Collections.Generic;
using System.Linq;

namespace EpilepsieDB.Web.API.Extensions
{
    public static class PatientExtension
    {
        public static PatientApiDto ToDto(this Patient patient)
        {
            return new PatientApiDto
            {
                PatientID = patient.ID,
                Acronym = patient.Acronym
            };
        }

        public static IEnumerable<PatientApiDto> ToDtos(this IEnumerable<Patient> patients)
        {
            List<PatientApiDto> dtos = new List<PatientApiDto>();

            foreach (Patient patient in patients)
            {
                dtos.Add(patient.ToDto());
            }

            return dtos;
        }
    }
}
