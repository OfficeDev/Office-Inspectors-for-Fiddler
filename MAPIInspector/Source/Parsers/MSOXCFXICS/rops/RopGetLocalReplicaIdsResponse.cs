namespace MAPIInspector.Parsers
{
    using System;
    using System.IO;

    /// <summary>
    ///  A class indicates the RopGetLocalReplicaIds ROP Response Buffer.
    ///  2.2.13.13.2 RopGetLocalReplicaIds ROP Success Response Buffer
    /// </summary>
    public class RopGetLocalReplicaIdsResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// This field contains the replica GUID that is shared by the IDs.
        /// </summary>
        public Guid? ReplGuid;

        /// <summary>
        /// An array of bytes that specifies the first value in the reserved range.
        /// </summary>
        public byte?[] GlobalCount;

        /// <summary>
        /// Parse the RopGetLocalReplicaIdsResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetLocalReplicaIdsResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.ReplGuid = this.ReadGuid();
                this.GlobalCount = this.ConvertArray(this.ReadBytes(6));
            }
        }
    }
}
