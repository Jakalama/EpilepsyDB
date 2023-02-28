using System.Collections.Generic;

namespace EpilepsieDB.EDF
{
    public enum EdfField
    {
        // fixed length
        Version,
        PatientID,
        RecordID,
        StartDate,
        StartTime,
        NumberOfBytesInHeader,
        Reserved,
        NumberOfDataRecords,
        DurationOfDataRecord,
        NumberOfSignals,

        // variable length
        Labels,
        TransducerType,
        PhysicalDimension,
        PhysicalMinimum,
        PhysicalMaximum,
        DigitalMinimum,
        DigitalMaximum,
        Prefiltering,
        NumberOfSamplesInDataRecord,
        SignalsReserved
    }

    internal static class EdfFields
    {
        public static readonly Dictionary<EdfField, int> Map = new Dictionary<EdfField, int>
        {
            {EdfField.Version, 8 },
            {EdfField.PatientID, 80 },
            {EdfField.RecordID, 80 },
            {EdfField.StartDate, 8 },
            {EdfField.StartTime, 8 },
            {EdfField.NumberOfBytesInHeader, 8 },
            {EdfField.Reserved, 44 },
            {EdfField.NumberOfDataRecords, 8},
            {EdfField.DurationOfDataRecord, 8 },
            {EdfField.NumberOfSignals, 4 },

            {EdfField.Labels, 16 },
            {EdfField.TransducerType, 80 },
            {EdfField.PhysicalDimension, 8 },
            {EdfField.PhysicalMinimum, 8 },
            {EdfField.PhysicalMaximum, 8 },
            {EdfField.DigitalMinimum, 8 },
            {EdfField.DigitalMaximum, 8 },
            {EdfField.Prefiltering, 80 },
            {EdfField.NumberOfSamplesInDataRecord, 8 },
            {EdfField.SignalsReserved, 32 },
        };
    }
}