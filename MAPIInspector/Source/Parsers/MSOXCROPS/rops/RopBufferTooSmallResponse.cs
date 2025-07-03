using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.15.1 RopBufferTooSmall
    /// A class indicates the RopBufferTooSmall ROP Response Buffer.
    /// </summary>
    public class RopBufferTooSmallResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the size required for the ROP output buffer.
        /// </summary>
        public ushort SizeNeeded;

        /// <summary>
        /// An array of bytes that contains the section of the ROP input buffer that was not executed because of the insufficient size of the ROP output buffer.
        /// </summary>
        public byte[] RequestBuffers;

        /// <summary>
        /// An unsigned integer that specifies the size of RequestBuffers.
        /// </summary>
        private uint requestBuffersSize;

        /// <summary>
        /// Initializes a new instance of the RopBufferTooSmallResponse class.
        /// </summary>
        /// <param name="requestBuffersSize"> The size of RequestBuffers.</param>
        public RopBufferTooSmallResponse(uint requestBuffersSize)
        {
            requestBuffersSize = requestBuffersSize;
        }

        /// <summary>
        /// Parse the RopBufferTooSmallResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopBufferTooSmallResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            SizeNeeded = ReadUshort();
            RequestBuffers = ReadBytes((int)requestBuffersSize);
        }
    }
}
