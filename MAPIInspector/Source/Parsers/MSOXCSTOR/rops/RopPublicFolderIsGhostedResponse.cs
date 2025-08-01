using System.Collections.Generic;
using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.3.7.2 RopPublicFolderIsGhosted ROP Success Response Buffer
    /// [MS-OXCROPS] 2.2.3.7.3 RopPublicFolderIsGhosted ROP Failure Response Buffer
    /// A class indicates the RopPublicFolderIsGhosted ROP Response Buffer.
    /// </summary>
    public class RopPublicFolderIsGhostedResponse : Block
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
        /// A Boolean that specifies whether the folder is a ghosted folder.
        /// </summary>
        public BlockT<bool> IsGhosted;

        /// <summary>
        /// An unsigned integer that is present if IsGhosted is nonzero and is not present if IsGhosted is zero.
        /// </summary>
        public BlockT<ushort> ServersCount;

        /// <summary>
        /// An unsigned integer that is present if the value of the IsGhosted field is nonzero and is not present if the value of the IsGhosted field is zero.
        /// </summary>
        public BlockT<ushort> CheapServersCount;

        /// <summary>
        /// A list of null-terminated ASCII strings that specify which servers have replicas (1) of this folder.
        /// </summary>
        public BlockString[] Servers;

        /// <summary>
        /// Parse the RopPublicFolderIsGhostedResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();
            if (ReturnValue == ErrorCodes.Success)
            {
                IsGhosted = ParseAs<byte, bool>();
                if (IsGhosted == true)
                {
                    ServersCount = ParseT<ushort>();
                    CheapServersCount = ParseT<ushort>();
                    var tmpServers = new List<BlockString>();
                    for (int i = 0; i < ServersCount; i++)
                    {
                        tmpServers.Add(ParseStringA());
                    }

                    Servers = tmpServers.ToArray();
                }
            }
        }

        protected override void ParseBlocks()
        {
            Text = "RopPublicFolderIsGhostedResponse";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
            AddChildBlockT(IsGhosted, "IsGhosted");
            AddChildBlockT(ServersCount, "ServersCount");
            AddChildBlockT(CheapServersCount, "CheapServersCount");
            AddLabeledChildren(Servers, "Servers");
        }
    }
}
