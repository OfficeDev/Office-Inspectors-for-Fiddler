using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The PropertyGroup.
    /// 2.2.2.8.1 PropertyGroup
    /// </summary>
    public class PropertyGroup : Block
    {
        /// <summary>
        /// An unsigned 32-bit integer value that specifies how many PropertyTag structures are present in the PropertyTags field. 
        /// </summary>
        public BlockT<uint> PropertyTagCount;

        /// <summary>
        /// An array of PropertyTagWithGroupPropertyName structures.
        /// </summary>
        public PropertyTagWithGroupPropertyName[] PropertyTags;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        protected override void Parse()
        {
            PropertyTagCount = ParseT<uint>();
            var tags = new List<PropertyTagWithGroupPropertyName>();
            for (int i = 0; i < PropertyTagCount.Data; i++)
            {
                tags.Add(Parse<PropertyTagWithGroupPropertyName>());
            }

            PropertyTags = tags.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("PropertyGroup");
            AddChildBlockT(PropertyTagCount, "PropertyTagCount");
            if (PropertyTags != null)
            {
                foreach (var tag in PropertyTags)
                {
                    AddChild(tag);
                }
            }
        }
    }
}
