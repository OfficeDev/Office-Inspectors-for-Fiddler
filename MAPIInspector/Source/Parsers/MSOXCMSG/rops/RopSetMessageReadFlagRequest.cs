namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.2.6.11 RopSetMessageReadFlag ROP
    /// A class indicates the RopSetMessageReadFlag ROP request Buffer.
    /// </summary>
    public class RopSetMessageReadFlagRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table that is referenced in the response. 
        /// </summary>
        public byte ResponseHandleIndex;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// A flags structure. The possible values for these flags are specified in [MS-OXCMSG] section 2.2.3.11.1.
        /// </summary>
        public ReadFlags ReadFlags;

        /// <summary>
        /// An array of bytes that is present when the RopLogon associated with LogonId was created with the Private flag
        /// </summary>
        public byte?[] ClientData;

        /// <summary>
        /// Parse the RopSetMessageReadFlagRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetMessageReadFlagRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.ResponseHandleIndex = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReadFlags = (ReadFlags)this.ReadByte();
            if(!MapiInspector.MAPIParser.IsFromFiddlerCore(MapiInspector.MAPIParser.ParsingSession))
            {
                if (((byte)DecodingContext.SessionLogonFlagMapLogId[MapiInspector.MAPIParser.ParsingSession.id][this.LogonId] & (byte)LogonFlags.Private) != (byte)LogonFlags.Private)
                {
                    this.ClientData = this.ConvertArray(this.ReadBytes(24));
                }
            }
            else
            {
                if (((byte)DecodingContext.SessionLogonFlagMapLogId[int.Parse(MapiInspector.MAPIParser.ParsingSession["VirtualID"])][this.LogonId] & (byte)LogonFlags.Private) != (byte)LogonFlags.Private)
                {
                    this.ClientData = this.ConvertArray(this.ReadBytes(24));
                }
            }
        }
    }
}
