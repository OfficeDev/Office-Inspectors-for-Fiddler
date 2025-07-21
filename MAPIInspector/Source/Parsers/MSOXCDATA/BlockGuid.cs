using BlockParser;
using System;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The BlockGuid class.
    /// </summary>
    public class BlockGuid : Block
    {
        public BlockT<Guid> value;

        protected override void Parse()
        {
            value = ParseT<Guid>();
        }

        protected override void ParseBlocks()
        {
            if (value != null) Text = $"{Guids.ToString(value)}";
        }
    }
}