namespace MAPIInspector.Parsers
{
    using BlockParser;

    /// <summary>
    /// 2.11.2 Property Value Structures
    /// 2.11.2.1 PropertyValue Structure
    /// </summary>
    public class PropertyValue : Block
    {
        /// <summary>
        /// A PropertyValue structure, as specified in section 2.11.2. The value MUST be compatible with the value of the propertyType field.
        /// </summary>
        public Block Value;

        /// <summary>
        /// The Count wide size of ptypMutiple type.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// An unsigned integer that specifies the data type of the property value, according to the table in section 2.11.1.
        /// </summary>
        private PropertyDataType propertyType;

        /// <summary>
        /// Bool value indicates if this property value is for address book.
        /// </summary>
        private readonly bool isAddressBook;

        /// <summary>
        /// Initializes a new instance of the PropertyValue class
        /// </summary>
        /// <param name="_propertyType">The property type</param>
        /// <param name="ptypMultiCountSize">The Count wide size of ptypMutiple type</param>
        /// <param name="addressBook">Whether it is AddressBook related property</param>
        public PropertyValue(PropertyDataType _propertyType, CountWideEnum ptypMultiCountSize = CountWideEnum.twoBytes, bool addressBook = false)
        {
            countWide = ptypMultiCountSize;
            propertyType = _propertyType;
            isAddressBook = addressBook;
        }

        /// <summary>
        /// Parse the PropertyValue structure.
        /// </summary>
        protected override void Parse()
        {
            Value = ReadPropertyValue(propertyType, parser, countWide);
        }

        protected override void ParseBlocks()
        {
            SetText("PropertyValue");
            if (Value != null)
            {
                AddChild(Value, $"Value:{Value.Text}");
            }
            else
            {
                SetText("MSOXCDATA: Not implemented type definition - PropertyValue");
            }
        }

        /// <summary>
        /// The method to return the object of PropertyValue.
        /// </summary>
        /// <param name="dataType">The Property data type.</param>
        /// <param name="parser">A BinaryParser containing the PropertyValue structure</param>
        /// <param name="ptypMultiCountSize">The Count wide size of ptypMutiple type.</param>
        /// <returns>The object of PropertyValue.</returns>
        static public Block ReadPropertyValue(PropertyDataType dataType, BinaryParser parser, CountWideEnum ptypMultiCountSize = CountWideEnum.twoBytes, bool bIsAddressBook = false)
        {
            switch (dataType)
            {
                case PropertyDataType.PtypInteger16: return Parse<PtypInteger16>(parser);
                case PropertyDataType.PtypInteger32: return Parse<PtypInteger32>(parser);
                case PropertyDataType.PtypFloating32: return Parse<PtypFloating32>(parser);
                case PropertyDataType.PtypFloating64: return Parse<PtypFloating64>(parser);
                case PropertyDataType.PtypCurrency: return Parse<PtypCurrency>(parser);
                case PropertyDataType.PtypFloatingTime: return Parse<PtypFloatingTime>(parser);
                case PropertyDataType.PtypErrorCode: return Parse<PtypErrorCode>(parser);
                case PropertyDataType.PtypBoolean: return Parse<PtypBoolean>(parser);
                case PropertyDataType.PtypInteger64: return Parse<PtypInteger64>(parser);
                case PropertyDataType.PtypString: return Parse<PtypString>(parser);
                case PropertyDataType.PtypString8: return Parse<PtypString8>(parser);
                case PropertyDataType.PtypTime: return Parse<PtypTime>(parser);
                case PropertyDataType.PtypGuid: return Parse<PtypGuid>(parser);
                case PropertyDataType.PtypServerId: return Parse<PtypServerId>(parser);
                case PropertyDataType.PtypRestriction:
                    {
                        var tempPropertyValue = new RestrictionType(ptypMultiCountSize);
                        tempPropertyValue.Parse(parser);
                        return tempPropertyValue;
                    }
                case PropertyDataType.PtypRuleAction:
                    {
                        var tempPropertyValue = new RuleAction(ptypMultiCountSize);
                        tempPropertyValue.Parse(parser);
                        return tempPropertyValue;
                    }
                case PropertyDataType.PtypUnspecified: return Parse<PtypUnspecified>(parser);
                case PropertyDataType.PtypNull: return Parse<PtypNull>(parser);
                case PropertyDataType.PtypBinary:
                    {
                        var tempPropertyValue = new PtypBinary(ptypMultiCountSize);
                        tempPropertyValue.Parse(parser);
                        return tempPropertyValue;
                    }
                case PropertyDataType.PtypMultipleInteger16: return Parse<PtypMultipleInteger16>(parser);
                case PropertyDataType.PtypMultipleInteger32: return Parse<PtypMultipleInteger32>(parser);
                case PropertyDataType.PtypMultipleFloating32: return Parse<PtypMultipleFloating32>(parser);
                case PropertyDataType.PtypMultipleFloating64: return Parse<PtypMultipleFloating64>(parser);
                case PropertyDataType.PtypMultipleCurrency: return Parse<PtypMultipleCurrency>(parser);
                case PropertyDataType.PtypMultipleFloatingTime: return Parse<PtypMultipleFloatingTime>(parser);
                case PropertyDataType.PtypMultipleInteger64: return Parse<PtypMultipleInteger64>(parser);
                case PropertyDataType.PtypMultipleString:
                    {
                        if (bIsAddressBook)
                        {
                            var tempPropertyValue = new PtypMultipleString_AddressBook(ptypMultiCountSize);
                            tempPropertyValue.Parse(parser);
                            return tempPropertyValue;
                        }
                        else
                        {
                            return Parse<PtypMultipleString>(parser);
                        }
                    }
                case PropertyDataType.PtypMultipleString8: return Parse<PtypMultipleString8>(parser);
                case PropertyDataType.PtypMultipleTime: return Parse<PtypMultipleTime>(parser);
                case PropertyDataType.PtypMultipleGuid: return Parse<PtypMultipleGuid>(parser);
                case PropertyDataType.PtypMultipleBinary:
                    {
                        var tempPropertyValue = new PtypMultipleBinary(ptypMultiCountSize);
                        tempPropertyValue.Parse(parser);
                        return tempPropertyValue;
                    }

                case PropertyDataType.PtypObject_Or_PtypEmbeddedTable: return Parse<PtypObject_Or_PtypEmbeddedTable>(parser);
            }

            return null;
        }
    }
}
