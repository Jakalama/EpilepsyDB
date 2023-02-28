using System.Collections.Generic;

namespace EpilepsieDB.Web.View.ViewModels
{
    public class RecordingDetailDto
    {
        public int RecordingID { get; set; }

        public string RecordingNumber { get; set; }

        public int PatientID { get; set; }

        public string PatientAcronym { get; set; }

        public IEnumerable<ScanDto> Scans { get; set; }
    }
}
