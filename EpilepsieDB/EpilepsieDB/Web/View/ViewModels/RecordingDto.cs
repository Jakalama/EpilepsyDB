using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EpilepsieDB.Web.View.ViewModels
{
    public class RecordingDto
    {
        public int RecordingID { get; set; }

        [Required]
        public string RecordingNumber { get; set; }

        [Required]
        public int PatientID { get; set; }

        public string PatientAcronym { get; set; }

        public IEnumerable<ScanDto> Scans { get; set; }
    }
}
