using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.10 RopSeekRowBookmark ROP
    /// A class indicates the RopSeekRowBookmark ROP Response Buffer.
    /// </summary>
    public class RopSeekRowBookmarkResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// A Boolean that specifies whether the bookmark target is no longer visible.
        /// </summary>
        public bool? RowNoLongerVisible;

        /// <summary>
        /// A Boolean that specifies whether the full number of rows sought past was less than the number that was requested.
        /// </summary>
        public bool? HasSoughtLess;

        /// <summary>
        /// An unsigned integer that specifies the direction and number of rows sought.
        /// </summary>
        public uint? RowsSought;

        /// <summary>
        /// Parse the RopSeekRowBookmarkResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSeekRowBookmarkResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                RowNoLongerVisible = ReadBoolean();
                HasSoughtLess = ReadBoolean();
                RowsSought = ReadUint();
            }
        }
    }
}
