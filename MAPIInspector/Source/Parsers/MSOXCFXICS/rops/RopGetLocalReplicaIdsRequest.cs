namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  A class indicates the RopGetLocalReplicaIds ROP Request Buffer.
    ///  2.2.13.13.1 RopGetLocalReplicaIds ROP Request Buffer
    /// </summary>
    public class RopGetLocalReplicaIdsRequest : BaseStructure
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
        /// An unsigned integer that specifies the number of IDs to reserve.
        /// </summary>
        public uint IdCount;

        /// <summary>
        /// Parse the RopGetLocalReplicaIdsRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetLocalReplicaIdsRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.IdCount = this.ReadUint();
        }
    }
}
