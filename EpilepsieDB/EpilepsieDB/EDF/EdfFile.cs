using System.IO;

namespace EpilepsieDB.EDF
{
    public class EdfFile
    {
        public EdfHeader Header { get; set; }

        public float[] BlockOffsets { get; set; }

        public EdfSignal[] Signals { get; set; }

        public EdfAnnotation[] Annotations { get; set; }

        public EdfFile()
        {
            Header = new EdfHeader();
            BlockOffsets = new float[] { };
            Signals = new EdfSignal[] { };
            Annotations = new EdfAnnotation[] { };
        }

        public EdfFile(EdfHeader header, float[] blockOffsets, EdfSignal[] signals, EdfAnnotation[] annotations)
        {
            Header = header;
            BlockOffsets = blockOffsets;
            Signals = signals;
            Annotations = annotations;
        }
    }
}
