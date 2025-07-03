using System.Collections.Generic;
using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the SeekEntriesRequest structure.
    /// 2.2.5.16 SeekEntries
    /// </summary>
    public class SeekEntriesRequest : BaseStructure
    {
        /// <summary>
        /// Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// A Boolean value that specifies whether the State field is present.
        /// </summary>
        public bool HasState;

        /// <summary>
        /// A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container.
        /// </summary>
        public STAT State;

        /// <summary>
        /// A Boolean value that specifies whether the Target field is present.
        /// </summary>
        public bool HasTarget;

        /// <summary>
        /// An AddressBookTaggedPropertyValue structure that specifies the property value being sought.
        /// </summary>
        public AddressBookTaggedPropertyValue Target;

        /// <summary>
        /// A Boolean value that specifies whether the ExplicitTableCount and ExplicitTable fields are present.
        /// </summary>
        public bool HasExplicitTable;

        /// <summary>
        /// An unsigned integer that specifies the number of structures present in the ExplicitTable field.
        /// </summary>
        public uint ExplicitTableCount;

        /// <summary>
        /// An array of MinimalEntryID structures that constitute an Explicit Table.
        /// </summary>
        public MinimalEntryID[] ExplicitTable;

        /// <summary>
        /// A Boolean value that specifies whether the Columns field is present.
        /// </summary>
        public bool HasColumns;

        /// <summary>
        /// A LargePropertyTagArray structure (section 2.2.1.8) that specifies the columns that the client is requesting.
        /// </summary>
        public LargePropertyTagArray Columns;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the SeekEntriesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing SeekEntriesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            Reserved = ReadUint();
            HasState = ReadBoolean();

            if (HasState)
            {
                State = new STAT();
                State.Parse(s);
            }

            HasTarget = ReadBoolean();

            if (HasTarget)
            {
                Target = new AddressBookTaggedPropertyValue();
                Target.Parse(s);
            }

            HasExplicitTable = ReadBoolean();

            if (HasExplicitTable)
            {
                ExplicitTableCount = ReadUint();
                List<MinimalEntryID> miniEIDList = new List<MinimalEntryID>();

                for (int i = 0; i < ExplicitTableCount; i++)
                {
                    MinimalEntryID miniEID = new MinimalEntryID();
                    miniEID.Parse(s);
                    miniEIDList.Add(miniEID);
                }

                ExplicitTable = miniEIDList.ToArray();
            }

            HasColumns = ReadBoolean();

            if (HasColumns)
            {
                Columns = new LargePropertyTagArray();
                Columns.Parse(s);
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