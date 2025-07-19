using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.3.1 Property Values
    /// 2.3.1.3 Binary_r
    /// A class indicates the Binary_r structure.
    /// </summary>
    public class Binary_r : Block
    {
        /// <summary>
        /// The number of uninterpreted bytes represented in structure. value MUST NOT exceed 2,097,152.
        /// </summary>
        public BlockT<uint> Cb;

        /// <summary>
        /// The uninterpreted bytes.
        /// </summary>
        public BlockBytes Lpb;

        /// <summary>
        /// Parse the Binary_r payload of session.
        /// </summary>
        protected override void Parse()
        {
            Cb = ParseT<uint>();
            Lpb = ParseBytes(Cb);
        }

        protected override void ParseBlocks()
        {
            Text = "Binary_r";
            AddChildBlockT(Cb, "cb");
            AddChildBytes(Lpb, "lpb");
        }
    }
}
