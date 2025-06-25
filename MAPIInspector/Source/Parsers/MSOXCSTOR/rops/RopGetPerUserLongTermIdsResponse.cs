namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.1.10 RopGetPerUserLongTermIds
    ///  A class indicates the RopGetPerUserLongTermIds ROP Response Buffer.
    /// </summary>
    public class RopGetPerUserLongTermIdsResponse : BaseStructure
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
        /// An unsigned integer that specifies the number of structures in the LongTermIds field.
        /// </summary>
        public ushort? LongTermIdCount;

        /// <summary>
        /// An array of LongTermID structures that specifies which folders the user has per-user information about. 
        /// </summary>
        public LongTermID[] LongTermIds;

        /// <summary>
        /// Parse the RopGetPerUserLongTermIdsResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetPerUserLongTermIdsResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                LongTermIdCount = ReadUshort();
                LongTermIds = new LongTermID[(int)LongTermIdCount];
                for (int i = 0; i < LongTermIdCount; i++)
                {
                    LongTermIds[i] = new LongTermID();
                    LongTermIds[i].Parse(s);
                }
            }
        }
    }
}
