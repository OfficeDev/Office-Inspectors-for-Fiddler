using BlockParser;
using Fiddler;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.1 RopModifyRules ROP
    /// A class indicates the ModifyRulesFlags ROP Response Buffer.
    /// </summary>
    public class ModifyRulesFlags : Block
    {
        private BlockT<byte> Byte0;

        /// <summary>
        /// Unused. This bit MUST be set to zero (0) when sent.
        /// </summary>
        public BlockT<byte> X;

        /// <summary>
        /// If this bit is set, the rules (2) in this request are to replace the existing set of rules (2) in the folder.
        /// </summary>
        public BlockT<byte> R;

        /// <summary>
        /// Parse the ModifyRulesFlags structure.
        /// </summary>
        protected override void Parse()
        {
            Byte0 = ParseT<byte>();
            int index = 0;
            X = CreateBlock(MapiInspector.Utilities.GetBits(Byte0, index, 7), Byte0.Size, Byte0.Offset);
            index = index + 7;
            R = CreateBlock(MapiInspector.Utilities.GetBits(Byte0, index, 1), Byte0.Size, Byte0.Offset);
        }

        protected override void ParseBlocks()
        {
            AddChildBlockT(X, "X");
            AddChildBlockT(R, "R (Replace)");
        }
    }
}
