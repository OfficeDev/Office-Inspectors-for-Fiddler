using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the RopSynchronizationImportHierarchyChange ROP Request Buffer.
    /// 2.2.3.2.4.3.1 RopSynchronizationImportHierarchyChange ROP Request Buffer
    /// </summary>
    public class RopSynchronizationImportHierarchyChangeRequest : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public BlockT<byte> LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the number of structures present in the HierarchyValues field.
        /// </summary>
        public BlockT<ushort> HierarchyValueCount;

        /// <summary>
        /// An array of TaggedPropertyValue structures that specify hierarchy-related properties of the folder.
        /// </summary>
        public TaggedPropertyValue[] HierarchyValues;

        /// <summary>
        /// An unsigned integer that specifies the number of structures present in the PropertyValues field.
        /// </summary>
        public BlockT<ushort> PropertyValueCount;

        /// <summary>
        /// An array of TaggedPropertyValue structures that specify the folders or messages to delete.
        /// </summary>
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the RopSynchronizationImportHierarchyChangeRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            HierarchyValueCount = ParseT<ushort>();

            var interHierarchyValues = new List<TaggedPropertyValue>();
            for (int i = 0; i < HierarchyValueCount; i++)
            {
                var val = new TaggedPropertyValue();
                val.Parse(parser);
                interHierarchyValues.Add(val);
            }
            HierarchyValues = interHierarchyValues.ToArray();

            PropertyValueCount = ParseT<ushort>();

            var interValue = new List<TaggedPropertyValue>();
            for (int i = 0; i < PropertyValueCount; i++)
            {
                var val = new TaggedPropertyValue();
                val.Parse(parser);
                interValue.Add(val);
            }
            PropertyValues = interValue.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("RopSynchronizationImportHierarchyChangeRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(HierarchyValueCount, "HierarchyValueCount");
            AddLabeledChildren(HierarchyValues, "HierarchyValues");
            AddChildBlockT(PropertyValueCount, "PropertyValueCount");
            AddLabeledChildren(PropertyValues, "PropertyValues");
        }
    }
}
