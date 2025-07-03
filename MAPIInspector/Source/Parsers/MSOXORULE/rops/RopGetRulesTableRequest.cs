namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2 RopGetRulesTable ROP
    /// The RopGetRulesTable ROP ([MS-OXCROPS] section 2.2.11.2) creates a Table object through which the client can access the standard rules in a folder using table operations as specified in [MS-OXCTABL]. 
    /// </summary>
    public class RopGetRulesTableRequest : BaseStructure
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
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored. 
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// A flags structure that contains flags that control the type of table. 
        /// </summary>
        public TableFlags TableFlags;

        /// <summary>
        /// Parse the RopGetRulesTableRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetRulesTableRequest structure.</param>
        public override void Parse(System.IO.Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            OutputHandleIndex = ReadByte();
            TableFlags = (TableFlags)ReadByte();
        }
    }
}
