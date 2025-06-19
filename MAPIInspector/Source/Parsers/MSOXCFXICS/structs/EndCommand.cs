using BlockParser;

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
        public BlockT<byte> Command;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        protected override void Parse()
        {
            Command = ParseT<byte>();
        }

        protected override void ParseBlocks()
        {
            SetText("EndCommand");
            AddChildBlockT(Command, "Command");
        }
    }
}
