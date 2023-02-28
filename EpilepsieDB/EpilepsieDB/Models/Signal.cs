using System.ComponentModel.DataAnnotations;

namespace EpilepsieDB.Models
{
    public class Signal : BaseModel
    {
        [Required]
        public int ScanID { get; set; }

        public virtual Scan Scan { get; set; }

        [Required]
        public int Channel { get; set; }

        [Required]
        public string Label { get; set; }

        [Required]
        public string TransducerType { get; set; }

        [Required]
        public string PhysicalDimension { get; set; }

        [Required]
        public double PhysicalMinimum { get; set; }

        [Required]
        public double PhysicalMaximum { get; set; }

        [Required]
        public short DigitalMinimum { get; set; }

        [Required]
        public short DigitalMaximum { get; set; }

        [Required]
        public string Prefiltering { get; set; }

        [Required]
        public string Reserved { get; set; }

        [Required]
        public int NumberOfSamples { get; set; }
    }
}
