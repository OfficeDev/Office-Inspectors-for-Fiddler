namespace MAPIInspector.Parsers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    /// 2.2.1.1 RopOpenFolder ROP
    /// 2.2.1.1.2 RopOpenFolder ROP Response Buffer
    /// The RopOpenFolder ROP ([MS-OXCROPS] section 2.2.4.1) opens an existing folder. 
    /// 2.2.4.1.2 RopOpenFolder ROP Success Response Buffer
    /// 2.2.4.1.3 RopOpenFolder ROP Failure Response Buffer
    /// </summary>
    public class RopOpenFolderResponse : BaseStructure
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
        /// A Boolean that indicates whether the folder has rules associated with it.
        /// </summary>
        public bool? HasRules;

        /// <summary>
        /// A Boolean that specifies whether the folder is a ghosted folder.
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
        /// A list of null-terminated ASCII strings that specify which servers have replicas (2) of this folder. 
        /// </summary>
        public MAPIString[] Servers;

        /// <summary>
        /// Parse the RopOpenFolderResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopOpenFolderResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            OutputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
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