using EpilepsieDB.Models;
using EpilepsieDB.Web.API.APIModels;
using Microsoft.AspNetCore.Http.Connections;
using System.Collections.Generic;

namespace EpilepsieDB.Web.API.Extensions
{
    public static class RecordingExtension
    {
        public static RecordingApiDto ToDto(this Recording recording)
        {
            return new RecordingApiDto()
            {
                RecordingID = recording.ID,
                RecordingNumber = recording.RecordingNumber,
                Patient = recording.Patient?.ToDto(),
            };
        }

        public static IEnumerable<RecordingApiDto> ToDtos(this IEnumerable<Recording> recordings)
        {
            List<RecordingApiDto> dtos = new List<RecordingApiDto>();

            foreach (Recording recording in recordings)
            {
                dtos.Add(recording.ToDto());
            }

            return dtos;
        }
    }
}
