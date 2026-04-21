using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AddressBookPropertyRow structure.
    /// [MS-OXCMAPIHTTP] 2.2.1 Common Data Types
    /// [MS-OXCMAPIHTTP] 2.2.1.7 AddressBookPropertyRow Structure
    /// </summary>
    public class AddressBookPropertyRow : Block
    {
        /// <summary>
        /// An unsigned integer that indicates whether all property values are present and without error in the ValueArray field.
        /// </summary>
        public BlockT<byte> Flags;

        /// <summary>
        /// An array of variable-sized structures.
        /// </summary>
        public Block[] ValueArray;

        /// <summary>
        /// The LargePropertyTagArray type used to initialize the constructed function.
        /// </summary>
        private LargePropertyTagArray largePropTagArray;

        /// <summary>
        /// Initializes a new instance of the AddressBookPropertyRow class.
        /// </summary>
        /// <param name="largePropTagArray">The LargePropertyTagArray value</param>
        public AddressBookPropertyRow(LargePropertyTagArray largePropTagArray)
        {
            this.largePropTagArray = largePropTagArray;
        }

        /// <summary>
        /// Parse the AddressBookPropertyRow structure.
        /// </summary>
        protected override void Parse()
        {
            Flags = ParseT<byte>();
            var result = new List<Block>();
            foreach (var propTag in largePropTagArray.PropertyTags)
            {
                Block addrRowValue = null;
                if (Flags == 0x00)
                {
                    if (propTag.PropertyType != PropertyDataType.PtypUnspecified)
                    {
                        var propValue = new AddressBookPropertyValue(propTag.PropertyType);
                        propValue.Parse(parser);
                        addrRowValue = propValue;
                    }
                    else
                    {
                        addrRowValue = Parse<AddressBookTypedPropertyValue>();
                    }
                }
                else if (Flags == 0x01)
                {
                    if (propTag.PropertyType != PropertyDataType.PtypUnspecified)
                    {
                        var flagPropValue = new AddressBookFlaggedPropertyValue(propTag.PropertyType);
                        flagPropValue.Parse(parser);
                        addrRowValue = flagPropValue;
                    }
                    else
                    {
                        addrRowValue = Parse<AddressBookFlaggedPropertyValueWithType>();
                    }
                }

                if (addrRowValue != null)
                {
                    addrRowValue.AddChild(propTag);
                    result.Add(addrRowValue);
                }
            }

            ValueArray = result.ToArray();
        }

        protected override void ParseBlocks()
        {
            Text = "AddressBookPropertyRow";
            AddChildBlockT(Flags, "Flags");
            AddLabeledChildren(ValueArray, "ValueArray");
        }
    }
}
