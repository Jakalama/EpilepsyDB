using EpilepsieDB.Models;
using EpilepsieDB.Web.View.ViewModels;
using System.Collections.Generic;

namespace EpilepsieDB.Web.View.Extensions
{
    public static class RecordingExtension
    {
        public static Recording ToModel(this RecordingDto dto, Patient patient = null)
        {
            Recording recording = new Recording();
            recording.ID = dto.RecordingID;
            recording.PatientID = dto.PatientID;
            recording.Patient = patient;
            recording.RecordingNumber = dto.RecordingNumber;

            return recording;
        }

        public static RecordingDto ToDto(this Recording model, IEnumerable<ScanDto> scans = null)
        {
            RecordingDto dto = new RecordingDto();
            dto.RecordingID = model.ID;
            dto.PatientID = model.PatientID;
            dto.RecordingNumber = model.RecordingNumber;
            dto.PatientAcronym = model.Patient?.Acronym;
            dto.Scans = scans;

            return dto;
        }

        public static RecordingDetailDto ToDetailDto(this Recording model, IEnumerable<ScanDto> scans)
        {
            RecordingDetailDto dto = new RecordingDetailDto();
            dto.RecordingID = model.ID;
            dto.PatientID = model.PatientID;
            dto.RecordingNumber = model.RecordingNumber;
            dto.PatientAcronym = model.Patient?.Acronym;
            dto.Scans = scans;

            return dto;
        }

        public static IEnumerable<RecordingDto> ToDtos(this IEnumerable<Recording> models)
        {
            List<RecordingDto> list = new List<RecordingDto>();

            foreach (var model in models)
            {
                list.Add(model.ToDto());
            }

            return list;
        }
    }
}
