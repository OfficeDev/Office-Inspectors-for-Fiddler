namespace MAPIInspector.Parsers
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    ///  2.1.2 AddressList Structure
    /// </summary>
    public class AddressList : BaseStructure
    {
        /// <summary>
        /// An unsigned integer whose value is equal to the number of associated addressees.
        /// </summary>
        public uint AddressCount;

        /// <summary>
        /// An array of AddressEntry structures. The number of structures is indicated by the AddressCount field.
        /// </summary>
        public AddressEntry[] Addresses;

        /// <summary>
        /// Parse the AddressList structure.
        /// </summary>
        /// <param name="s">A stream containing the AddressList structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            AddressCount = ReadUint();
            List<AddressEntry> tempArray = new List<AddressEntry>();
            for (int i = 0; i < AddressCount; i++)
            {
                AddressEntry tempAddress = new AddressEntry();
                tempAddress.Parse(s);
                tempArray.Add(tempAddress);
            }

            Addresses = tempArray.ToArray();
        }
    }
}
