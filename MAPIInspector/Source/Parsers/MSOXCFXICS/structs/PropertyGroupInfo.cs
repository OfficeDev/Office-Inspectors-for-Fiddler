using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The PropertyGroupInfo class
    /// 2.2.2.8 PropertyGroupInfo
    /// </summary>
    public class PropertyGroupInfo : Block
    {
        /// <summary>
        /// An unsigned 32-bit integer value that identifies a property mapping within the current synchronization download context.
        /// </summary>
        public BlockT<uint> GroupId;

        /// <summary>
        /// A reserved field
        /// </summary>
        public BlockT<uint> Reserved;

        /// <summary>
        ///  An unsigned 32-bit integer value that specifies how many PropertyGroup structures are present in the Groups field.
        /// </summary>
        public BlockT<uint> GroupCount;

        /// <summary>
        /// An array of PropertyGroup structures,
        /// </summary>
        public PropertyGroup[] Groups;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        protected override void Parse()
        {
            GroupId = ParseT<uint>();
            Reserved = ParseT<uint>();
            GroupCount = ParseT<uint>();
            var tmpGrp = new List<PropertyGroup>();
            for (int i = 0; i < GroupCount; i++)
            {
                tmpGrp.Add(Parse<PropertyGroup>());
            }

            Groups = tmpGrp.ToArray();
        }

        protected override void ParseBlocks()
        {
            Text = "PropertyGroupInfo";
            AddChildBlockT(GroupId, "GroupId");
            AddChild(Reserved, "Reserved:0x00000000");
            AddChildBlockT(GroupCount, "GroupCount");
            foreach (var group in Groups)
            {
                AddChild(group);
            }
        }
    }
}
