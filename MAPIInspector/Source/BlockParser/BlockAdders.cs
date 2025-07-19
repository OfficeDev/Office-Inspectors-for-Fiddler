namespace BlockParser
{
    /// <summary>
    /// Provides methods for adding and labeling child blocks to form a tree structure rooted at a <see cref="Block"/> instance.
    /// </summary>
    public partial class Block
    {
        /// <summary>
        /// Adds a child block to this block.
        /// </summary>
        /// <remarks>
        /// The child is added only if it is not <c>null</c> and has been successfully parsed (<see cref="Parsed"/> is <c>true</c>).
        /// </remarks>
        /// <param name="child">The child block to add. Must not be <c>null</c> and must be parsed.</param>
        public void AddChild(Block child)
        {
            if (child != null && child.Parsed)
            {
                children.Add(child);
            }
        }

        /// <summary>
        /// Adds a child block to this block and sets its text label.
        /// </summary>
        /// <remarks>
        /// The child is added only if it is not <c>null</c> and has been successfully parsed. The child's text is set to the provided label.
        /// </remarks>
        /// <param name="child">The child block to add. Must not be <c>null</c> and must be parsed.</param>
        /// <param name="label">The label to set as the child's text.</param>
        public void AddChild(Block child, string label)
        {
            if (child != null && child.Parsed)
            {
                child.Text = label;
                children.Add(child);
            }
        }

        /// <summary>
        /// Adds a child block of type <see cref="BlockT{T}"/> to this block with a specified label.
        /// </summary>
        /// <remarks>
        /// The child's text is set to the label followed by its data value (e.g., "Label: Value").
        /// </remarks>
        /// <typeparam name="T">The value type of the data contained in the child block.</typeparam>
        /// <param name="child">The child block to add. Must not be <c>null</c> and must be parsed.</param>
        /// <param name="label">The label to use in the child's text.</param>
        public void AddChildBlockT<T>(BlockT<T> child, string label) where T : struct
        {
            if (child != null && child.Parsed)
            {
                child.Text = $"{label}: {child}";
                children.Add(child);
            }
        }

        /// <summary>
        /// Adds a <see cref="BlockString"/> child to this block with a specified label.
        /// </summary>
        /// <remarks>
        /// The child's text is set to the label followed by its data (e.g., "Label: Data").
        /// </remarks>
        /// <param name="child">The <see cref="BlockString"/> to add. Must not be <c>null</c> and must be parsed.</param>
        /// <param name="label">The label to prepend to the child's data in its text.</param>
        public void AddChildString(BlockString child, string label)
        {
            if (child != null && child.Parsed)
            {
                child.Text = $"{label}: {child.Data}";
                children.Add(child);
            }
        }

        /// <summary>
        /// Adds a <see cref="BlockBytes"/> child to this block with a specified label.
        /// </summary>
        /// <remarks>
        /// The child's text is set to the label followed by its data in hex format (e.g., "Label: 01020304").
        /// </remarks>
        /// <param name="child">The <see cref="BlockBytes"/> to add. Must not be <c>null</c> and must be parsed.</param>
        /// <param name="label">The label to prepend to the child's data in its text.</param>
        public void AddChildBytes(BlockBytes child, string label)
        {
            if (child != null && child.Parsed)
            {
                child.Text = $"{label}: {child.ToHexString()}";
                children.Add(child);
            }
        }

        /// <summary>
        /// Adds a header node with the specified text to this block.
        /// </summary>
        /// <remarks>
        /// The header node is created with the given text and added as a child. The node has no size or offset.
        /// </remarks>
        /// <param name="header">The subheader text to set as the node's label.</param>
        public void AddHeader(string header) => AddChild(Create(header));

        /// <summary>
        /// Inserts a header node with the specified text to this block.
        /// Node is added as first child of this block.
        /// </summary>
        /// <remarks>
        /// The header node is created with the given text and added as a child. The node has no size or offset.
        /// </remarks>
        /// <param name="header">The subheader text to set as the node's label.</param>
        public void InsertHeader(string header) => children.Insert(0, Create(header));

        /// <summary>
        /// Adds a subheader node with the specified label to this block.
        /// </summary>
        /// <remarks>
        /// The subheader node inherits this block's offset and size, and is added as a child.
        /// </remarks>
        /// <param name="header">The subheader text to set as the node's label.</param>
        public void AddSubHeader(string header)
        {
            var node = Create();
            node.Text = header;
            node.Offset = Offset;
            node.Size = Size;
            AddChild(node);
        }

        /// <summary>
        /// Adds a labeled child node to this block, containing the specified block as its child.
        /// </summary>
        /// <remarks>
        /// A new node is created with the given label, and its offset and size are set to match the provided block.
        /// The block is added as a child to the new node, which is then added to this block.
        /// </remarks>
        /// <param name="block">The block to add as a child. Must not be <c>null</c> and must be parsed.</param>
        /// <param name="text">The label text for the new node.</param>
        public void AddLabeledChild(Block block, string text)
        {
            if (block != null && block.Parsed)
            {
                var node = Create();
                node.Text = text;
                node.Offset = block.Offset;
                node.Size = block.Size;
                node.AddChild(block);
                AddChild(node);
            }
        }

        /// <summary>
        /// Adds a labeled node to this block, containing the specified blocks as its children.
        /// </summary>
        /// <remarks>
        /// If <paramref name="blocks"/> is <c>null</c>, no action is taken. If it is empty, the created node will have no children.
        /// The node's offset and size are set based on the provided blocks.
        /// Each child block is labeled using its <see cref="Text"/> property, or its type name if <see cref="Text"/> is <c>null</c> or empty.
        /// </remarks>
        /// <param name="blocks">The blocks to add as children. Each must be parsed.</param>
        /// <param name="text">The label for the new node.</param>
        public void AddLabeledChildren(Block[] blocks, string text)
        {
            if (blocks != null)
            {
                var node = Create();
                node.Text = text;

                if (blocks.Length > 0)
                {
                    long size = 0;
                    foreach (var block in blocks)
                    {
                        string label;
                        if (block is BlockString blockString)
                        {
                            label = $"\"{blockString.Data}\"";
                        }
                        else if (block is IBlockT)
                        {
                            label = block.ToString();
                        }
                        else if (string.IsNullOrEmpty(block.Text))
                        {
                            label = block.GetType().Name;
                        }
                        else
                        {
                            label = block.ToString();
                        }

                        node.AddChild(block, label);
                        size += block.Size;
                    }

                    node.Offset = blocks[0].Offset;
                    node.Size = size;
                }

                AddChild(node);
            }
        }
    }
}
