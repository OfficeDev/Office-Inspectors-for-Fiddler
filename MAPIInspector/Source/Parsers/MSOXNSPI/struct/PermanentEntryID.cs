using System;
using System.IO;
using System.Text;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.9 EntryIDs
    /// 2.2.9.3 PermanentEntryID
    /// A class indicates the PermanentEntryID structure.
    /// </summary>
    public class PermanentEntryID : BaseStructure
    {
        /// <summary>
        /// The type of ID.
        /// </summary>
        public byte IDType;

        /// <summary>
        /// Reserved. All clients and servers MUST set value to the constant 0x00.
        /// </summary>
        public byte R1;

        /// <summary>
        /// Reserved. All clients and servers MUST set value to the constant 0x00.
        /// </summary>
        public byte R2;

        /// <summary>
        /// Reserved. All clients and servers MUST set value to the constant 0x00.
        /// </summary>
        public byte R3;

        /// <summary>
        /// A FlatUID_r value that contains the constant GUID specified in Permanent Entry ID GUID,
        /// </summary>
        public Guid ProviderUID;

        /// <summary>
        /// Reserved. All clients and servers MUST set value to the constant 0x00000001.
        /// </summary>
        public uint R4;

        /// <summary>
        /// The display type of the object specified by Permanent Entry ID.
        /// </summary>
        public DisplayTypeValues DisplayTypeString;

        /// <summary>
        /// The DN (1) of the object specified by Permanent Entry ID.
        /// </summary>
        public MAPIString DistinguishedName;

        /// <summary>
        /// Parse the PermanentEntryID payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            IDType = ReadByte();
            R1 = ReadByte();
            R2 = ReadByte();
            R3 = ReadByte();
            ProviderUID = ReadGuid();
            R4 = ReadUint();
            DisplayTypeString = (DisplayTypeValues)ReadUint();
            DistinguishedName = new MAPIString(Encoding.ASCII);
            DistinguishedName.Parse(s);
        }
    }
}
