namespace EpilepsieDB.EDF
{
    public class FixedLengthFloat : HeaderValue<float>
    {
        public float Value
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

        public FixedLengthFloat(EdfField name) : base(name) { }

        public override string ToAscii()
        {
            return value.ToString().PadRight(AsciiLength, ' ');
        }
    }
}
