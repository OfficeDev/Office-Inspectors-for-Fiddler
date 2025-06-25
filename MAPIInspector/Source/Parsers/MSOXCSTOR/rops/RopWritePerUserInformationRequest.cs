namespace MAPIInspector.Parsers
{
    using System;
    using System.IO;

    /// <summary>
    ///  2.2.1.13 RopWritePerUserInformation
    ///  A class indicates the RopWritePerUserInformation ROP Request Buffer.
    /// </summary>
    public class RopWritePerUserInformationRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// A LongTermID structure that specifies the folder to set per-user information for.
        /// </summary>
        public LongTermID FolderId;

        /// <summary>
        /// A Boolean that specifies whether this operation specifies the end of the per-user information stream.
        /// </summary>
        public bool HasFinished;

        /// <summary>
        /// An unsigned integer that specifies the location in the per-user information stream to start writing
        /// </summary>
        public uint DataOffset;

        /// <summary>
        /// An unsigned integer that specifies the size of the Data field.
        /// </summary>
        public ushort DataSize;

        /// <summary>
        /// An array of bytes that is the per-user data to write.
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// An GUID that is present when the DataOffset is 0 and the RopLogon associated with the LogonId field was created with the Private flag set in the RopLogon ROP request buffer
        /// </summary>
        public Guid? ReplGuid;

        /// <summary>
        /// Parse the RopWritePerUserInformationRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopWritePerUserInformationRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            FolderId = new LongTermID();
            FolderId.Parse(s);
            HasFinished = ReadBoolean();
            DataOffset = ReadUint();
            DataSize = ReadUshort();
            Data = ReadBytes((int)DataSize);
            if (!MapiInspector.MAPIParser.IsFromFiddlerCore(MapiInspector.MAPIParser.ParsingSession))
            {
                if (DataOffset == 0 && (((byte)DecodingContext.SessionLogonFlagMapLogId[MapiInspector.MAPIParser.ParsingSession.id][LogonId] & (byte)LogonFlags.Private) == (byte)LogonFlags.Private))
                {
                    ReplGuid = ReadGuid();
                }
            }
            else
            {
                if (DataOffset == 0 && (((byte)DecodingContext.SessionLogonFlagMapLogId[int.Parse(MapiInspector.MAPIParser.ParsingSession["VirtualID"])][LogonId] & (byte)LogonFlags.Private) == (byte)LogonFlags.Private))
                {
                    ReplGuid = ReadGuid();
                }
            }
        }
    }
}
