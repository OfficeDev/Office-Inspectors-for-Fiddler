namespace MAPIInspector.Parsers
{
    using System.IO;
    using System.Text;

    /// <summary>
    ///  A class indicates the AUX_PERF_CLIENTINFO Auxiliary Block Structure
    ///  Section 2.2.2.2 AUX_HEADER Structure
    ///  Section 2.2.2.2.4   AUX_PERF_CLIENTINFO Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_CLIENTINFO : BaseStructure
    {
        /// <summary>
        /// The speed of client computer's network adapter, in kilobits per second.
        /// </summary>
        public uint AdapterSpeed;

        /// <summary>
        /// The client-assigned client identification number.
        /// </summary>
        public ushort ClientID;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the MachineName field. 
        /// </summary>
        public ushort MachineNameOffset;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the UserName field. 
        /// </summary>
        public ushort UserNameOffset;

        /// <summary>
        /// The size of the client IP address referenced by the ClientIPOffset field. 
        /// </summary>
        public ushort ClientIPSize;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the ClientIP field. 
        /// </summary>
        public ushort ClientIPOffset;

        /// <summary>
        /// The size of the client IP subnet mask referenced by the ClientIPMaskOffset field. 
        /// </summary>
        public ushort ClientIPMaskSize;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the ClientIPMask field. 
        /// </summary>
        public ushort ClientIPMaskOffset;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the AdapterName field. 
        /// </summary>
        public ushort AdapterNameOffset;

        /// <summary>
        /// The size of the network adapter Media Access Control (MAC) address referenced by the MacAddressOffset field. 
        /// </summary>
        public ushort MacAddressSize;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the MacAddress field. 
        /// </summary>
        public ushort MacAddressOffset;

        /// <summary>
        /// A flag that shows the mode in which the client is running. 
        /// </summary>
        public ClientModeFlag ClientMode;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field. 
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// A null-terminated Unicode string that contains the client computer name. 
        /// </summary>
        public MAPIString MachineName;

        /// <summary>
        /// A null-terminated Unicode string that contains the user's account name. 
        /// </summary>
        public MAPIString UserName;

        /// <summary>
        /// The client's IP address. 
        /// </summary>
        public byte?[] ClientIP;

        /// <summary>
        /// The client's IP subnet mask. 
        /// </summary>
        public byte?[] ClientIPMask;

        /// <summary>
        /// A null-terminated Unicode string that contains the client network adapter name.
        /// </summary>
        public MAPIString AdapterName;

        /// <summary>
        /// The client's network adapter MAC address. 
        /// </summary>
        public byte?[] MacAddress;

        /// <summary>
        /// Parse the AUX_PERF_CLIENTINFO structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_CLIENTINFO structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            AdapterSpeed = ReadUint();
            ClientID = ReadUshort();
            MachineNameOffset = ReadUshort();
            UserNameOffset = ReadUshort();
            ClientIPSize = ReadUshort();
            ClientIPOffset = ReadUshort();
            ClientIPMaskSize = ReadUshort();
            ClientIPMaskOffset = ReadUshort();
            AdapterNameOffset = ReadUshort();
            MacAddressSize = ReadUshort();
            MacAddressOffset = ReadUshort();
            ClientMode = (ClientModeFlag)ReadUshort();
            Reserved = ReadUshort();

            if (MachineNameOffset != 0)
            {
                MachineName = new MAPIString(Encoding.Unicode);
                MachineName.Parse(s);
            }

            if (UserNameOffset != 0)
            {
                UserName = new MAPIString(Encoding.Unicode);
                UserName.Parse(s);
            }

            if (ClientIPSize > 0 && ClientIPOffset != 0)
            {
                ClientIP = ConvertArray(ReadBytes(ClientIPSize));
            }

            if (ClientIPMaskSize > 0 && ClientIPMaskOffset != 0)
            {
                ClientIPMask = ConvertArray(ReadBytes(ClientIPMaskSize));
            }

            if (AdapterNameOffset != 0)
            {
                AdapterName = new MAPIString(Encoding.Unicode);
                AdapterName.Parse(s);
            }

            if (MacAddressSize > 0 && MacAddressOffset != 0)
            {
                MacAddress = ConvertArray(ReadBytes(MacAddressSize));
            }
        }
    }
}