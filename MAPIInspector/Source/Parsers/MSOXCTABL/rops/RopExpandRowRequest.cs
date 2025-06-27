using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.17 RopExpandRow ROP
    /// The RopExpandRow ROP ([MS-OXCROPS] section 2.2.5.16) expands a collapsed category of a table and returns the rows that belong in the newly expanded category. 
    /// </summary>
    public class RopExpandRowRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the maximum number of expanded rows to return data for.
        /// </summary>
        public ushort MaxRowCount;

        /// <summary>
        /// An identifier that specifies the category to be expanded.
        /// </summary>
        public long CategoryId;

        /// <summary>
        /// Parse the RopExpandRowRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopExpandRowRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            MaxRowCount = ReadUshort();
            CategoryId = ReadINT64();
        }
    }
}
