using System.IO;
using System.Text;

namespace MAPIInspector.Parsers
{
    /// <summary>
    ///  A class indicates the AUX_PROTOCOL_DEVICE_IDENTIFICATION Auxiliary Block Structure
    ///  Section 2.2.2.2 AUX_HEADER Structure
    ///  Section 2.2.2.2.22   AUX_PROTOCOL_DEVICE_IDENTIFICATION Auxiliary Block Structure
    /// </summary>
    public class AUX_PROTOCOL_DEVICE_IDENTIFICATION : BaseStructure
    {
        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure, as specified in section 2.2.2.2, to the DeviceManufacturer field. 
        /// </summary>
        public ushort DeviceManufacturerOffset;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the DeviceModel field. 
        /// </summary>
        public ushort DeviceModelOffset;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the DeviceSerialNumber field. 
        /// </summary>
        public ushort DeviceSerialNumberOffset;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the DeviceVersion field. 
        /// </summary>
        public ushort DeviceVersionOffset;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the DeviceFirmwareVersion field. 
        /// </summary>
        public ushort DeviceFirmwareVersionOffset;

        /// <summary>
        /// A null-terminated Unicode string that contains the name of the manufacturer of the device. 
        /// </summary>
        public MAPIString DeviceManufacturer;

        /// <summary>
        /// A null-terminated Unicode string that contains the model name of the device. 
        /// </summary>
        public MAPIString DeviceModel;

        /// <summary>
        /// A null-terminated Unicode string that contains the serial number of the device. 
        /// </summary>
        public MAPIString DeviceSerialNumber;

        /// <summary>
        /// A null-terminated Unicode string that contains the version number of the device. 
        /// </summary>
        public MAPIString DeviceVersion;

        /// <summary>
        /// A null-terminated Unicode string that contains the firmware version of the device. 
        /// </summary>
        public MAPIString DeviceFirmwareVersion;

        /// <summary>
        /// Parse the AUX_PROTOCOL_DEVICE_IDENTIFICATION structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PROTOCOL_DEVICE_IDENTIFICATION structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            DeviceManufacturerOffset = ReadUshort();
            DeviceModelOffset = ReadUshort();
            DeviceSerialNumberOffset = ReadUshort();
            DeviceVersionOffset = ReadUshort();
            DeviceFirmwareVersionOffset = ReadUshort();

            if (DeviceManufacturerOffset != 0)
            {
                DeviceManufacturer = new MAPIString(Encoding.Unicode);
                DeviceManufacturer.Parse(s);
            }

            if (DeviceModelOffset != 0)
            {
                DeviceModel = new MAPIString(Encoding.Unicode);
                DeviceModel.Parse(s);
            }

            if (DeviceSerialNumberOffset != 0)
            {
                DeviceSerialNumber = new MAPIString(Encoding.Unicode);
                DeviceSerialNumber.Parse(s);
            }

            if (DeviceVersionOffset != 0)
            {
                DeviceVersion = new MAPIString(Encoding.Unicode);
                DeviceVersion.Parse(s);
            }

            if (DeviceFirmwareVersionOffset != 0)
            {
                DeviceFirmwareVersion = new MAPIString(Encoding.Unicode);
                DeviceFirmwareVersion.Parse(s);
            }
        }
    }
}