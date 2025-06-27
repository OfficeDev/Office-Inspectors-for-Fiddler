using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the RopFastTransferSourceCopyProperties ROP Request Buffer.
    /// 2.2.3.1.1.2.1 RopFastTransferSourceCopyProperties ROP Request Buffer
    /// </summary>
    public class RopFastTransferSourceCopyPropertiesRequest : Block
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
        /// An unsigned integer that specifies whether descendant subobjects are copied
        /// </summary>
        public BlockT<byte> Level;

        /// <summary>
        /// A flags structure that contains flags that control the type of operation. 
        /// </summary>
        public BlockT<CopyFlags_CopyProperties> CopyFlags;

        /// <summary>
        /// A flags structure that contains flags that control the behavior of the operation. 
        /// </summary>
        public BlockT<SendOptions> SendOptions;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the PropertyTags field.
        /// </summary>
        public BlockT<ushort> PropertyTagCount;

        /// <summary>
        /// An array of PropertyTag structures that specifies the properties to exclude during the copy.
        /// </summary>
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopFastTransferSourceCopyPropertiesRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            OutputHandleIndex = ParseT<byte>();
            Level = ParseT<byte>();
            CopyFlags = ParseT<CopyFlags_CopyProperties>();
            SendOptions = ParseT<SendOptions>();
            PropertyTagCount = ParseT<ushort>();
            var interTag = new List<PropertyTag>();
            for (int i = 0; i < PropertyTagCount; i++)
            {
                interTag.Add(Parse<PropertyTag>());
            }

            PropertyTags = interTag.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("RopFastTransferSourceCopyPropertiesRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            AddChildBlockT(Level, "Level");
            AddChildBlockT(CopyFlags, "CopyFlags");
            AddChildBlockT(SendOptions, "SendOptions");
            AddChildBlockT(PropertyTagCount, "PropertyTagCount");
            AddLabeledChildren(PropertyTags, "PropertyTags");
        }
    }
}