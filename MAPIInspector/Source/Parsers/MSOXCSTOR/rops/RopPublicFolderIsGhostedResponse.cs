namespace MAPIInspector.Parsers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    ///  2.2.1.7 RopPublicFolderIsGhosted
    ///  A class indicates the RopPublicFolderIsGhosted ROP Response Buffer.
    /// </summary>
    public class RopPublicFolderIsGhostedResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// A Boolean that specifies whether the folder is a ghosted folder.
        /// </summary>
        public bool? IsGhosted;

        /// <summary>
        /// An unsigned integer that is present if IsGhosted is nonzero and is not present if IsGhosted is zero.
        /// </summary>
        public ushort? ServersCount;

        /// <summary>
        /// An unsigned integer that is present if the value of the IsGhosted field is nonzero and is not present if the value of the IsGhosted field is zero.
        /// </summary>
        public ushort? CheapServersCount;

        /// <summary>
        /// A list of null-terminated ASCII strings that specify which servers have replicas (1) of this folder.
        /// </summary>
        public MAPIString[] Servers;

        /// <summary>
        /// Parse the RopPublicFolderIsGhostedResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopPublicFolderIsGhostedResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                IsGhosted = ReadBoolean();
                if (IsGhosted == true)
                {
                    ServersCount = ReadUshort();
                    CheapServersCount = ReadUshort();
                    List<MAPIString> tmpServers = new List<MAPIString>();
                    for (int i = 0; i < ServersCount; i++)
                    {
                        MAPIString subServer = new MAPIString(Encoding.ASCII);
                        subServer.Parse(s);
                        tmpServers.Add(subServer);
                    }

                    Servers = tmpServers.ToArray();
                }
            }
        }
    }
}
