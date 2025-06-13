namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Represent a bitmask command.
    /// 2.2.2.6.3 Bitmask Command (0x42)

    /// </summary>
    public class BitmaskCommand : Command
    {
        /// <summary>
        /// Bitmask Command.
        /// </summary>
        public byte Command;

        /// <summary>
        /// The low-order byte of the low value of the first GLOBCNT range.
        /// </summary>
        public byte StartValue;

        /// <summary>
        /// One bit set for each value within a range, excluding the low value of the first GLOBCNT range.
        /// </summary>
        public byte Bitmask;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains BitmaskCommand.</param>
        public override void Parse(FastTransferStream stream)
        {
            this.Command = stream.ReadByte();
            this.StartValue = stream.ReadByte();
            this.Bitmask = stream.ReadByte();
        }
    }
}
