using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AUX_PERF_CLIENTINFO Auxiliary Block Structure
    ///  Section 2.2.2.2 AUX_HEADER Structure
    ///  Section 2.2.2.2.4 AUX_PERF_CLIENTINFO Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_CLIENTINFO : Block
    {
        /// <summary>
        /// The speed of client computer's network adapter, in kilobits per second.
        /// </summary>
        public BlockT<uint> AdapterSpeed;

        /// <summary>
        /// The client-assigned client identification number.
        /// </summary>
        public BlockT<ushort> ClientID;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the MachineName field.
        /// </summary>
        public BlockT<ushort> MachineNameOffset;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the UserName field.
        /// </summary>
        public BlockT<ushort> UserNameOffset;

        /// <summary>
        /// The size of the client IP address referenced by the ClientIPOffset field.
        /// </summary>
        public BlockT<ushort> ClientIPSize;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the ClientIP field.
        /// </summary>
        public BlockT<ushort> ClientIPOffset;

        /// <summary>
        /// The size of the client IP subnet mask referenced by the ClientIPMaskOffset field.
        /// </summary>
        public BlockT<ushort> ClientIPMaskSize;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the ClientIPMask field.
        /// </summary>
        public BlockT<ushort> ClientIPMaskOffset;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the AdapterName field.
        /// </summary>
        public BlockT<ushort> AdapterNameOffset;

        /// <summary>
        /// The size of the network adapter Media Access Control (MAC) address referenced by the MacAddressOffset field.
        /// </summary>
        public BlockT<ushort> MacAddressSize;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the MacAddress field.
        /// </summary>
        public BlockT<ushort> MacAddressOffset;

        /// <summary>
        /// A flag that shows the mode in which the client is running.
        /// </summary>
        public BlockT<ClientModeFlag> ClientMode;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field.
        /// </summary>
        public BlockT<ushort> Reserved;

        /// <summary>
        /// A null-terminated Unicode string that contains the client computer name.
        /// </summary>
        public BlockString MachineName;

        /// <summary>
        /// A null-terminated Unicode string that contains the user's account name.
        /// </summary>
        public BlockString UserName;

        /// <summary>
        /// The client's IP address.
        /// </summary>
        public BlockBytes ClientIP;

        /// <summary>
        /// The client's IP subnet mask.
        /// </summary>
        public BlockBytes ClientIPMask;

        /// <summary>
        /// A null-terminated Unicode string that contains the client network adapter name.
        /// </summary>
        public BlockString AdapterName;

        /// <summary>
        /// The client's network adapter MAC address.
        /// </summary>
        public BlockBytes MacAddress;

        /// <summary>
        /// Parse the AUX_PERF_CLIENTINFO structure.
        /// </summary>
        protected override void Parse()
        {
            AdapterSpeed = ParseT<uint>();
            ClientID = ParseT<ushort>();
            MachineNameOffset = ParseT<ushort>();
            UserNameOffset = ParseT<ushort>();
            ClientIPSize = ParseT<ushort>();
            ClientIPOffset = ParseT<ushort>();
            ClientIPMaskSize = ParseT<ushort>();
            ClientIPMaskOffset = ParseT<ushort>();
            AdapterNameOffset = ParseT<ushort>();
            MacAddressSize = ParseT<ushort>();
            MacAddressOffset = ParseT<ushort>();
            ClientMode = ParseT<ClientModeFlag>();
            Reserved = ParseT<ushort>();

            if (MachineNameOffset != 0)
            {
                // TODO: actually read from the offset, not from the current position
                MachineName = ParseStringW();
            }

            if (UserNameOffset != 0)
            {
                // TODO: actually read from the offset, not from the current position
                UserName = ParseStringW();
            }

            if (ClientIPSize > 0 && ClientIPOffset != 0)
            {
                // TODO: actually read from the offset, not from the current position
                ClientIP = ParseBytes(ClientIPSize);
            }

            if (ClientIPMaskSize > 0 && ClientIPMaskOffset != 0)
            {
                // TODO: actually read from the offset, not from the current position
                ClientIPMask = ParseBytes(ClientIPMaskSize);
            }

            if (AdapterNameOffset != 0)
            {
                // TODO: actually read from the offset, not from the current position
                AdapterName = ParseStringW();
            }

            if (MacAddressSize > 0 && MacAddressOffset != 0)
            {
                // TODO: actually read from the offset, not from the current position
                MacAddress = ParseBytes(MacAddressSize);
            }
        }

        protected override void ParseBlocks()
        {
            Text = "AUX_PERF_CLIENTINFO";
            AddChildBlockT(AdapterSpeed, "AdapterSpeed");
            AddChildBlockT(ClientID, "ClientID");
            AddChildBlockT(MachineNameOffset, "MachineNameOffset");
            AddChildBlockT(UserNameOffset, "UserNameOffset");
            AddChildBlockT(ClientIPSize, "ClientIPSize");
            AddChildBlockT(ClientIPOffset, "ClientIPOffset");
            AddChildBlockT(ClientIPMaskSize, "ClientIPMaskSize");
            AddChildBlockT(ClientIPMaskOffset, "ClientIPMaskOffset");
            AddChildBlockT(AdapterNameOffset, "AdapterNameOffset");
            AddChildBlockT(MacAddressSize, "MacAddressSize");
            AddChildBlockT(MacAddressOffset, "MacAddressOffset");
            AddChildBlockT(ClientMode, "ClientMode");
            AddChildBlockT(Reserved, "Reserved");
            AddChildString(MachineName, "MachineName");
            AddChildString(UserName, "UserName");
            AddChildBytes(ClientIP, "ClientIP");
            AddChildBytes(ClientIPMask, "ClientIPMask");
            AddChildString(AdapterName, "AdapterName");
            AddChildBytes(MacAddress, "MacAddress");
        }
    }
}