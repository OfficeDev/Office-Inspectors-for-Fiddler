namespace MAPIInspector.Parsers
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    ///  2.2.1.1 RopLogon
    ///  A class indicates the RopLogon ROP Response Buffer for public folders.
    /// </summary>
    public class RopLogonResponse_PublicFolders : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// A flags structure that contains flags that specify the type of RopLogon.
        /// </summary>
        public LogonFlags? LogonFlags;

        /// <summary>
        /// 13 64-bit identifiers that specify a set of special folders for a mailbox.
        /// </summary>
        public FolderID[] FolderIds;

        /// <summary>
        /// An identifier that specifies a replica ID for the RopLogon.
        /// </summary>
        public ushort? ReplId;

        /// <summary>
        /// A GUID that specifies the replica GUID associated with the replica ID that is specified in the ReplId field.
        /// </summary>
        public Guid? ReplGuid;

        /// <summary>
        /// This field is not used and is ignored by the client.
        /// </summary>
        public Guid? PerUserGuid;

        /// <summary>
        /// The below two fields is defined for RopLogon redirect response in section 2.2.3.1.4 in MS-OXCROPS.
        /// An unsigned integer that specifies the length of the ServerName field.
        /// </summary>
        public byte? ServerNameSize;

        /// <summary>
        /// A null-terminated ASCII string that specifies a different server for the client to connect to.
        /// </summary>
        public MAPIString ServerName;

        /// <summary>
        /// Parse the RopLogonResponse_PublicFolders structure.
        /// </summary>
        /// <param name="s">A stream containing RopLogonResponse_PublicFolders structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            OutputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                LogonFlags = (LogonFlags)ReadByte();
                FolderIds = new FolderID[13];
                for (int i = 0; i < 13; i++)
                {
                    FolderIds[i] = new FolderID();
                    FolderIds[i].Parse(s);
                }

                ReplId = ReadUshort();
                ReplGuid = ReadGuid();
                PerUserGuid = ReadGuid();
            }
            else if ((AdditionalErrorCodes)ReturnValue == AdditionalErrorCodes.WrongServer)
            {
                LogonFlags = (LogonFlags)ReadByte();
                ServerNameSize = ReadByte();
                ServerName = new MAPIString(Encoding.ASCII);
                ServerName.Parse(s);
            }
        }
    }
}
