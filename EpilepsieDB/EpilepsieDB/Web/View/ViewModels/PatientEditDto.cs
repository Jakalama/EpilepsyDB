using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EpilepsieDB.Web.View.ViewModels
{
    public class PatientEditDto
    {
        [Required]
        public int PatientID { get; set; }

        [Required]
        public string Acronym { get; set; }

        public IFormFile NiftiFile { get; set; }

        public IFormFile MriImage { get; set; }

        public string MriImagePath { get; set; }

    }
}
