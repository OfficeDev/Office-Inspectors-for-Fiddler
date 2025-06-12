namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Represent a pop command.
    /// 2.2.2.6.2 Pop Command (0x50)
    /// </summary>
    public class PopCommand : Command
    {
        /// <summary>
        /// The Command for pop
        /// </summary>
        public byte Command;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains PopCommand.</param>
        public override void Parse(FastTransferStream stream)
        {
            this.Command = stream.ReadByte();
        }
    }
}
