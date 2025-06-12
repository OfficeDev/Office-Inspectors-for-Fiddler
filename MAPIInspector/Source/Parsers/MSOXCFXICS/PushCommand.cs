namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Represent a push command.
    /// 2.2.2.6.1 Push Command (0x01 – 0x06)
    /// </summary>
    public class PushCommand : Command
    {
        /// <summary>
        /// An integer that specifies the number of high-order bytes that the GLOBCNT structures
        /// </summary>
        public byte Command;

        /// <summary>
        /// A byte array that contains the bytes shared by the GLOBCNT structures
        /// </summary>
        public byte[] CommonBytes;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains PushCommand.</param>
        public override void Parse(FastTransferStream stream)
        {
            this.Command = stream.ReadByte();
            this.CommonBytes = stream.ReadBlock(this.Command);
        }
    }
}
