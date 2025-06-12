namespace MAPIInspector.Parsers
{
    using System.Text;

    /// <summary>
    /// The FolderReplicaInfo structure contains information about server replicas of a public folder.
    /// 2.2.2.9 FolderReplicaInfo
    /// </summary>
    public class FolderReplicaInfo : BaseStructure
    {
        /// <summary>
        /// A UInt value.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// A UInt value.
        /// </summary>
        public uint Depth;

        /// <summary>
        /// A LongTermID structure. Contains the LongTermID of a folder, for which server replica information is being described.
        /// </summary>
        public LongTermId FolderLongTermId;

        /// <summary>
        /// An unsigned 32-bit integer value that determines how many elements exist in ServerDNArray. 
        /// </summary>
        public uint ServerDNCount;

        /// <summary>
        /// An unsigned 32-bit integer value that determines how many of the leading elements in ServerDNArray have the same,lowest, network access cost.
        /// </summary>
        public uint CheapServerDNCount;

        /// <summary>
        /// An array of ASCII-encoded NULL-terminated strings. 
        /// </summary>
        public MAPIString[] ServerDNArray;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains FolderReplicaInfo.</param>
        public void Parse(FastTransferStream stream)
        {
            this.Flags = stream.ReadUInt32();
            this.Depth = stream.ReadUInt32();
            this.FolderLongTermId = new LongTermId(stream);
            this.ServerDNCount = stream.ReadUInt32();
            this.CheapServerDNCount = stream.ReadUInt32();
            this.ServerDNArray = new MAPIString[this.ServerDNCount];

            for (int i = 0; i < this.ServerDNCount; i++)
            {
                this.ServerDNArray[i] = new MAPIString(Encoding.ASCII);
                this.ServerDNArray[i].Parse(stream);
            }
        }
    }
}
