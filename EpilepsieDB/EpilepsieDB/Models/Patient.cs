using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EpilepsieDB.Models
{
    public class Patient : BaseModel
    {
        [Required]
        public string Acronym { get; set; }

        [Required]
        public string ContentDir { get; set; }

        [Required]
        public string NiftiFilePath { get; set; }

        [Required]
        public string MriImagePath { get; set; }

        public virtual ICollection<Recording> Recordings { get; set; }
    }
}
