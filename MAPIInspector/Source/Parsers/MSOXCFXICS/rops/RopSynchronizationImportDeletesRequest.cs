using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the RopSynchronizationImportDeletes ROP Request Buffer.
    /// 2.2.3.2.4.5.1 RopSynchronizationImportDeletes ROP Request Buffer
    /// </summary>
    public class RopSynchronizationImportDeletesRequest : Block
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
        /// A flags structure that contains flags that specify options for the imported deletions.
        /// </summary>
        public BlockT<ImportDeleteFlags> ImportDeleteFlags;

        /// <summary>
        /// An unsigned integer that specifies the number of structures present in the PropertyValues field.
        /// </summary>
        public BlockT<ushort> PropertyValueCount;

        /// <summary>
        /// An array of TaggedPropertyValue structures that specify the folders or messages to delete.
        /// </summary>
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the RopSynchronizationImportDeletesRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            ImportDeleteFlags = ParseT<ImportDeleteFlags>();
            PropertyValueCount = ParseT<ushort>();
            var interValue = new List<TaggedPropertyValue>();

            for (int i = 0; i < PropertyValueCount; i++)
            {
                var value = new TaggedPropertyValue();
                value.Parse(parser);
                interValue.Add(value);
            }

            PropertyValues = interValue.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("RopSynchronizationImportDeletesRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(ImportDeleteFlags, "ImportDeleteFlags");
            AddChildBlockT(PropertyValueCount, "PropertyValueCount");
            AddLabeledChildren(PropertyValues, "PropertyValues");
        }
    }
}
