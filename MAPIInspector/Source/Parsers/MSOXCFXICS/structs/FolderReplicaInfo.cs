using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The FolderReplicaInfo structure contains information about server replicas of a public folder.
    /// 2.2.2.9 FolderReplicaInfo
    /// </summary>
    public class FolderReplicaInfo : Block
    {
        /// <summary>
        /// A UInt value.
        /// </summary>
        public BlockT<uint> Flags;

        /// <summary>
        /// A UInt value.
        /// </summary>
        public BlockT<uint> Depth;

        /// <summary>
        /// A LongTermID structure. Contains the LongTermID of a folder, for which server replica information is being described.
        /// </summary>
        public LongTermId FolderLongTermId;

        /// <summary>
        /// An unsigned 32-bit integer value that determines how many elements exist in ServerDNArray. 
        /// </summary>
        public BlockT<uint> ServerDNCount;

        /// <summary>
        /// An unsigned 32-bit integer value that determines how many of the leading elements in ServerDNArray have the same,lowest, network access cost.
        /// </summary>
        public BlockT<uint> CheapServerDNCount;

        /// <summary>
        /// An array of ASCII-encoded NULL-terminated strings.
        /// </summary>
        public PtypString8[] ServerDNArray;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        protected override void Parse()
        {
            Flags = ParseT<uint>();
            Depth = ParseT<uint>();
            FolderLongTermId = Parse<LongTermId>();
            ServerDNCount = ParseT<uint>();
            CheapServerDNCount = ParseT<uint>();

            var tmpDNArray = new List<PtypString8>();
            for (int i = 0; i < ServerDNCount; i++)
            {

                tmpDNArray.Add(Parse<PtypString8>());
            }

            ServerDNArray = tmpDNArray.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("FolderReplicaInfo");
            AddChildBlockT(Flags, "Flags");
            AddChildBlockT(Depth, "Depth");
            if (FolderLongTermId != null) AddChild(FolderLongTermId, "FolderLongTermId");
            AddChildBlockT(ServerDNCount, "ServerDNCount");
            AddChildBlockT(CheapServerDNCount, "CheapServerDNCount");
            foreach (var serverDN in ServerDNArray)
            {
                if (serverDN != null) AddChild(serverDN, $"ServerDN");
            }
        }
    }
}
