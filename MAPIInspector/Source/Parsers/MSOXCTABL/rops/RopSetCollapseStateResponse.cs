using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.20 RopSetCollapseState ROP
    ///  A class indicates the RopSetCollapseState ROP Response Buffer.
    /// </summary>
    public class RopSetCollapseStateResponse : BaseStructure
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
        /// An unsigned integer that specifies the size of the Bookmark field.
        /// </summary>
        public ushort? BookmarkSize;

        /// <summary>
        /// An array of bytes that specifies the origin for the seek operation. 
        /// </summary>
        public byte?[] Bookmark;

        /// <summary>
        /// Parse the RopSetCollapseStateResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetCollapseStateResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                BookmarkSize = ReadUshort();
                Bookmark = ConvertArray(ReadBytes((int)BookmarkSize));
            }
        }
    }
}
