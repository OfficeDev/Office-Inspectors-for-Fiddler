namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Represent an end command.
    /// 2.2.2.6.5 End Command (0x00)
    /// </summary>
    public class EndCommand : Command
    {
        /// <summary>
        /// The Command for end
        /// </summary>
        public byte Command;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains EndCommand.</param>
        public override void Parse(FastTransferStream stream)
        {
            this.Command = stream.ReadByte();
        }
    }
}
