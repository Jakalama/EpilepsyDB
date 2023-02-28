using System.Collections.Generic;

namespace EpilepsieDB.EDF
{
    public class EdfBlock
    {
        public float Offset { get; set; }

        public List<EdfAnnotation> Annotations { get; set; } = new List<EdfAnnotation>();
    }
}
