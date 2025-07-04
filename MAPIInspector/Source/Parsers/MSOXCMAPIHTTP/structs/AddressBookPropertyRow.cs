using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AddressBookPropertyRow structure.
    /// 2.2.1 Common Data Types
    /// 2.2.1.7 AddressBookPropertyRow Structure
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
        /// The ptypMultiCountSize type used to initialize the constructed function.
        /// </summary>
        private CountWideEnum ptypMultiCountSize;

        /// <summary>
        /// Initializes a new instance of the AddressBookPropertyRow class.
        /// </summary>
        /// <param name="largePropTagArray">The LargePropertyTagArray value</param>
        /// <param name="ptypMultiCountSize">The ptypMultiCountSize value</param>
        public AddressBookPropertyRow(LargePropertyTagArray largePropTagArray, CountWideEnum ptypMultiCountSize = CountWideEnum.fourBytes)
        {
            this.largePropTagArray = largePropTagArray;
            this.ptypMultiCountSize = ptypMultiCountSize;
        }

        /// <summary>
        /// Parse the AddressBookPropertyRow structure.
        /// </summary>
        protected override void Parse()
        {
            Flags = ParseT<byte>();
            var result = new List<Block>();

            if (largePropTagArray is LargePropertyTagArray)
            {
                foreach (var propTag in largePropTagArray.PropertyTags)
                {
                    Block addrRowValue = null;
                    if (Flags == 0x00)
                    {
                        if (propTag.PropertyType != PropertyDataType.PtypUnspecified)
                        {
                            var propValue = new AddressBookPropertyValue(propTag.PropertyType, ptypMultiCountSize);
                            propValue.Parse(parser);
                            //propValue.PropertyTag = $"{propTag.PropertyType}:{Utilities.EnumToString(propTag.PropertyId.Data)}";
                            addrRowValue = propValue;
                        }
                        else
                        {
                            //typePropValue.PropertyTag = $"{propTag.PropertyType}:{Utilities.EnumToString(propTag.PropertyId.Data)}";
                            addrRowValue = Parse<AddressBookTypedPropertyValue>();
                        }
                    }
                    else if (Flags == 0x01)
                    {
                        if (propTag.PropertyType != PropertyDataType.PtypUnspecified)
                        {
                            var flagPropValue = new AddressBookFlaggedPropertyValue(propTag.PropertyType);
                            flagPropValue.Parse(parser);
                            //flagPropValue.PropertyTag = $"{propTag.PropertyType}:{Utilities.EnumToString(propTag.PropertyId.Data)}";
                            addrRowValue = flagPropValue;
                        }
                        else
                        {
                            //flagPropValue.PropertyTag = $"{propTag.PropertyType}:{Utilities.EnumToString(propTag.PropertyId.Data)}";
                            addrRowValue = Parse<AddressBookFlaggedPropertyValueWithType>();
                        }
                    }

                    result.Add(addrRowValue);
                }
            }

            ValueArray = result.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("AddressBookPropertyRow");
            AddChildBlockT(Flags, "Flags");
            AddLabeledChildren(ValueArray, "ValueArray");
        }
    }
}