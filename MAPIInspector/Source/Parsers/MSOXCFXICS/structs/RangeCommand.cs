using BlockParser;

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
        public BlockT<byte> Command;

        /// <summary>
        /// The low value of the range.
        /// </summary>
        public BlockBytes LowValue;

        /// <summary>
        /// The high value of the range.
        /// </summary>
        public BlockBytes HighValue;

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
        protected override void Parse()
        {
            Command = ParseT<byte>(parser);
            LowValue = ParseBytes(parser, (int)length);
            HighValue = ParseBytes(parser, (int)length);
        }

        protected override void ParseBlocks()
        {
            SetText("RangeCommand");
            if (Command != null) AddChild(Command, $"Command:{Command.Data}");
            AddLabeledChild(LowValue, "LowValue");
            AddLabeledChild(HighValue, "HighValue");
        }
    }
}
