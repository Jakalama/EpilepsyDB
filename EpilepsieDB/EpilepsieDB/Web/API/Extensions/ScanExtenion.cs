using EpilepsieDB.Models;
using EpilepsieDB.Web.API.APIModels;
using System.Collections.Generic;

namespace EpilepsieDB.Web.API.Extensions
{
    public static class ScanExtenion
    {
        public static ScanApiDto ToDto(this Scan scan)
        {
            return new ScanApiDto()
            {
                ScanID = scan.ID,
                Recording = scan.Recording?.ToDto(),
                ScanNumber = scan.ScanNumber,
                Version = scan.Version,
                PatientInfo = scan.PatientInfo,
                RecordInfo = scan.RecordInfo,
                StartDate = scan.StartDate.ToShortDateString(),
                StartTime = scan.StartTime.ToShortTimeString(),
                NumberOfRecords = scan.NumberOfRecords,
                DurationOfDataRecord = scan.DurationOfDataRecord,
                NumberOfSignals = scan.NumberOfSignals,
                Labels = scan.Labels,
                TransducerTypes = scan.TransducerTypes,
                PhysicalDimensions = scan.PhysicalDimensions
            };
        }

        public static IEnumerable<ScanApiDto> ToDtos(this IEnumerable<Scan> scans)
        {
            List<ScanApiDto> dtos = new List<ScanApiDto>();

            foreach (Scan scan in scans)
            {
                dtos.Add(scan.ToDto());
            }

            return dtos;
        }
    }
}
