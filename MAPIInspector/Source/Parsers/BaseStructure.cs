using BlockParser;
using System.Windows.Forms;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// BaseStructure class
    /// </summary>
    public partial class BaseStructure
    {
        /// <summary>
        /// Boolean value, if payload is compressed or obfuscated, value is true. otherwise, value is false.
        /// </summary>
        public static bool IsCompressedXOR = false;

        /// <summary>
        /// This field is for rgbOutputBuffer or ExtendedBuffer_Input in MAPIHTTP layer
        /// </summary>
        private static int compressBufferindex = 0;

        const bool DebugNodes = false;

        /// <summary>
        /// Recursively adds a BlockParser.Block and its children to a TreeNode structure.
        /// </summary>
        /// <param name="block">The block to add as a node.</param>
        /// <param name="blockRootOffset">The root offset to calculate the absolute position of the block.</param>
        /// <returns>The TreeNode representing the block and its children.</returns>
        public static TreeNode AddBlock(Block block, int blockRootOffset)
        {
            // Clean up embedded null characters in the block text for display purposes
            var text = block.Text.Replace("\0", "\\0");
            const int maxNodeLength = 100;
            // Truncate the text if it exceeds 100 characters for display purposes
            if (text.Length > maxNodeLength)
            {
                text = text.Substring(0, maxNodeLength) + "...";
            }
            var blockOffset = blockRootOffset + (int)block.Offset;
            var position = new Position(blockOffset, (int)block.Size)
            {
                SourceBlock = block
            };
            var node = new TreeNode(text) { Tag = position };

            if (DebugNodes)
            {
                node.BackColor = System.Drawing.Color.PaleGreen;
                System.Drawing.Color backColor;
                if (string.IsNullOrEmpty(text))
                {
                    backColor = System.Drawing.Color.Tomato;
                }
                else
                {
                    backColor = System.Drawing.Color.SkyBlue;
                }

                var type = block.GetType();
                var typeName = type.Name;
                var args = type.GetGenericArguments();
                if (args.Length > 0)
                {
                    typeName += $"({args[0].FullName})";
                }

                var x = IsCompressedXOR ? " X" : "";
                var debugNode = new TreeNode($"Block: {typeName} at {blockRootOffset:X}+{block.Offset:X}={blockOffset:X} with size {block.Size} bytes{x} {compressBufferindex}")
                {
                    BackColor = backColor,
                    Tag = "ignore"
                };
                node.Nodes.Add(debugNode);
            }

            if (block is RPC_HEADER_EXT header)
            {
                IsCompressedXOR = header.Flags.Data.HasFlag(RpcHeaderFlags.XorMagic) ||
                    header.Flags.Data.HasFlag(RpcHeaderFlags.Compressed);
            }

            if (block.Text == "RgbOutputBuffers" || block.Text == "buffers")
            {
                compressBufferindex = 0;
            }

            foreach (var child in block.Children)
            {
                var childIsPayload = child is RgbOutputBuffer || child is ExtendedBuffer_Input;
                // If the item in array is complex type, loop call the function to add it to tree.
                // compressBufferindex is used to record the rgbOutputBuffer or ExtendedBuffer_Input number here
                if (childIsPayload)
                {
                    compressBufferindex += 1;
                }

                // If the field name is Payload and its compressed, recalculating the offset and length, else directly loop call this function
                if (child.Text == "Payload" && IsCompressedXOR)
                {
                    var rpcHeader = (block as RgbOutputBuffer)?.RPCHEADEREXT ??
                        (block as ExtendedBuffer_Input)?.RPCHEADEREXT;
                    var childNode = AddBlock(child, blockRootOffset);
                    node.Nodes.Add(childNode);
                    if (childNode.Tag is Position nodePosition && nodePosition != null)
                    {
                        nodePosition.Offset = rpcHeader._Size;
                        childNode.Tag = nodePosition;
                    }
                    childNode.Text = "Payload(CompressedOrObfuscated)";
                    TreeNodeForCompressed(childNode, blockOffset + (int)rpcHeader.Size, compressBufferindex - 1);
                }
                else
                {
                    if (child.Text == "Payload")
                    {
                        // minus the Payload is not in compressed
                        compressBufferindex -= 1;
                    }

                    node.Nodes.Add(AddBlock(child, blockRootOffset));
                }
            }

            return node;
        }

        /// <summary>
        /// Modify the start index for the TreeNode which source data is compressed
        /// </summary>
        /// <param name="node">The node in compressed buffers</param>
        /// <param name="current">Indicates start position of the node</param>
        /// <param name="compressBufferindex">Indicates the index of this node in all compressed buffers in same session</param>
        /// <param name="isAux">Indicates whether the buffer which this node are in is auxiliary</param>
        /// <returns>The tree node with BufferIndex and IsCompressedXOR properties </returns>
        public static TreeNode TreeNodeForCompressed(TreeNode node, int current, int compressBufferindex, bool isAux = false)
        {
            foreach (TreeNode nd in node.Nodes)
            {
                if (nd.Tag is Position pos)
                {
                    if (DebugNodes)
                    {
                        nd.Nodes.Insert(0, new TreeNode($"Compressed: SI: {pos.StartIndex:X} SI`:{pos.StartIndex - current:X} C:{current:X} BI:{compressBufferindex:X}")
                        {
                            BackColor = System.Drawing.Color.AliceBlue,
                            Tag = "ignore"
                        });
                    }

                    pos.IsCompressedXOR = true;
                    pos.StartIndex -= current;
                    pos.BufferIndex = compressBufferindex;
                }

                if (nd.Nodes.Count != 0)
                {
                    TreeNodeForCompressed(nd, current, compressBufferindex, isAux);
                }
            }

            return node;
        }
    }
}