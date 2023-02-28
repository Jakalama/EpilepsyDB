namespace EpilepsieDB.EDF
{
    public class FixedLengthDouble : HeaderValue<double>
    {
        public double Value
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

        public FixedLengthDouble(EdfField name) : base(name) { }

        public override string ToAscii()
        {
            return value.ToString().PadRight(AsciiLength, ' ');
        }
    }
}
