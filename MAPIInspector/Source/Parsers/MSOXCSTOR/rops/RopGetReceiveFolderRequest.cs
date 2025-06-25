namespace MAPIInspector.Parsers
{
    using System.IO;
    using System.Text;

    /// <summary>
    ///  2.2.1.2 RopGetReceiveFolder
    ///  A class indicates the RopGetReceiveFolder ROP Request Buffer.
    /// </summary>
    public class RopGetReceiveFolderRequest : BaseStructure
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
        /// A null-terminated ASCII string that specifies the message class to find the Receive folder for.
        /// </summary>
        public MAPIString MessageClass;

        /// <summary>
        /// Parse the RopGetReceiveFolderRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetReceiveFolderRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            MessageClass = new MAPIString(Encoding.ASCII);
            MessageClass.Parse(s);
        }
    }
}
