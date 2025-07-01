using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.3.1 Property Values
    /// 2.3.1.12 PropertyValue_r
    /// A class indicates the PropertyValue_r structure.
    /// </summary>
    public class PropertyValue_r : BaseStructure
    {
        /// <summary>
        /// Encodes the PropTag of the property whose value is represented by the PropertyValue_r data structure.
        /// </summary>
        public uint UlPropTag;

        /// <summary>
        /// Reserved. All clients and servers MUST set value to the constant 0x00000000.
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// Encodes the actual value of the property represented by the PropertyValue_r data structure.
        /// </summary>
        public PROP_VAL_UNION Value;

        /// <summary>
        /// Parse the PropertyValue_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            UlPropTag = ReadUint();
            Reserved = ReadUint();
            Value = new PROP_VAL_UNION((int)UlPropTag & 0XFFFF);
            Value.Parse(s);
        }
    }
}
