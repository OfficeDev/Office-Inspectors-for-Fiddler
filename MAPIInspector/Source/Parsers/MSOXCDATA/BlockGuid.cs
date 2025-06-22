namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System;

    /// <summary>
    /// The BlockGuid  class.
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
            if (value != null) SetText($"{Guids.ToString(value.Data)}");
        }
    }
}