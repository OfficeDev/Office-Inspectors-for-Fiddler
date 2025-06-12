namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Represent a range command.
    /// 2.2.2.6.4 Range Command (0x52)
    /// </summary>
    public class RangeCommand : Command
    {
        /// <summary>
        /// Bitmask Command.
        /// </summary>
        public byte Command;

        /// <summary>
        /// The low value of the range.
        /// </summary>
        public byte[] LowValue;

        /// <summary>
        /// The high value of the range.
        /// </summary>
        public byte[] HighValue;

        /// <summary>
        /// The length of the LowValue and hignValue.
        /// </summary>
        private uint length;

        /// <summary>
        /// Initializes a new instance of the RangeCommand class.
        /// </summary>
        /// <param name="length">The length of the LowValue and hignValue.</param>
        public RangeCommand(uint length)
        {
            this.length = length;
        }

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains RangeCommand.</param>
        public override void Parse(FastTransferStream stream)
        {
            this.Command = stream.ReadByte();
            this.LowValue = stream.ReadBlock((int)this.length);
            this.HighValue = stream.ReadBlock((int)this.length);
        }
    }
}
