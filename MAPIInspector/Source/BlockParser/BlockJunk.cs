using System.Drawing;

namespace BlockParser
{
    public class BlockJunk : Block
    {
        public static readonly Color BackColor = Color.Coral;

        string Label;
        BlockBytes junkData;

        public BlockJunk(string label)
        {
            Label = label;
        }

        protected override void Parse()
        {
            junkData = ParseBytes(parser, parser.RemainingBytes);
        }

        protected override void ParseBlocks()
        {
            Text = Label;
            AddChild(junkData);
        }
    }
}
