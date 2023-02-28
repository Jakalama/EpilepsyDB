using System.ComponentModel.DataAnnotations;

namespace EpilepsieDB.Models
{
    public class Annotation : BaseModel
    {
        [Required]
        public int ScanID { get; set; }

        public virtual Scan Scan { get; set; }

        [Required]
        public float Offset { get; set;}

        [Required]
        public float Duration { get; set;}

        [Required]
        public string Description { get; set;}
    }
}
