namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.2.25 RopUnlockRegionStream
    ///  A class indicates the RopUnlockRegionStream ROP Request Buffer.
    /// </summary>
    public class RopUnlockRegionStreamRequest : BaseStructure
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
        /// An unsigned integer that specifies the byte location in the stream where the region begins.
        /// </summary>
        public ulong RegionOffset;

        /// <summary>
        /// An unsigned integer that specifies the size of the region, in bytes.
        /// </summary>
        public ulong RegionSize;

        /// <summary>
        /// A flags structure that contains flags specifying the behavior of the lock operation. 
        /// </summary>
        public uint LockFlags;

        /// <summary>
        /// Parse the RopUnlockRegionStreamRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopUnlockRegionStreamRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            RegionOffset = ReadUlong();
            RegionSize = ReadUlong();
            LockFlags = ReadUint();
        }
    }
}
