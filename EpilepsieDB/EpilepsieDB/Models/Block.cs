using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EpilepsieDB.Models
{
    public class Block : BaseModel
    {
        [Required]
        public int ScanID { get; set; }

        public virtual Scan Scan { get; set; }

        [Required]
        public DateTime Starttime { get; set; }

        [Required]
        public DateTime Endtime { get; set; }

        [Required]
        public float GapToPrevious { get; set; }
    }
}
