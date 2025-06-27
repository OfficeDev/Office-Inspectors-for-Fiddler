using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.1.2 AddressList Structure
    /// </summary>
    public class AddressList : Block
    {
        /// <summary>
        /// An unsigned integer whose value is equal to the number of associated addressees.
        /// </summary>
        public BlockT<uint> AddressCount;

        /// <summary>
        /// An array of AddressEntry structures. The number of structures is indicated by the AddressCount field.
        /// </summary>
        public AddressEntry[] Addresses;

        /// <summary>
        /// Parse the AddressList structure.
        /// </summary>
        protected override void Parse()
        {
            AddressCount = ParseT<uint>();
            var tempArray = new List<AddressEntry>();
            for (int i = 0; i < AddressCount; i++)
            {
                tempArray.Add(Parse<AddressEntry>());
            }

            Addresses = tempArray.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("AddressList");
            AddChildBlockT(AddressCount, "AddressCount");
            AddLabeledChildren(Addresses, "Addresses");
        }
    }
}
