namespace BlockParser
{
    /// <summary>
    /// Routines for chaining blocks together to form a tree rooted at a BlockParser.Block instance.
    /// </summary>
    public partial class Block
    {
        /// <summary>
        /// Adds a child block to the current block.
        /// </summary>
        /// <remarks>The child block is added to the internal collection only if it is not <see
        /// langword="null"/> and has been successfully parsed.
        /// </remarks>
        /// <param name="child">The child block to add. Should not be <see langword="null"/> and should have its <see cref="Block.Parsed"/>
        /// property set to <see langword="true"/>.</param>
        public void AddChild(Block child)
        {
            if (child != null && child.Parsed)
            {
                children.Add(child);
            }
        }

        /// <summary>
        /// Adds a child block to the current block and sets its text using the specified format string and arguments.
        /// </summary>
        /// <remarks>The method adds the specified child block to the collection of children if the block
        /// is not <see langword="null"/>  and has been successfully parsed. The text of the child block is set using
        /// the provided format string and arguments.
        /// </remarks>
        /// <param name="child">The child block to add. Should not be <see langword="null"/> and should be parsed.</param>
        /// <param name="format">A composite format string used to set the text of the child block.</param>
        /// <param name="args">An array of objects to format into the <paramref name="format"/> string.</param>
        public void AddChild(Block child, string label)
        {
            if (child != null && child.Parsed)
            {
                child.SetText(label);
                children.Add(child);
            }
        }

        /// <summary>
        /// Adds a child block of type <see cref="BlockT{T}"/> to the current block with a specified label.
        /// </summary>
        /// <remarks>
        /// The child block's text is set to the label followed by its data value.
        /// </remarks>
        /// <example>If the label is "Value" and the data is 42, the text will be "Value:42".
        /// </example>
        /// <typeparam name="T">The value type of the data contained in the child block.</typeparam>
        /// <param name="child">The child block to add. Should not be <see langword="null"/> and should be parsed.</param>
        /// <param name="label">The label to use in the text of the child block.</param>
        public void AddChildBlockT<T>(BlockT<T> child, string label) where T : struct
        {
            if (child != null && child.Parsed)
            {
                child.SetText($"{label}:{child.Data}");
                children.Add(child);
            }
        }

        /// <summary>
        /// Adds a child string to the current block with a specified label.
        /// </summary>
        /// <remarks>The method updates the text of the <paramref name="child"/> by prepending the
        /// specified <paramref name="label"/> followed by a colon (":") to the child's data. The updated child is then
        /// added to the collection of children.
        /// </remarks>
        /// <example>
        /// For example, if the label is "Description" and the child's data is "This is a test",
        /// then the child's text will be set to "Description:This is a test".
        /// </example>
        /// <param name="child">The <see cref="BlockString"/> instance to add as a child. Should not be null and should have been parsed.</param>
        /// <param name="label">The label to prepend to the child's data when setting its text.</param>
        public void AddChildString(BlockString child, string label)
        {
            if (child != null && child.Parsed)
            {
                child.SetText($"{label}:{child.Data}");
                children.Add(child);
            }
        }

        /// <summary>
        /// Adds a header element with the specified text to the current object.
        /// </summary>
        /// <remarks>This method creates a new header element using the provided text and adds it as a
        /// child to the current object. Ensure that <paramref name="text"/> is a valid, non-empty string to avoid
        /// unexpected behavior.
        /// 
        /// This node will not have any size or offset, and it is typically used to represent a header in a structured format.
        /// </remarks>
        /// <param name="text">The text content of the header element. Should not be null or empty.</param>
        public void AddHeader(string text) => AddChild(Create(text));

        /// <summary>
        /// Adds a subheader node with formatted text to the current structure.
        /// </summary>
        /// <remarks>The subheader node inherits the current offset and size values and is added as a
        /// child to the current structure.
        /// </remarks>
        /// <param name="text">The text to set for the subheader node. This text will be used as the label for the node.</param>
        public void AddSubHeader(string text)
        {
            var node = Create();
            node.SetText(text);
            node.Offset = Offset;
            node.Size = Size;
            AddChild(node);
        }

        /// <summary>
        /// Adds a labeled child node to the current node based on the specified block and text.
        /// </summary>
        /// <remarks>This method creates a new node, sets its label text, and assigns the offset and size
        /// of the specified block. The block is then added as a child to the newly created node, which is subsequently
        /// added to the current node.
        /// </remarks>
        /// <param name="block">The block to be added as a child. Must not be null and must have been parsed.</param>
        /// <param name="text">The label text to associate with the child node.</param>
        public void AddLabeledChild(Block block, string text)
        {
            if (block != null && block.Parsed)
            {
                var node = Create();
                node.SetText(text);
                node.Offset = block.Offset;
                node.Size = block.Size;
                node.AddChild(block);
                AddChild(node);
            }
        }

        /// <summary>
        /// Adds a labeled child node to the current node, based on the provided blocks and text.
        /// </summary>
        /// <remarks>If <paramref name="blocks"/> is null, no action is performed. If <paramref
        /// name="blocks"/> is empty, the created node will not have any children. The size and offset of the newly
        /// created node are calculated based on the provided blocks.</remarks>
        /// <param name="blocks">An array of <see cref="Block"/> objects to be added as children. Each block will be labeled using its <see
        /// cref="Block.Text"/> property or its type name if <see cref="Block.Text"/> is null or empty.</param>
        /// <param name="text">The text to set for the newly created node that will contain the labeled children.</param>
        public void AddLabeledChildren(Block[] blocks, string text)
        {
            if (blocks != null)
            {
                var node = Create();
                node.SetText(text);

                if (blocks.Length > 0)
                {
                    long size = 0;
                    foreach (var block in blocks)
                    {
                        var label = string.IsNullOrEmpty(block.Text) ? block.GetType().Name : block.Text;
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
