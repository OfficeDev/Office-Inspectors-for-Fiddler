using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AUX_OSVERSIONINFO Auxiliary Block Structure
    /// Section 2.2.2.2 AUX_HEADER Structure
    /// Section 2.2.2.2.16 AUX_OSVERSIONINFO Auxiliary Block Structure
    /// </summary>
    public class AUX_OSVERSIONINFO : Block
    {
        /// <summary>
        /// The size of this AUX_OSVERSIONINFO structure.
        /// </summary>
        public BlockT<uint> OSVersionInfoSize;

        /// <summary>
        /// The major version number of the operating system of the server.
        /// </summary>
        public BlockT<uint> MajorVersion;

        /// <summary>
        /// The minor version number of the operating system of the server.
        /// </summary>
        public BlockT<uint> MinorVersion;

        /// <summary>
        /// The build number of the operating system of the server.
        /// </summary>
        public BlockT<uint> BuildNumber;

        /// <summary>
        /// Reserved and MUST be ignored when received.
        /// </summary>
        public BlockBytes Reserved1; // 132 bytes reserved for future use, should be ignored when received.

        /// <summary>
        /// The major version number of the latest operating system service pack that is installed on the server.
        /// </summary>
        public BlockT<ushort> ServicePackMajor;

        /// <summary>
        /// The minor version number of the latest operating system service pack that is installed on the server.
        /// </summary>
        public BlockT<ushort> ServicePackMinor;

        /// <summary>
        /// Reserved and MUST be ignored when received.
        /// </summary>
        public BlockT<uint> Reserved2;

        /// <summary>
        /// Parse the AUX_OSVERSIONINFO structure.
        /// </summary>
        protected override void Parse()
        {
            OSVersionInfoSize = ParseT<uint>();
            MajorVersion = ParseT<uint>();
            MinorVersion = ParseT<uint>();
            BuildNumber = ParseT<uint>();
            Reserved1 = ParseBytes(132);
            ServicePackMajor = ParseT<ushort>();
            ServicePackMinor = ParseT<ushort>();
            Reserved2 = ParseT<uint>();
        }

        protected override void ParseBlocks()
        {
            Text = "AUX_OSVERSIONINFO";
            AddChildBlockT(OSVersionInfoSize, "OSVersionInfoSize");
            AddChildBlockT(MajorVersion, "MajorVersion");
            AddChildBlockT(MinorVersion, "MinorVersion");
            AddChildBlockT(BuildNumber, "BuildNumber");
            AddChildBytes(Reserved1, "Reserved1");
            AddChildBlockT(ServicePackMajor, "ServicePackMajor");
            AddChildBlockT(ServicePackMinor, "ServicePackMinor");
            AddChildBlockT(Reserved2, "Reserved2");
        }
    }
}