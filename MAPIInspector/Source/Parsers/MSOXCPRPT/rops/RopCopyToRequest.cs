using BlockParser;
using System.Collections.Generic;
using System.Windows.Forms.Design;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.11 RopCopyTo
    /// A class indicates the RopCopyTo ROP Request Buffer.
    /// </summary>
    public class RopCopyToRequest : Block
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
        /// A Boolean that specifies whether to copy subobjects.
        /// </summary>
        public BlockT<bool> WantSubObjects;

        /// <summary>
        /// A flags structure that contains flags that control the operation behavior.
        /// </summary>
        public BlockT<CopyFlags> CopyFlags;

        /// <summary>
        /// An unsigned integer that specifies how many tags are present in the ExcludedTags field.
        /// </summary>
        public BlockT<ushort> ExcludedTagCount;

        /// <summary>
        /// An array of PropertyTag structures that specifies the properties to exclude from the copy.
        /// </summary>
        public PropertyTag[] ExcludedTags;

        /// <summary>
        /// Parse the RopCopyToRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            SourceHandleIndex = ParseT<byte>();
            DestHandleIndex = ParseT<byte>();
            WantAsynchronous = ParseAs<byte, bool>();
            WantSubObjects = ParseAs<byte, bool>();
            CopyFlags = ParseT<CopyFlags>();
            ExcludedTagCount = ParseT<ushort>();
            var tmpExcludedTags = new List<PropertyTag>();

            for (int i = 0; i < ExcludedTagCount; i++)
            {
                tmpExcludedTags.Add(Parse<PropertyTag>());
            }

            ExcludedTags = tmpExcludedTags.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("RopCopyToRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(SourceHandleIndex, "SourceHandleIndex");
            AddChildBlockT(DestHandleIndex, "DestHandleIndex");
            AddChildBlockT(WantAsynchronous, "WantAsynchronous");
            AddChildBlockT(WantSubObjects, "WantSubObjects");
            AddChildBlockT(CopyFlags, "CopyFlags");
            AddChildBlockT(ExcludedTagCount, "ExcludedTagCount");
            AddLabeledChildren(ExcludedTags, "ExcludedTags");
        }
    }
}
