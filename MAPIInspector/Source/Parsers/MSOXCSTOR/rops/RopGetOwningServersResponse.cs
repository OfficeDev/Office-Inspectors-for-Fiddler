using System.Collections.Generic;
using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.3.6.2 RopGetOwningServers ROP Success Response Buffer
    /// [MS-OXCROPS] 2.2.3.6.3 RopGetOwningServers ROP Failure Response Buffer
    /// A class indicates the RopGetOwningServers ROP Response Buffer.
    /// </summary>
    public class RopGetOwningServersResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<ErrorCodes> ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of strings in the OwningServers field.
        /// </summary>
        public BlockT<ushort> OwningServersCount;

        /// <summary>
        /// An unsigned integer that specifies the number of strings in the OwningServers field that refer to lowest-cost servers.
        /// </summary>
        public BlockT<ushort> CheapServersCount;

        /// <summary>
        /// A list of null-terminated ASCII strings that specify which servers have replicas (1) of this folder.
        /// </summary>
        public BlockString[] OwningServers;

        /// <summary>
        /// Parse the RopGetOwningServersResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();
            if (ReturnValue == ErrorCodes.Success)
            {
                OwningServersCount = ParseT<ushort>();
                CheapServersCount = ParseT<ushort>();

                var tmpOwning = new List<BlockString>();
                for (int i = 0; i < OwningServersCount; i++)
                {
                    tmpOwning.Add(ParseStringA());
                }

                OwningServers = tmpOwning.ToArray();
            }
        }

        protected override void ParseBlocks()
        {
            Text = "RopGetOwningServersResponse";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
            AddChildBlockT(OwningServersCount, "OwningServersCount");
            AddChildBlockT(CheapServersCount, "CheapServersCount");
            AddLabeledChildren(OwningServers, "OwningServers");
        }
    }
}
