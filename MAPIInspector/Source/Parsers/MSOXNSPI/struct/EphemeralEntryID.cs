using System;
using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.9 EntryIDs
    /// 2.2.9.2 EphemeralEntryID
    /// A class indicates the EphemeralEntryID structure.
    /// </summary>
    public class EphemeralEntryID : BaseStructure
    {
        /// <summary>
        /// The type of ID.
        /// </summary>
        public byte Type;

        /// <summary>
        /// Reserved, generally value is a constant 0x00.
        /// </summary>
        public byte R1;

        /// <summary>
        /// Reserved, generally value is a constant 0x00.
        /// </summary>
        public byte R2;

        /// <summary>
        /// Reserved, generally value is a constant 0x00.
        /// </summary>
        public byte R3;

        /// <summary>
        /// A FlatUID_r value contains the GUID of the server that issued Ephemeral Entry ID.
        /// </summary>
        public Guid ProviderUID;

        /// <summary>
        /// Reserved, generally value is a constant 0x00000001.
        /// </summary>
        public uint R4;

        /// <summary>
        /// The display type of the object specified by Ephemeral Entry ID.
        /// </summary>
        public DisplayTypeValues DisplayType;

        /// <summary>
        /// The Minimal Entry ID of object.
        /// </summary>
        public MinimalEntryID Mid;

        /// <summary>
        /// Parse the EphemeralEntryID payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            Type = ReadByte();
            R1 = ReadByte();
            R2 = ReadByte();
            R3 = ReadByte();
            ProviderUID = ReadGuid();
            R4 = ReadUint();
            DisplayType = (DisplayTypeValues)ReadUint();
            Mid = new MinimalEntryID();
            Mid.Parse(s);
        }
    }
}
