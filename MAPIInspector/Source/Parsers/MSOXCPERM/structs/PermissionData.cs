using BlockParser;
using System.Collections.Generic;
using System.Security.Permissions;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2 RopModifyPermissions ROP
    /// A class indicates the PermissionData.
    /// </summary>
    public class PermissionData : Block
    {
        /// <summary>
        /// A set of flags that specify the type of change to be made to the folder permissions.
        /// </summary>
        public BlockT<PermissionDataFlags> PermissionDataFlags;

        /// <summary>
        /// An integer that specifies the number of structures contained in the PropertyValues field.
        /// </summary>
        public BlockT<ushort> PropertyValueCount;

        /// <summary>
        /// An array of TaggedPropertyValue structures ([MS-OXCDATA] section 2.11.4).
        /// </summary>
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the PermissionData structure.
        /// </summary>
        protected override void Parse()
        {
            PermissionDataFlags = ParseT<PermissionDataFlags>();
            PropertyValueCount = ParseT<ushort>();
            var listPropertyValues = new List<TaggedPropertyValue>();

            for (int i = 0; i < PropertyValueCount; i++)
            {
                TaggedPropertyValue tempPropertyValue = new TaggedPropertyValue();
                tempPropertyValue.Parse(parser);
                listPropertyValues.Add(tempPropertyValue);
            }

            PropertyValues = listPropertyValues.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("PermissionData");
            AddChildBlockT(PermissionDataFlags, "PermissionDataFlags");
            AddChildBlockT(PropertyValueCount, "PropertyValueCount");
            AddLabeledChildren(PropertyValues, "PropertyValues");
        }
    }
}
