using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.8 Property Row Structures
    /// 2.8.1 PropertyRow Structures
    /// </summary>
    public class PropertyRow : Block
    {
        /// <summary>
        /// An unsigned integer. This value indicate if all property values are present and without error.
        /// </summary>
        public BlockT<byte> Flag;

        /// <summary>
        /// An array of variable-sized structures.
        /// </summary>
        public Block[] ValueArray;

        /// <summary>
        /// Bytes as byte array.
        /// </summary>
        public BlockBytes bytes;

        /// <summary>
        /// The array of property tag.
        /// </summary>
        private PropertyTag[] propTags;

        /// <summary>
        /// Predetermined size of structure, used in ropNotify
        /// </summary>
        private int size;

        /// <summary>
        /// Initializes a new instance of the PropertyRow class
        /// </summary>
        /// <param name="_propTags">The array of property tag.</param>
        public PropertyRow(PropertyTag[] _propTags)
        {
            propTags = _propTags;
        }

        /// <summary>
        /// Initializes a new instance of the PropertyRow class
        /// </summary>
        /// <param name="size">The size of the structure.</param>
        /// <param name="_propTags">The array of property tag.</param>
        public PropertyRow(int _size, PropertyTag[] _propTags)
        {
            size = _size;
            propTags = _propTags;
        }

        /// <summary>
        /// Parse the PropertyRow structure.
        /// </summary>
        protected override void Parse()
        {
            Flag = ParseT<byte>();
            var tempPropArray = new List<Block>();
            if (propTags != null)
            {
                foreach (var tempPropTag in propTags)
                {
                    Block rowPropValue = null;
                    tempPropTag.PropertyType.Data = BaseStructure.ConvertToPropType((ushort)tempPropTag.PropertyType.Data);

                    if (Flag.Data == 0x00)
                    {
                        if (tempPropTag.PropertyType.Data != PropertyDataType.PtypUnspecified)
                        {
                            var propValue = new PropertyValue(tempPropTag.PropertyType.Data);
                            propValue.Parse(parser);
                            propValue.AddHeader($"PropertyTag: {tempPropTag.PropertyType}:{MapiInspector.Utilities.EnumToString(tempPropTag.PropertyId.Data)}");
                            rowPropValue = propValue;
                        }
                        else
                        {
                            var typePropValue = new TypedPropertyValue();
                            typePropValue.Parse(parser);
                            typePropValue.AddHeader($"PropertyTag: {tempPropTag.PropertyType}:{MapiInspector.Utilities.EnumToString(tempPropTag.PropertyId.Data)}");
                            rowPropValue = typePropValue;
                        }
                    }
                    else if (Flag.Data == 0x01)
                    {
                        if (tempPropTag.PropertyType.Data != PropertyDataType.PtypUnspecified)
                        {
                            var flagPropValue = new FlaggedPropertyValue(tempPropTag.PropertyType.Data);
                            flagPropValue.Parse(parser);
                            flagPropValue.AddHeader($"PropertyTag: {tempPropTag.PropertyType}:{MapiInspector.Utilities.EnumToString(tempPropTag.PropertyId.Data)}");
                            rowPropValue = flagPropValue;
                        }
                        else
                        {
                            var flagPropValue = new FlaggedPropertyValueWithType();
                            flagPropValue.Parse(parser);
                            flagPropValue.AddHeader($"PropertyTag: {tempPropTag.PropertyType}:{MapiInspector.Utilities.EnumToString(tempPropTag.PropertyId.Data)}");
                            rowPropValue = flagPropValue;
                        }
                    }

                    tempPropArray.Add(rowPropValue);
                }
            }
            else if (size > 0)
            {
                bytes = ParseBytes(size - 1);
            }

            ValueArray = tempPropArray.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("PropertyRow");
            AddChildBlockT(Flag, "Flag");

            AddChildBytes(bytes, "Bytes");
            foreach (var propValue in ValueArray)
            {
                if (propValue != null) AddChild(propValue, $"{propValue.GetType().Name}");
            }
        }
    }
}
