using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXORULE] 2.2.5.1.2.3 OP_DEFER_ACTION ActionData Structure
    /// This type is specified in MS-OXORULE section 2.2.5.1.2.3 OP_DEFER_ACTION ActionData Structure
    /// </summary>
    public class OP_DEFER_ACTION : Block
    {
        /// <summary>
        /// The defer Action data.
        /// </summary>
        public BlockBytes DeferActionData;

        /// <summary>
        /// The length of DeferActionData
        /// </summary>
        private int length;

        /// <summary>
        /// Initializes a new instance of the OP_DEFER_ACTION class.
        /// </summary>
        /// <param name="size">The size.</param>
        public OP_DEFER_ACTION(int size)
        {
            length = size - 9; // 9 is the size of OP_DEFER_ACTION header, which includes RopId, LogonId, InputHandleIndex, OutputHandleIndex, and TableFlags.
        }

        /// <summary>
        /// Parse the OP_DEFER_ACTION structure.
        /// </summary>
        protected override void Parse()
        {
            DeferActionData = ParseBytes(length);
        }

        protected override void ParseBlocks()
        {
            AddChildBytes(DeferActionData, "DeferActionData");
        }
    }
}
