using System.Collections.Generic;
using System.IO;

namespace BlockParser
{
    // TODO: Make use of these
    public static class Constants
    {
        public const uint _MaxBytes = 0xFFFF;
        public const uint _MaxDepth = 25;
        public const uint _MaxEID = 500;
        public const uint _MaxEntriesSmall = 500;
        public const uint _MaxEntriesLarge = 1000;
        public const uint _MaxEntriesExtraLarge = 1500;
        public const uint _MaxEntriesEnormous = 10000;
    }

    public abstract partial class Block
    {
        public long Size { get; set; }
        public long Offset { get; set; }
        public string Text { get; protected set; } = string.Empty;
        public uint Source
        {
            get => _source;
            set
            {
                _source = value;
                foreach (var child in Children)
                {
                    child.Source = value;
                }
            }
        }

        public IReadOnlyList<Block> Children => children.AsReadOnly();
        public bool IsHeader => Size == 0 && Offset == 0;
        public bool HasData => !string.IsNullOrEmpty(Text) || Children.Count > 0;

        protected BinaryParser parser;
        public bool Parsed { get; protected set; } = false;
        protected bool EnableJunk { get; set; } = true;
        protected virtual bool UsePipes() => false;

        private List<Block> children { get; } = new List<Block>();
        private string _stringBlock;
        private uint _source;

        // Overrides
        /// <summary>
        /// When implemented in a derived class, parses the current block from the associated <see cref="BinaryParser"/>.
        /// This method should set up the block's data and state based on the binary input.
        /// </summary>
        protected abstract void Parse();
        /// <summary>
        /// When overridden in a derived class, parses and adds any child blocks to this block.
        /// No default implementation is provided, as this method is expected to be specific to the derived class's structure.
        /// </summary>
        protected abstract void ParseBlocks();

        public void SetText(string format, params object[] args)
        {
            if (!string.IsNullOrEmpty(format))
            {
                if (args.Length > 0)
                {
                    Text = string.Format(format, args);
                }
                else
                {
                    Text = format;
                }
            }
            else
            {
                Text = string.Empty;
            }
        }

        public void ShiftOffset(long shift)
        {
            Offset += shift;
            foreach (var child in Children)
            {
                child.ShiftOffset(shift);
            }
        }

        // Add child blocks of various types
        public void AddChild(Block child)
        {
            if (child != null && child.Parsed)
            {
                children.Add(child);
            }
        }

        public void AddChild(Block child, string format, params object[] args)
        {
            if (child != null && child.Parsed)
            {
                child.SetText(format, args);
                children.Add(child);
            }
        }

        public void AddChildBlockT<T>(BlockT<T> child, string label) where T : struct
        {
            if (child != null && child.Parsed)
            {
                child.SetText($"{label}:{child.Data}");
                children.Add(child);
            }
        }

        public void AddChildString(BlockString child, string label)
        {
            if (child != null && child.Parsed)
            {
                child.SetText($"{label}:{child.Data}");
                children.Add(child);
            }
        }

        // Add a text only node with no size/offset and no children
        public void AddHeader(string text) => AddChild(Create(text));

        public void AddHeader(string format, params object[] args) => AddHeader(string.Format(format, args));

        // Add a text only node with size/offset matching the child node so that it "contains" the child
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

        // Add a text only node with size/offset matching the children node so that it "contains" the children
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

        // Add a text only node with size/offset matching the parent node so that it matches the parent
        public void AddSubHeader(string format, params object[] args)
        {
            var node = Create();
            node.SetText(format, args);
            node.Offset = Offset;
            node.Size = Size;
            AddChild(node);
        }

        public void Parse(Stream stream, bool enableJunk = false) => Parse(stream, 0, enableJunk);
        private void Parse(Stream stream, int cbBin, bool enableJunk = false)
        {
            var parser = new BinaryParser(stream, stream.Position);
            Parse(parser, 0, enableJunk);
            stream.Seek(Size, SeekOrigin.Current);
        }

        public void Parse(BinaryParser parser, bool enableJunk = false) => Parse(parser, 0, enableJunk);

        private void Parse(BinaryParser parser, int cbBin, bool enableJunk = false)
        {
            this.parser = parser;
            parser.PushCap(cbBin);
            this.EnableJunk = enableJunk;
            EnsureParsed();
            parser.PopCap();
        }

        protected void EnsureParsed()
        {
            if (!Parsed && parser != null && !parser.Empty)
            {
                Parsed = true; // parse can unset this if needed
                Offset = parser.Offset;

                Parse();
                ParseBlocks();

                if (HasData && EnableJunk && parser.RemainingBytes > 0)
                {
                    var junkData = ParseBytes(parser.RemainingBytes);
                    AddLabeledChild(junkData, string.Format("Unparsed data"));
                }

                Size = parser.Offset - Offset;
            }

            var stringArray = ToStringsInternal();
            _stringBlock = Strings.TrimWhitespace(string.Join(string.Empty, stringArray));
            _stringBlock = _stringBlock.Replace('\0', '.');
        }

        public string FullString
        {
            get
            {
                EnsureParsed();
                return _stringBlock;
            }
        }

        public override string ToString() => Text;

        private List<string> ToStringsInternal()
        {
            var strings = new List<string>(Children.Count + 1);
            if (!string.IsNullOrEmpty(Text)) strings.Add(Text + "\r\n");

            foreach (var child in Children)
            {
                var childStrings = child.ToStringsInternal();
                if (!string.IsNullOrEmpty(Text)) childStrings = Strings.TabStrings(childStrings, UsePipes());
                strings.AddRange(childStrings);
            }

            return strings;
        }

        // Only used for debugging purposes, returns the entire binary stream as a byte array
        public byte[] PeekBytes => parser.PeekBytes;
    }
}
