namespace EpilepsieDB.EDF
{
    public class FixedLengthString : HeaderValue<string>
    {
        public string Value
        {
            get
            {
                return this.value.Trim();
            }
            set
            {
                this.value = value.Trim();
            }
        }

        public FixedLengthString(EdfField name) : base(name) { }

        public override string ToAscii()
        {
            string asciiString = "";
            if (value != null)
                asciiString = value.PadRight(AsciiLength, ' ');
            else
                asciiString = asciiString.PadRight(AsciiLength, ' ');

            return asciiString;
        }
    }
}
