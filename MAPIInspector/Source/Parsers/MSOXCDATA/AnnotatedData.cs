namespace MAPIInspector.Parsers
{
    using System.Collections.Generic;

    /// <summary>
    /// The AnnotatedData class. Base class for annotated data. Allows us to specify an alternate display string for parsed data
    /// </summary>
    public class AnnotatedData : BaseStructure
    {
        public string ParsedValue { get; set; }

        /// <summary>
        /// Size of the data
        /// </summary>
        public virtual int Size { get; } = 0;

        /// <summary>
        /// By overriding ToString, we can display the parsed string instead of the raw data
        /// </summary>
        public override string ToString() => ParsedValue;

        /// <summary>
        /// Alternate parsings for display
        /// </summary>
        public Dictionary<string, string> parsedValues = new Dictionary<string, string>();
        public string this[string key]
        {
            get
            {
                return parsedValues.ContainsKey(key) ? parsedValues[key] : string.Empty;
            }
            set
            {
                parsedValues[key] = value;
            }
        }
    }
}