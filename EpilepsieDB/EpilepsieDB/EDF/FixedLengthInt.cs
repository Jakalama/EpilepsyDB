namespace EpilepsieDB.EDF
{
    public class FixedLengthInt : HeaderValue<short>
    {
        public short Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        public FixedLengthInt(EdfField name) : base(name) { }

        public override string ToAscii()
        {
            return value.ToString().PadRight(AsciiLength, ' ');
        }
    }
}
