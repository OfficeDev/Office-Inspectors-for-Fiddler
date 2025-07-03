using BlockParser;
using System.Collections.Generic;
using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the LargePropertyTagArray structure.
    /// 2.2.1 Common Data Types
    /// 2.2.1.8 LargePropertyTagArray Structure
    /// </summary>
    public class LargePropertyTagArray : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the number of structures contained in the PropertyTags field.
        /// </summary>
        public uint PropertyTagCount;

        /// <summary>
        /// An array of PropertyTag structures, each of which contains a property tag that specifies a property.
        /// </summary>
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the LargePropertyTagArray structure.
        /// </summary>
        /// <param name="s">A stream containing LargePropertyTagArray structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            PropertyTagCount = ReadUint();
            List<PropertyTag> tempPT = new List<PropertyTag>();

            for (int i = 0; i < PropertyTagCount; i++)
            {
                tempPT.Add(Block.Parse<PropertyTag>(s));
            }

            PropertyTags = tempPT.ToArray();
        }
    }
}