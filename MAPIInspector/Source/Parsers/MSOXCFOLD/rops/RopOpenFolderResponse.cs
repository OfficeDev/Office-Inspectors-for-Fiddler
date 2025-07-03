using BlockParser;
using System.Collections.Generic;
using System.Text;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.1.1 RopOpenFolder ROP
    /// 2.2.1.1.2 RopOpenFolder ROP Response Buffer
    /// The RopOpenFolder ROP ([MS-OXCROPS] section 2.2.4.1) opens an existing folder.
    /// 2.2.4.1.2 RopOpenFolder ROP Success Response Buffer
    /// 2.2.4.1.3 RopOpenFolder ROP Failure Response Buffer
    /// </summary>
    public class RopOpenFolderResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        /// </summary>
        public BlockT<byte> OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<ErrorCodes> ReturnValue;

        /// <summary>
        /// A Boolean that indicates whether the folder has rules associated with it.
        /// </summary>
        public BlockT<bool> HasRules;

        /// <summary>
        /// A Boolean that specifies whether the folder is a ghosted folder.
        /// </summary>
        public BlockT<bool> IsGhosted;

        /// <summary>
        /// This value specifies the number of strings in the Servers field.
        /// </summary>
        public BlockT<ushort> ServerCount;

        /// <summary>
        /// This value specifies the number of values in the Servers field that refer to lowest-cost servers.
        /// </summary>
        public BlockT<ushort> CheapServerCount;

        /// <summary>
        /// A list of null-terminated ASCII strings that specify which servers have replicas (2) of this folder.
        /// </summary>
        public BlockString[] Servers;

        /// <summary>
        /// Parse the RopOpenFolderResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            OutputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                HasRules = ParseAs<byte, bool>();
                IsGhosted = ParseAs<byte, bool>();
                if (IsGhosted)
                {
                    ServerCount = ParseT<ushort>();
                    CheapServerCount = ParseT<ushort>();
                    var tempServers = new List<BlockString>();
                    for (int i = 0; i < ServerCount; i++)
                    {
                        tempServers.Add(ParseStringA());
                    }

                    Servers = tempServers.ToArray();
                }
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopOpenFolderResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue:{ReturnValue.Data.FormatErrorCode()}");
            AddChildBlockT(HasRules, "HasRules");
            AddChildBlockT(IsGhosted, "IsGhosted");
            AddChildBlockT(ServerCount, "ServerCount");
            AddChildBlockT(CheapServerCount, "CheapServerCount");
            AddLabeledChildren(Servers, "Servers");
        }
    }
}