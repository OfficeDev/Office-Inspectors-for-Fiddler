using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AUX_OSVERSIONINFO Auxiliary Block Structure
    /// Section 2.2.2.2 AUX_HEADER Structure
    /// Section 2.2.2.2.16   AUX_OSVERSIONINFO Auxiliary Block Structure
    /// </summary>
    public class AUX_OSVERSIONINFO : BaseStructure
    {
        /// <summary>
        /// The size of this AUX_OSVERSIONINFO structure.
        /// </summary>
        public uint OSVersionInfoSize;

        /// <summary>
        /// The major version number of the operating system of the server.
        /// </summary>
        public uint MajorVersion;

        /// <summary>
        /// The minor version number of the operating system of the server.
        /// </summary>
        public uint MinorVersion;

        /// <summary>
        /// The build number of the operating system of the server.
        /// </summary>
        public uint BuildNumber;

        /// <summary>
        /// Reserved and MUST be ignored when received. 
        /// </summary>
        public byte[] Reserved1;

        /// <summary>
        /// The major version number of the latest operating system service pack that is installed on the server.
        /// </summary>
        public ushort ServicePackMajor;

        /// <summary>
        /// The minor version number of the latest operating system service pack that is installed on the server.
        /// </summary>
        public ushort ServicePackMinor;

        /// <summary>
        /// Reserved and MUST be ignored when received. 
        /// </summary>
        public uint Reserved2;

        /// <summary>
        /// Parse the AUX_OSVERSIONINFO structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_OSVERSIONINFO structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            OSVersionInfoSize = ReadUint();
            MajorVersion = ReadUint();
            MinorVersion = ReadUint();
            BuildNumber = ReadUint();
            Reserved1 = ReadBytes(132);
            ServicePackMajor = ReadUshort();
            ServicePackMinor = ReadUshort();
            Reserved2 = ReadUint();
        }
    }
}