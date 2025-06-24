namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.2.6.3 RopSaveChangesMessage ROP
    /// A class indicates the RopSaveChangesMessage ROP request Buffer.
    /// </summary>
    public class RopSaveChangesMessageRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table that is referenced in the response.
        /// </summary>
        public byte ResponseHandleIndex;

        /// <summary>
        ///  An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// A flags structure that contains flags that specify how the save operation behaves.
        /// </summary>
        public SaveFlags SaveFlags;

        /// <summary>
        /// Parse the RopSaveChangesMessageRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSaveChangesMessageRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            ResponseHandleIndex = ReadByte();
            InputHandleIndex = ReadByte();
            SaveFlags = (SaveFlags)ReadByte();
        }
    }
}
