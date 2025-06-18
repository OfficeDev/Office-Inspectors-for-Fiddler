using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Represent a push command.
    /// 2.2.2.6.1 Push Command (0x01 – 0x06)
    /// </summary>
    public class PushCommand : Command
    {
        /// <summary>
        /// An integer that specifies the number of high-order bytes that the GLOBCNT structures, as specified in section 2.2.2.5, share
        /// </summary>
        public BlockT<byte> Command;

        /// <summary>
        /// A byte array that contains the bytes shared by the GLOBCNT structures
        /// </summary>
        public BlockBytes CommonBytes;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        protected override void Parse()
        {
            Command = ParseT<byte>(parser);
            CommonBytes = ParseBytes(parser, Command.Data, 6);
        }

        protected override void ParseBlocks()
        {
            SetText("PushCommand");
            if (Command != null) AddChild(Command, $"Command:{Command.Data}");
            AddLabeledChild(CommonBytes, "CommonBytes");
        }
    }
}