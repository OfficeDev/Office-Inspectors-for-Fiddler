using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AUX_PROTOCOL_DEVICE_IDENTIFICATION Auxiliary Block Structure
    ///  Section 2.2.2.2 AUX_HEADER Structure
    ///  Section 2.2.2.2.22 AUX_PROTOCOL_DEVICE_IDENTIFICATION Auxiliary Block Structure
    /// </summary>
    public class AUX_PROTOCOL_DEVICE_IDENTIFICATION : Block
    {
        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure, as specified in section 2.2.2.2, to the DeviceManufacturer field.
        /// </summary>
        public BlockT<ushort> DeviceManufacturerOffset;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the DeviceModel field.
        /// </summary>
        public BlockT<ushort> DeviceModelOffset;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the DeviceSerialNumber field.
        /// </summary>
        public BlockT<ushort> DeviceSerialNumberOffset;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the DeviceVersion field.
        /// </summary>
        public BlockT<ushort> DeviceVersionOffset;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the DeviceFirmwareVersion field.
        /// </summary>
        public BlockT<ushort> DeviceFirmwareVersionOffset;

        /// <summary>
        /// A null-terminated Unicode string that contains the name of the manufacturer of the device.
        /// </summary>
        public BlockString DeviceManufacturer;

        /// <summary>
        /// A null-terminated Unicode string that contains the model name of the device.
        /// </summary>
        public BlockString DeviceModel;

        /// <summary>
        /// A null-terminated Unicode string that contains the serial number of the device.
        /// </summary>
        public BlockString DeviceSerialNumber;

        /// <summary>
        /// A null-terminated Unicode string that contains the version number of the device.
        /// </summary>
        public BlockString DeviceVersion;

        /// <summary>
        /// A null-terminated Unicode string that contains the firmware version of the device.
        /// </summary>
        public BlockString DeviceFirmwareVersion;

        /// <summary>
        /// Parse the AUX_PROTOCOL_DEVICE_IDENTIFICATION structure.
        /// </summary>
        protected override void Parse()
        {
            DeviceManufacturerOffset = ParseT<ushort>();
            DeviceModelOffset = ParseT<ushort>();
            DeviceSerialNumberOffset = ParseT<ushort>();
            DeviceVersionOffset = ParseT<ushort>();
            DeviceFirmwareVersionOffset = ParseT<ushort>();

            if (DeviceManufacturerOffset != 0)
            {
                // TODO: Use actual offset to parse string
                DeviceManufacturer = ParseStringW();
            }

            if (DeviceModelOffset != 0)
            {
                // TODO: Use actual offset to parse string
                DeviceModel = ParseStringW();
            }

            if (DeviceSerialNumberOffset != 0)
            {
                // TODO: Use actual offset to parse string
                DeviceSerialNumber = ParseStringW();
            }

            if (DeviceVersionOffset != 0)
            {
                // TODO: Use actual offset to parse string
                DeviceVersion = ParseStringW();
            }

            if (DeviceFirmwareVersionOffset != 0)
            {
                // TODO: Use actual offset to parse string
                DeviceFirmwareVersion = ParseStringW();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("AUX_PROTOCOL_DEVICE_IDENTIFICATION");
            AddChildBlockT(DeviceManufacturerOffset, "DeviceManufacturerOffset");
            AddChildBlockT(DeviceModelOffset, "DeviceModelOffset");
            AddChildBlockT(DeviceSerialNumberOffset, "DeviceSerialNumberOffset");
            AddChildBlockT(DeviceVersionOffset, "DeviceVersionOffset");
            AddChildBlockT(DeviceFirmwareVersionOffset, "DeviceFirmwareVersionOffset");
            AddChildString(DeviceManufacturer, "DeviceManufacturer");
            AddChildString(DeviceModel, "DeviceModel");
            AddChildString(DeviceSerialNumber, "DeviceSerialNumber");
            AddChildString(DeviceVersion, "DeviceVersion");
            AddChildString(DeviceFirmwareVersion, "DeviceFirmwareVersion");
        }
    }
}