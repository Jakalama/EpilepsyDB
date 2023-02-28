using EpilepsieDB.Models;
using EpilepsieDB.Web.View.ViewModels;
using System.Collections.Generic;

namespace EpilepsieDB.Web.View.Extensions
{
    public static class PatientExtension
    {
        public static PatientDto ToDto(this Patient patient)
        {
            PatientDto dto = new PatientDto()
            {
                PatientID = patient.ID,
                Acronym = patient.Acronym
            };

            return dto;
        }

        public static PatientDetailDto ToDetailDto(this Patient patient)
        {
            PatientDetailDto dto = new PatientDetailDto()
            {
                PatientID = patient.ID,
                Acronym = patient.Acronym,
                MriImagePath = patient.MriImagePath,
            };

            return dto;
        }

        public static PatientEditDto ToEditDto(this Patient patient)
        {
            PatientEditDto dto = new PatientEditDto()
            {
                PatientID = patient.ID,
                Acronym = patient.Acronym,
                MriImagePath = patient.MriImagePath,
            };

            return dto;
        }

        public static Patient ToModel(this PatientEditDto dto)
        {
            Patient model = new Patient()
            {
                ID = dto.PatientID,
                Acronym = dto.Acronym
            };

            return model;
        }

        public static IEnumerable<PatientDto> ToDtos(this IEnumerable<Patient> patients)
        {
            List<PatientDto> dtos = new List<PatientDto>();

            foreach (var patient in patients)
            {
                dtos.Add(patient.ToDto());
            }

            return dtos;
        }

        public static Patient ToModel(this PatientDto dto)
        {
            return new Patient
            {
                ID = dto.PatientID,
                Acronym = dto.Acronym
            };
        }
    }
}
