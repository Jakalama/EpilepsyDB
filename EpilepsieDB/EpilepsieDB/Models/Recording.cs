using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EpilepsieDB.Models
{
    public class Recording : BaseModel
    {
        [Required]
        public int PatientID { get; set; }

        public virtual Patient Patient { get; set; }

        [Required]
        public string RecordingNumber { get; set; }

        [Required]
        public string ContentDir { get; set; }

        public virtual ICollection<Scan> Scans { get; set; }
    }
}
