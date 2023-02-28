using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EpilepsieDB.Web.View.ViewModels
{
    public class PatientDto
    {
        public int PatientID { get; set; }

        [Required]
        public string Acronym { get; set; }

        [Required]
        public IFormFile NiftiFile { get; set; }

        [Required]
        public IFormFile MriImage { get; set; }
    }
}
