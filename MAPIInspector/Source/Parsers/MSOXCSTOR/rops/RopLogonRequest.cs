namespace MAPIInspector.Parsers
{
    using System.IO;
    using System.Text;

    /// <summary>
    ///  2.2.1.1 RopLogon
    ///  A class indicates the RopLogon ROP Request Buffer.
    /// </summary>
    public class RopLogonRequest : BaseStructure
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
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// A flags structure that contains flags that control the behavior of the RopLogon.
        /// </summary>
        public LogonFlags LogonFlags;

        /// <summary>
        /// A flags structure that contains more flags that control the behavior of the RopLogon.
        /// </summary>
        public OpenFlags OpenFlags;

        /// <summary>
        /// A flags structure. This field is not used and is ignored by the server.
        /// </summary>
        public uint StoreState;

        /// <summary>
        /// An unsigned integer that specifies the size of the ESSDN field.
        /// </summary>
        public ushort EssdnSize;

        /// <summary>
        /// A null-terminated ASCII string that specifies which mailbox to log on to. 
        /// </summary>
        public MAPIString Essdn;

        /// <summary>
        /// Parse the RopLogonRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopLogonRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            OutputHandleIndex = ReadByte();
            LogonFlags = (LogonFlags)ReadByte();
            OpenFlags = (OpenFlags)ReadUint();
            StoreState = ReadUint();
            EssdnSize = ReadUshort();
            if (EssdnSize > 0)
            {
                Essdn = new MAPIString(Encoding.ASCII);
                Essdn.Parse(s);
            }
        }
    }
}
