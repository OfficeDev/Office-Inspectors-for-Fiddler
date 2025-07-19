using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the RopSynchronizationImportMessageChange ROP Request Buffer.
    /// 2.2.3.2.4.2.1 RopSynchronizationImportMessageChange ROP Request Buffer
    /// </summary>
    public class RopSynchronizationImportMessageChangeRequest : Block
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
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        /// </summary>
        public BlockT<byte> OutputHandleIndex;

        /// <summary>
        /// A flags structure that contains flags that control the behavior of the synchronization.
        /// </summary>
        public BlockT<ImportFlag> ImportFlag;

        /// <summary>
        /// An unsigned integer that specifies the number of structures present in the PropertyValues field.
        /// </summary>
        public BlockT<ushort> PropertyValueCount;

        /// <summary>
        /// An array of TaggedPropertyValue structures that specify extra properties on the message.
        /// </summary>
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the RopSynchronizationImportMessageChangeRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            OutputHandleIndex = ParseT<byte>();
            ImportFlag = ParseT<ImportFlag>();
            PropertyValueCount = ParseT<ushort>();

            var interValue = new List<TaggedPropertyValue>();
            for (int i = 0; i < PropertyValueCount; i++)
            {
                var tagValue = new TaggedPropertyValue();
                tagValue.Parse(parser);
                interValue.Add(tagValue);
            }
            PropertyValues = interValue.ToArray();
        }

        protected override void ParseBlocks()
        {
            Text = "RopSynchronizationImportMessageChangeRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            AddChildBlockT(ImportFlag, "ImportFlag");
            AddChildBlockT(PropertyValueCount, "PropertyValueCount");
            AddLabeledChildren(PropertyValues, "PropertyValues");
        }
    }
}
