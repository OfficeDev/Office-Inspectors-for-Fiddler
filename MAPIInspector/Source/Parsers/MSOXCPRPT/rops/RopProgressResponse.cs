namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.2.23 RopProgress
    ///  A class indicates the RopProgress ROP Response Buffer.
    /// </summary>
    public class RopProgressResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public byte? LogonId;

        /// <summary>
        /// An unsigned integer that specifies the number of tasks completed.
        /// </summary>
        public uint? CompletedTaskCount;

        /// <summary>
        /// An unsigned integer that specifies the total number of tasks.
        /// </summary>
        public uint? TotalTaskCount;

        /// <summary>
        /// Parse the RopProgressResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopProgressResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                LogonId = ReadByte();
                CompletedTaskCount = ReadUint();
                TotalTaskCount = ReadUint();
            }
        }
    }
}
