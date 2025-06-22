namespace MAPIInspector.Parsers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    /// 2.2.1.2 RopCreateFolder ROP
    /// A class indicates the RopCreateFolder ROP Response Buffer.
    /// </summary>
    public class RopCreateFolderResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request. 
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An identifier that specifies the folder created or opened.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// A Boolean that indicates whether an existing folder was opened or a new folder was created.
        /// </summary>
        public bool? IsExistingFolder;

        /// <summary>
        /// A Boolean that indicates whether the folder has rules associated with it.
        /// </summary>
        public bool? HasRules;

        /// <summary>
        /// A Boolean that indicates whether the server is an active replica of this folder. 
        /// </summary>
        public bool? IsGhosted;

        /// <summary>
        /// This value specifies the number of strings in the Servers field.
        /// </summary>
        public ushort? ServerCount;

        /// <summary>
        /// This value specifies the number of values in the Servers field that refer to lowest-cost servers.
        /// </summary>
        public ushort? CheapServerCount;

        /// <summary>
        /// These strings specify which servers have replicas (2) of this folder.
        /// </summary>
        public MAPIString[] Servers;

        /// <summary>
        /// Parse the RopCreateFolderResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopCreateFolderResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            OutputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                FolderId = new FolderID();
                FolderId.Parse(s);
                IsExistingFolder = ReadBoolean();
                if ((bool)IsExistingFolder)
                {
                    HasRules = ReadBoolean();
                    IsGhosted = ReadBoolean();
                    if ((bool)IsGhosted)
                    {
                        ServerCount = ReadUshort();
                        CheapServerCount = ReadUshort();
                        List<MAPIString> tempServers = new List<MAPIString>();
                        for (int i = 0; i < ServerCount; i++)
                        {
                            MAPIString tempString = new MAPIString(Encoding.ASCII);
                            tempString.Parse(s);
                            tempServers.Add(tempString);
                        }

                        Servers = tempServers.ToArray();
                    }
                }
            }
        }
    }
}