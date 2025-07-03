using MapiInspector;
using System.Collections.Generic;
using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AddressBookPropertyRow structure.
    /// 2.2.1 Common Data Types
    /// 2.2.1.7 AddressBookPropertyRow Structure
    /// </summary>
    public class AddressBookPropertyRow : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that indicates whether all property values are present and without error in the ValueArray field.
        /// </summary>
        public byte Flags;

        /// <summary>
        /// An array of variable-sized structures.
        /// </summary>
        public object[] ValueArray;

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
        /// <param name="s">A stream containing AddressBookPropertyRow structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            Flags = ReadByte();
            List<object> result = new List<object>();

            if (largePropTagArray is LargePropertyTagArray)
            {
                foreach (var propTag in largePropTagArray.PropertyTags)
                {
                    object addrRowValue = null;

                    if (Flags == 0x00)
                    {
                        if (propTag.PropertyType != PropertyDataType.PtypUnspecified)
                        {
                            AddressBookPropertyValue propValue = new AddressBookPropertyValue(propTag.PropertyType, ptypMultiCountSize);
                            propValue.Parse(s);
                            propValue.PropertyTag = $"{propTag.PropertyType}:{Utilities.EnumToString(propTag.PropertyId.Data)}";
                            addrRowValue = propValue;
                        }
                        else
                        {
                            AddressBookTypedPropertyValue typePropValue = new AddressBookTypedPropertyValue();
                            typePropValue.Parse(s);
                            typePropValue.PropertyTag = $"{propTag.PropertyType}:{Utilities.EnumToString(propTag.PropertyId.Data)}";
                            addrRowValue = typePropValue;
                        }
                    }
                    else if (Flags == 0x01)
                    {
                        if (propTag.PropertyType != PropertyDataType.PtypUnspecified)
                        {
                            AddressBookFlaggedPropertyValue flagPropValue = new AddressBookFlaggedPropertyValue(propTag.PropertyType);
                            flagPropValue.Parse(s);
                            flagPropValue.PropertyTag = $"{propTag.PropertyType}:{Utilities.EnumToString(propTag.PropertyId.Data)}";
                            addrRowValue = flagPropValue;
                        }
                        else
                        {
                            AddressBookFlaggedPropertyValueWithType flagPropValue = new AddressBookFlaggedPropertyValueWithType();
                            flagPropValue.Parse(s);
                            flagPropValue.PropertyTag = $"{propTag.PropertyType}:{Utilities.EnumToString(propTag.PropertyId.Data)}";
                            addrRowValue = flagPropValue;
                        }
                    }

                    result.Add(addrRowValue);
                }
            }

            ValueArray = result.ToArray();
        }
    }
}