namespace MAPIInspector.Parsers
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// 2.1 AddressList Structures
    /// 2.1.1 AddressEntry Structure
    /// </summary>
    public class AddressEntry : BaseStructure
    {
        /// <summary>
        /// An unsigned integer whose value is equal to the number of associated TaggedPropertyValue structures, as specified in section 2.11.4.
        /// </summary>
        public uint PropertyCount;

        /// <summary>
        /// A set of TaggedPropertyValue structures representing one addressee.
        /// </summary>
        public TaggedPropertyValue[] Values;

        /// <summary>
        /// Parse the AddressEntry structure.
        /// </summary>
        /// <param name="s">A stream containing the AddressEntry structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            PropertyCount = ReadUint();
            List<TaggedPropertyValue> tempArray = new List<TaggedPropertyValue>();
            for (int i = 0; i < PropertyCount; i++)
            {
                TaggedPropertyValue tempproperty = new TaggedPropertyValue();
                tempproperty.Parse(s);
                tempArray.Add(tempproperty);
            }

            Values = tempArray.ToArray();
        }
    }
}
