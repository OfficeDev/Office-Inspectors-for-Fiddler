using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Represent a bitmask command.
    /// [MS-OXCFXICS] 2.2.2.6.3 Bitmask Command (0x42)
    /// </summary>
    public class BitmaskCommand : Command
    {
        /// <summary>
        /// Bitmask Command.
        /// </summary>
        public BlockT<byte> Command;

        /// <summary>
        /// The low-order byte of the low value of the first GLOBCNT range.
        /// </summary>
        public BlockT<byte> StartValue;

        /// <summary>
        /// One bit set for each value within a range, excluding the low value of the first GLOBCNT range.
        /// </summary>
        public BlockT<byte> Bitmask;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        protected override void Parse()
        {
            Command = ParseT<byte>();
            StartValue = ParseT<byte>();
            Bitmask = ParseT<byte>();
        }

        protected override void ParseBlocks()
        {
            Text = "BitmaskCommand";
            if (Command != null) AddChild(Command, $"Command: {Command.Data:X2}");
            AddChildBlockT(StartValue, "StartValue");
            if (Bitmask != null) AddChild(Bitmask, $"Bitmask: {Bitmask.Data:X2}");
        }
    }
}
