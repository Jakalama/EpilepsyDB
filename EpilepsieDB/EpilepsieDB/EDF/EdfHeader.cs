namespace EpilepsieDB.EDF
{
    public class EdfHeader
    {
        public EdfHeader() { }

        public FixedLengthString Version { get; } = new FixedLengthString(EdfField.Version);
        public FixedLengthString PatientID { get; } = new FixedLengthString(EdfField.PatientID);
        public FixedLengthString RecordID { get; } = new FixedLengthString(EdfField.RecordID);
        public FixedLengthString StartDate { get; } = new FixedLengthString(EdfField.StartDate);
        public FixedLengthString StartTime { get; } = new FixedLengthString(EdfField.StartTime);
        public FixedLengthInt NumberOfBytesInHeader { get; } = new FixedLengthInt(EdfField.NumberOfBytesInHeader);
        public FixedLengthString Reserved { get; } = new FixedLengthString(EdfField.Reserved);
        public FixedLengthInt NumberOfDataRecords { get; } = new FixedLengthInt(EdfField.NumberOfDataRecords);
        public FixedLengthFloat DurationOfDataRecord { get; } = new FixedLengthFloat(EdfField.DurationOfDataRecord);
        public FixedLengthInt NumberOfSignals { get; } = new FixedLengthInt(EdfField.NumberOfSignals);

        public bool IsEdfPlus { get; set; } = false;
        public bool IsInterupted { get; set; } = false;

        public EdfSignalHeader[] SignalHeaders { get; set; }
    }
}
