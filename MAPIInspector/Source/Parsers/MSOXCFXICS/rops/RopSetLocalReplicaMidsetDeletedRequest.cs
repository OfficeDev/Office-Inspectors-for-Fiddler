namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  A class indicates the RopSetLocalReplicaMidsetDeleted ROP Request Buffer.
    ///  2.2.13.12.1 RopSetLocalReplicaMidsetDeleted ROP Request Buffer
    /// </summary>
    public class RopSetLocalReplicaMidsetDeletedRequest : BaseStructure
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
        /// An unsigned integer that specifies the size of both the LongTermIdRangeCount and LongTermIdRanges fields.
        /// </summary>
        public ushort DataSize;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the LongTermIdRanges field.
        /// </summary>
        public uint LongTermIdRangeCount;

        /// <summary>
        /// An array of LongTermIdRange structures that specify the ranges of message identifiers that have been deleted.
        /// </summary>
        public LongTermIdRange[] LongTermIdRanges;

        /// <summary>
        /// Parse the RopSetLocalReplicaMidsetDeletedRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetLocalReplicaMidsetDeletedRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.DataSize = this.ReadUshort();
            this.LongTermIdRangeCount = this.ReadUint();
            LongTermIdRange[] interRangs = new LongTermIdRange[this.LongTermIdRangeCount];

            for (int i = 0; i < interRangs.Length; i++)
            {
                interRangs[i] = new LongTermIdRange();
                interRangs[i].Parse(s);
            }

            this.LongTermIdRanges = interRangs;
        }
    }
}
