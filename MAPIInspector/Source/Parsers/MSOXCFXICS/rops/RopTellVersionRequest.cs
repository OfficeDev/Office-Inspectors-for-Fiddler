namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  A class indicates the RopTellVersion ROP Request Buffer.
    ///  2.2.3.1.1.6.1 RopTellVersion ROP Request Buffer
    /// </summary>
    public class RopTellVersionRequest : BaseStructure
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
        /// An array of three unsigned 16-bit integers that contains the version information for the other server. 
        /// </summary>
        public byte[] Version;

        /// <summary>
        /// Parse the RopTellVersionRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopTellVersionRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.Version = this.ReadBytes(6);
        }
    }
}
