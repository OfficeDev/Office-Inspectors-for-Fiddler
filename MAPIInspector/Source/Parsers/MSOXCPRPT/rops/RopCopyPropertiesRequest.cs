using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.10 RopCopyProperties
    /// A class indicates the RopCopyProperties ROP Request Buffer.
    /// </summary>
    public class RopCopyPropertiesRequest : Block
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
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the source Server object is stored.
        /// </summary>
        public BlockT<byte> SourceHandleIndex;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the destination Server object is stored.
        /// </summary>
        public BlockT<byte> DestHandleIndex;

        /// <summary>
        /// A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP
        /// </summary>
        public BlockT<bool> WantAsynchronous;

        /// <summary>
        /// A flags structure that contains flags that control the operation behavior.
        /// </summary>
        public BlockT<CopyFlags> CopyFlags;

        /// <summary>
        /// An unsigned integer that specifies how many tags are present in the PropertyTags field.
        /// </summary>
        public BlockT<ushort> PropertyTagCount;

        /// <summary>
        /// An array of PropertyTag structures that specifies the properties to copy.
        /// </summary>
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopCopyPropertiesRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            SourceHandleIndex = ParseT<byte>();
            DestHandleIndex = ParseT<byte>();
            WantAsynchronous = ParseAs<byte, bool>();
            CopyFlags = ParseT<CopyFlags>();
            PropertyTagCount = ParseT<ushort>();
            var tmpPropertyTags = new List<PropertyTag>();

            for (int i = 0; i < PropertyTagCount; i++)
            {
                tmpPropertyTags.Add(Parse<PropertyTag>());
            }

            PropertyTags = tmpPropertyTags.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("RopCopyPropertiesRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(SourceHandleIndex, "SourceHandleIndex");
            AddChildBlockT(DestHandleIndex, "DestHandleIndex");
            AddChildBlockT(WantAsynchronous, "WantAsynchronous");
            AddChildBlockT(CopyFlags, "CopyFlags");
            AddChildBlockT(PropertyTagCount, "PropertyTagCount");
            AddLabeledChildren(PropertyTags, "PropertyTags");
        }
    }
}
