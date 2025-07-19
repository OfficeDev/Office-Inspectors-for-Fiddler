using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the SeekEntriesRequest structure.
    /// 2.2.5.16 SeekEntries
    /// </summary>
    public class SeekEntriesRequest : Block
    {
        /// <summary>
        /// Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        /// </summary>
        public BlockT<uint> Reserved;

        /// <summary>
        /// A Boolean value that specifies whether the State field is present.
        /// </summary>
        public BlockT<bool> HasState;

        /// <summary>
        /// A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container.
        /// </summary>
        public STAT State;

        /// <summary>
        /// A Boolean value that specifies whether the Target field is present.
        /// </summary>
        public BlockT<bool> HasTarget;

        /// <summary>
        /// An AddressBookTaggedPropertyValue structure that specifies the property value being sought.
        /// </summary>
        public AddressBookTaggedPropertyValue Target;

        /// <summary>
        /// A Boolean value that specifies whether the ExplicitTableCount and ExplicitTable fields are present.
        /// </summary>
        public BlockT<bool> HasExplicitTable;

        /// <summary>
        /// An unsigned integer that specifies the number of structures present in the ExplicitTable field.
        /// </summary>
        public BlockT<uint> ExplicitTableCount;

        /// <summary>
        /// An array of MinimalEntryID structures that constitute an Explicit Table.
        /// </summary>
        public MinimalEntryID[] ExplicitTable;

        /// <summary>
        /// A Boolean value that specifies whether the Columns field is present.
        /// </summary>
        public BlockT<bool> HasColumns;

        /// <summary>
        /// A LargePropertyTagArray structure (section 2.2.1.8) that specifies the columns that the client is requesting.
        /// </summary>
        public LargePropertyTagArray Columns;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public BlockT<uint> AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the SeekEntriesRequest structure.
        /// </summary>
        protected override void Parse()
        {
            Reserved = ParseT<uint>();
            HasState = ParseAs<byte, bool>();
            if (HasState) State = Parse<STAT>();
            HasTarget = ParseAs<byte, bool>();
            if (HasTarget) Target = Parse<AddressBookTaggedPropertyValue>();
            HasExplicitTable = ParseAs<byte, bool>();
            if (HasExplicitTable)
            {
                ExplicitTableCount = ParseT<uint>();
                var miniEIDList = new List<MinimalEntryID>();
                for (int i = 0; i < ExplicitTableCount; i++)
                {
                    miniEIDList.Add(Parse<MinimalEntryID>());
                }

                ExplicitTable = miniEIDList.ToArray();
            }

            HasColumns = ParseAs<byte, bool>();
            if (HasColumns) Columns = Parse<LargePropertyTagArray>();
            AuxiliaryBufferSize = ParseT<uint>();
            if (AuxiliaryBufferSize > 0) AuxiliaryBuffer = Parse<ExtendedBuffer>();
        }

        protected override void ParseBlocks()
        {
            Text = "SeekEntriesRequest";
            AddChildBlockT(Reserved, "Reserved");
            AddChildBlockT(HasState, "HasState");
            AddChild(State, "State");
            AddChildBlockT(HasTarget, "HasTarget");
            AddChild(Target, "Target");
            AddChildBlockT(HasExplicitTable, "HasExplicitTable");
            AddChildBlockT(ExplicitTableCount, "ExplicitTableCount");
            AddLabeledChildren(ExplicitTable, "ExplicitTable");
            AddChildBlockT(HasColumns, "HasColumns");
            AddChild(Columns, "Columns");
            AddChildBlockT(AuxiliaryBufferSize, "AuxiliaryBufferSize");
            AddChild(AuxiliaryBuffer, "AuxiliaryBuffer");
        }
    }
}