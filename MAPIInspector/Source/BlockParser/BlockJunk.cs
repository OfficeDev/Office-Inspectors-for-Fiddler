using System.Drawing;

namespace BlockParser
{
    public class BlockJunk : BlockInteresting
    {
        public override Color BackColor => Color.Coral;
        public override string InterestingLabel => "Exceptions found in this block";

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
