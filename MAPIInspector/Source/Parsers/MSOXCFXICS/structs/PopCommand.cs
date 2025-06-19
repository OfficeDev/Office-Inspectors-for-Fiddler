using BlockParser;

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
            SetText("PopCommand");
            if (Command != null) AddChild(Command, $"Command:{Command.Data}");
        }
    }
}
