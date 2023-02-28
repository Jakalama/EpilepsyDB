namespace EpilepsieDB.Web.API.APIModels
{
    public class ScanApiDto
    {
        public int ScanID { get; set; }

        public string ScanNumber { get; set; }

        public string Version { get; set; }

        public string PatientInfo { get; set; }

        public string RecordInfo { get; set; }

        public string StartDate { get; set; }

        public string StartTime { get; set; }

        public int NumberOfRecords { get; set; }

        public float DurationOfDataRecord { get; set; }

        public int NumberOfSignals { get; set; }

        public string Labels { get; set; }

        public string TransducerTypes { get; set; }

        public string PhysicalDimensions { get; set; }

        public RecordingApiDto Recording { get; set; }
    }
}