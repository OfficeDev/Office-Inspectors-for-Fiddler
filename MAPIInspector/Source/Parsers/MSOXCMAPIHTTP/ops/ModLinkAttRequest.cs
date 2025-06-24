namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    ///  A class indicates the ModLinkAttRequest structure.
    ///  2.2.5.10 ModLinkAtt
    /// </summary>
    public class ModLinkAttRequest : BaseStructure
    {
        /// <summary>
        /// A set of bit flags that specify options to the server. 
        /// </summary>
        public uint Flags;

        /// <summary>
        /// A PropertyTag structure that specifies the property to be modified.
        /// </summary>
        public PropertyTag PropertyTag;

        /// <summary>
        /// A MinimalEntryID structure that specifies the Minimal Entry ID of the address book row to be modified.
        /// </summary>
        public MinimalEntryID MinimalId;

        /// <summary>
        /// A Boolean value that specifies whether the EntryIdCount and EntryIds fields are present.
        /// </summary>
        public bool HasEntryIds;

        /// <summary>
        /// An unsigned integer that specifies the count of structures in the EntryIds field. 
        /// </summary>
        public uint? EntryIdCount;

        /// <summary>
        /// An array of entry IDs, each of which is either an EphemeralEntryID structure or a PermanentEntryID structure. 
        /// </summary>
        public object[] EntryIds;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the ModLinkAttRequest structure.
        /// </summary>
        /// <param name="s">A stream containing ModLinkAttRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            Flags = ReadUint();
            PropertyTag = Block.Parse<PropertyTag>(s);
            MinimalId = new MinimalEntryID();
            MinimalId.Parse(s);
            HasEntryIds = ReadBoolean();

            if (HasEntryIds)
            {
                EntryIdCount = ReadUint();
                List<object> tempObj = new List<object>();

                for (int i = 0; i < EntryIdCount; i++)
                {
                    var cb = ReadUint(); //See details on MS-OXNSPI  3.1.4.1.15 NspiModLinkAtt (Opnum 14) and 2.2.2.3 Binary_r Structure
                    byte currentByte = ReadByte();
                    s.Position -= 1;
                    if (currentByte == 0x87)
                    {
                        EphemeralEntryID ephemeralEntryID = new EphemeralEntryID();
                        ephemeralEntryID.Parse(s);
                        tempObj.Add(ephemeralEntryID);
                    }
                    else if (currentByte == 0x00)
                    {
                        PermanentEntryID permanentEntryID = new PermanentEntryID();
                        permanentEntryID.Parse(s);
                        tempObj.Add(permanentEntryID);
                    }
                    else
                    {
                        uint length = ReadUint();
                        byte[] byteleft = ReadBytes((int)length);
                    }
                }

                EntryIds = tempObj.ToArray();
            }

            AuxiliaryBufferSize = ReadUint();

            if (AuxiliaryBufferSize > 0)
            {
                AuxiliaryBuffer = new ExtendedBuffer();
                AuxiliaryBuffer.Parse(s);
            }
        }
    }
}