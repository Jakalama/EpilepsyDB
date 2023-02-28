using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EpilepsieDB.Models
{
    public class Scan : BaseModel
    {
        [Required]
        public int RecordingID { get; set; }

        public virtual Recording Recording { get; set; }

        [Required]
        public string EdfDisplayName { get; set; }

        [Required]
        public string EdfFilePath { get; set;}

        [Required]
        public string ScanNumber { get; set; }

        [Required]
        public string Version { get; set; }

        [Required]
        public string PatientInfo { get; set; }

        [Required]
        public string RecordInfo { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Start Time")]
        [DisplayFormat(DataFormatString = "{HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime StartTime { get; set; }

        [Required]
        public int NumberOfRecords { get; set; }

        [Required]
        public float DurationOfDataRecord { get; set; }

        [Required]
        public int NumberOfSignals { get; set; }

        [Required]
        public string Labels { get; set; }

        [Required]
        public string TransducerTypes { get; set; }

        [Required]
        public string PhysicalDimensions { get; set; }

        [Required]
        public ICollection<Block> Blocks { get; set; }

        [Required]
        public ICollection<Signal> Signals { get; set; }

        [Required]
        public ICollection<Annotation> Annotations { get; set; }

    }
}
