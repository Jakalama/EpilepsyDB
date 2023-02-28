using System.Linq;
using System.Collections.Generic;

namespace EpilepsieDB.EDF
{
    public class EdfSignalHeader
    {
        public FixedLengthString Label { get; } = new FixedLengthString(EdfField.Labels);
        public FixedLengthString TransducerType { get; } = new FixedLengthString(EdfField.TransducerType);
        public FixedLengthString PhysicalDimension { get; } = new FixedLengthString(EdfField.PhysicalDimension);
        public FixedLengthDouble PhysicalMinimum { get; } = new FixedLengthDouble(EdfField.PhysicalMinimum);
        public FixedLengthDouble PhysicalMaximum { get; } = new FixedLengthDouble(EdfField.PhysicalMaximum);
        public FixedLengthInt DigitalMinimum { get; } = new FixedLengthInt(EdfField.DigitalMinimum);
        public FixedLengthInt DigitalMaximum { get; } = new FixedLengthInt(EdfField.DigitalMaximum);
        public FixedLengthString Prefiltering { get; } = new FixedLengthString(EdfField.Prefiltering);
        public FixedLengthInt NumberOfSamples{ get; } = new FixedLengthInt(EdfField.NumberOfSamplesInDataRecord);
        public FixedLengthString Reserved { get; } = new FixedLengthString(EdfField.SignalsReserved);
    }
}
