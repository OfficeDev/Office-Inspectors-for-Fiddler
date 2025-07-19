using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.1.2 RopCreateFolder ROP
    /// A class indicates the RopCreateFolder ROP Response Buffer.
    /// </summary>
    public class RopCreateFolderResponse : Block
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
        /// An identifier that specifies the folder created or opened.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// A Boolean that indicates whether an existing folder was opened or a new folder was created.
        /// </summary>
        public BlockT<bool> IsExistingFolder;

        /// <summary>
        /// A Boolean that indicates whether the folder has rules associated with it.
        /// </summary>
        public BlockT<bool> HasRules;

        /// <summary>
        /// A Boolean that indicates whether the server is an active replica of this folder.
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
        /// These strings specify which servers have replicas (2) of this folder.
        /// </summary>
        public BlockString[] Servers;

        /// <summary>
        /// Parse the RopCreateFolderResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            OutputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                FolderId = Parse<FolderID>();
                IsExistingFolder = ParseAs<byte, bool>();
                if (IsExistingFolder)
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
        }

        protected override void ParseBlocks()
        {
            SetText("RopCreateFolderResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue: {ReturnValue.Data.FormatErrorCode()}");
            AddChild(FolderId, "FolderId");
            AddChildBlockT(IsExistingFolder, "IsExistingFolder");
            AddChildBlockT(HasRules, "HasRules");
            AddChildBlockT(IsGhosted, "IsGhosted");
            AddChildBlockT(ServerCount, "ServerCount");
            AddChildBlockT(CheapServerCount, "CheapServerCount");
            AddLabeledChildren(Servers, "Servers");
        }
    }
}
