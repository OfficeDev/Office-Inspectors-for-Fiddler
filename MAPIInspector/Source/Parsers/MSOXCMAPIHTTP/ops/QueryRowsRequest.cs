namespace MAPIInspector.Parsers
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    ///  A class indicates the QueryRowsRequest structure.
    ///  2.2.5.12 QueryRows
    /// </summary>
    public class QueryRowsRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specify the authentication type for the connection.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// A Boolean value that specifies whether the State field is present.
        /// </summary>
        public bool HasState;

        /// <summary>
        /// A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container. 
        /// </summary>
        public STAT State;

        /// <summary>
        /// An unsigned integer that specifies the number of structures present in the ExplicitTable field. 
        /// </summary>
        public uint ExplicitTableCount;

        /// <summary>
        /// An array of MinimalEntryID structures that constitute the Explicit Table.
        /// </summary>
        public MinimalEntryID[] ExplicitTable;

        /// <summary>
        /// An unsigned integer that specifies the number of rows the client is requesting.
        /// </summary>
        public uint RowCount;

        /// <summary>
        /// A Boolean value that specifies whether the Columns field is present.
        /// </summary>
        public bool HasColumns;

        /// <summary>
        /// A LargePropertyTagArray structure that specifies the properties that the client requires for each row returned. 
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
        /// Parse the QueryRowsRequest structure.
        /// </summary>
        /// <param name="s">A stream containing QueryRowsRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            Flags = ReadUint();
            HasState = ReadBoolean();

            if (HasState)
            {
                State = new STAT();
                State.Parse(s);
            }

            ExplicitTableCount = ReadUint();
            List<MinimalEntryID> miniEntryIDlist = new List<MinimalEntryID>();

            for (int i = 0; i < ExplicitTableCount; i++)
            {
                MinimalEntryID miniEntryID = new MinimalEntryID();
                miniEntryID.Parse(s);
                miniEntryIDlist.Add(miniEntryID);
            }

            ExplicitTable = miniEntryIDlist.ToArray();
            RowCount = ReadUint();
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