using BlockParser;
using System.Collections.Generic;
using System.Windows.Forms.Design;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.13 RopGetNamesFromPropertyIds
    /// A class indicates the RopGetNamesFromPropertyIds ROP Request Buffer.
    /// </summary>
    public class RopGetNamesFromPropertyIdsRequest : Block
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
        /// An unsigned integer that specifies the number of integers contained in the PropertyIds field.
        /// </summary>
        public BlockT<ushort> PropertyIdCount;

        /// <summary>
        /// An array of unsigned 16-bit integers.
        /// </summary>
        public BlockT<ushort>[] PropertyIds;

        /// <summary>
        /// Parse the RopGetNamesFromPropertyIdsRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            PropertyIdCount = ParseT<ushort>();
            var tmpPropertyIds = new List<BlockT<ushort>>();

            for (int i = 0; i < PropertyIdCount; i++)
            {
                tmpPropertyIds.Add(ParseT<ushort>());
            }
            PropertyIds = tmpPropertyIds.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("RopGetNamesFromPropertyIdsRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(PropertyIdCount, "PropertyIdCount");
            AddLabeledChildren(PropertyIds, "PropertyIds");
        }
    }
}
