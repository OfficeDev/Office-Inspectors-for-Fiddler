namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.2.1.13 RopGetHierarchyTable ROP
    /// The RopGetHierarchyTable ROP ([MS-OXCROPS] section 2.2.4.13) is used to retrieve the hierarchy table for a folder. 
    /// </summary>
    public class RopGetHierarchyTableRequest : BaseStructure
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
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored. 
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// These flags control the type of table.
        /// </summary>
        public HierarchyTableFlags TableFlags;

        /// <summary>
        /// Parse the RopGetHierarchyTableRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetHierarchyTableRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            OutputHandleIndex = ReadByte();
            TableFlags = (HierarchyTableFlags)ReadByte();
        }
    }
}