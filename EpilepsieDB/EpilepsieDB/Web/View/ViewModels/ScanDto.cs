using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EpilepsieDB.Web.View.ViewModels
{
    public class ScanDto
    {
        public int ScanID { get; set; }

        [Required]
        public string ScanNumber { get; set; }

        public string PatientAcronym { get; set; }

        [Required]
        public IFormFile EdfFile { get; set; }

        [Required]
        public int RecordingID { get; set; }
    }
}
