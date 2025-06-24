namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.2.6.7 RopReloadCachedInformation ROP
    /// A class indicates the RopReloadCachedInformation ROP request Buffer.
    /// </summary>
    public class RopReloadCachedInformationRequest : BaseStructure
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
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// Reserved. This field MUST be set to 0x0000.
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// Parse the RopReloadCachedInformationRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopReloadCachedInformationRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.Reserved = this.ReadUshort();
        }
    }
}
