using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the AUX_TYPE_EXCEPTION_TRACE Auxiliary Block Structure
    /// </summary>
    public class AUX_EXCEPTION_TRACE : Block
    {
        /// <summary>
        /// A flag that indicates that the server combines capabilities on a single endpoint.
        /// </summary>
        public BlockT<uint> RopIndex;
        public BlockString[] ExceptionMessage;

        /// <summary>
        /// Parse the AUX_ENDPOINT_CAPABILITIES structure.
        /// </summary>
        protected override void Parse()
        {
            RopIndex = new BlockT<uint>(parser);
            BlockString str = null;
            var exceptionMessage = new List<BlockString>();

            while (!parser.Empty)
            {
                str = ParseStringLineA(parser);
                exceptionMessage.Add(str);

                if (str.Empty) break;
            }

            ExceptionMessage = exceptionMessage.ToArray();
        }

        protected override void ParseBlocks()
        {
            Text = "AUX_EXCEPTION_TRACE";
            AddChildBlockT(RopIndex, "RopIndex");
            AddLabeledChildren(ExceptionMessage, "ExceptionMessage");
        }
    }
}
