namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.Collections.Generic;

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
            Flags = BlockT<uint>.Parse(parser);
            Depth = BlockT<uint>.Parse(parser);
            FolderLongTermId = Parse<LongTermId>(parser);
            ServerDNCount = BlockT<uint>.Parse(parser);
            CheapServerDNCount = BlockT<uint>.Parse(parser);

            var tmpDNArray = new List<PtypString8>();
            for (int i = 0; i < ServerDNCount.Data; i++)
            {

                tmpDNArray.Add(Parse<PtypString8>(parser));
            }

            ServerDNArray = tmpDNArray.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("FolderReplicaInfo");
            if (Flags != null) AddChild(Flags, $"Flags:{Flags.Data}");
            if (Depth != null) AddChild(Depth, $"Depth:{Depth.Data}");
            if (FolderLongTermId != null) AddChild(FolderLongTermId, "FolderLongTermId");
            if (ServerDNCount != null) AddChild(ServerDNCount, $"ServerDNCount:{ServerDNCount.Data}");
            if (CheapServerDNCount != null) AddChild(CheapServerDNCount, $"CheapServerDNCount:{CheapServerDNCount.Data}");
            foreach (var serverDN in ServerDNArray)
            {
                if (serverDN != null) AddChild(serverDN, $"ServerDN");
            }
        }
    }
}
