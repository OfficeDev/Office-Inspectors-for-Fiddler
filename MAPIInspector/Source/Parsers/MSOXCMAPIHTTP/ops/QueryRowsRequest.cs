using BlockParser;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Windows.Forms.Design;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the QueryRowsRequest structure.
    /// 2.2.5.12 QueryRows
    /// </summary>
    public class QueryRowsRequest : Block
    {
        /// <summary>
        /// An unsigned integer that specify the authentication type for the connection.
        /// </summary>
        public BlockT<uint> Flags;

        /// <summary>
        /// A Boolean value that specifies whether the State field is present.
        /// </summary>
        public BlockT<bool> HasState;

        /// <summary>
        /// A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container.
        /// </summary>
        public STAT State;

        /// <summary>
        /// An unsigned integer that specifies the number of structures present in the ExplicitTable field.
        /// </summary>
        public BlockT<uint> ExplicitTableCount;

        /// <summary>
        /// An array of MinimalEntryID structures that constitute the Explicit Table.
        /// </summary>
        public MinimalEntryID[] ExplicitTable;

        /// <summary>
        /// An unsigned integer that specifies the number of rows the client is requesting.
        /// </summary>
        public BlockT<uint> RowCount;

        /// <summary>
        /// A Boolean value that specifies whether the Columns field is present.
        /// </summary>
        public BlockT<bool> HasColumns;

        /// <summary>
        /// A LargePropertyTagArray structure that specifies the properties that the client requires for each row returned.
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
        /// Parse the QueryRowsRequest structure.
        /// </summary>
        protected override void Parse()
        {
            Flags = ParseT<uint>();
            HasState = ParseAs<byte, bool>();
            if (HasState) State = Parse<STAT>();
            ExplicitTableCount = ParseT<uint>();
            var miniEntryIDlist = new List<MinimalEntryID>();
            for (int i = 0; i < ExplicitTableCount; i++)
            {
                miniEntryIDlist.Add(Parse<MinimalEntryID>());
            }

            ExplicitTable = miniEntryIDlist.ToArray();
            RowCount = ParseT<uint>();
            HasColumns = ParseAs<byte, bool>();
            if (HasColumns) Columns = Parse<LargePropertyTagArray>();
            AuxiliaryBufferSize = ParseT<uint>();
            if (AuxiliaryBufferSize > 0) AuxiliaryBuffer = Parse<ExtendedBuffer>();
        }

        protected override void ParseBlocks()
        {
            SetText("QueryRowsRequest");
            AddChildBlockT(Flags, "Flags");
            AddChildBlockT(HasState, "HasState");
            AddChild(State, "State");
            AddChildBlockT(ExplicitTableCount, "ExplicitTableCount");
            AddLabeledChildren(ExplicitTable, "ExplicitTable");
            AddChildBlockT(RowCount, "RowCount");
            AddChildBlockT(HasColumns, "HasColumns");
            AddChild(Columns, "Columns");
            AddChildBlockT(AuxiliaryBufferSize, "AuxiliaryBufferSize");
            AddChild(AuxiliaryBuffer, "AuxiliaryBuffer");
        }
    }
}