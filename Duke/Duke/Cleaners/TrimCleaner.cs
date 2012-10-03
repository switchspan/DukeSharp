namespace Duke.Cleaners
{
    /// <summary>
    /// A cleaner which removes leading and trailing whitespace, without
    /// making any other changes.
    /// </summary>
    public class TrimCleaner : ICleaner
    {
        public string Clean(string value)
        {
            value = value.Trim();
            if (value.Equals(""))
                return null;

            return value;
        }
    }
}
