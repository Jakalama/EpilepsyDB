namespace EpilepsieDB.EDF
{
    public abstract class HeaderValue<T>
    {
        protected T value;

        public int AsciiLength { get; set; }

        public HeaderValue(EdfField fieldName)
        {
            AsciiLength = EdfFields.Map[fieldName];
        }

        public abstract string ToAscii();
    }
}
