using System.Collections.Generic;
using System.IO;

namespace BlockParser
{
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

    public abstract class Block
    {
        protected BinaryParser parser;
        protected bool Parsed { get; set; } = false;
        protected bool EnableJunk { get; set; } = true;
        public long Size { get; set; }
        public long Offset { get; set; }
        protected virtual bool UsePipes() => false;
        public string Text { get; protected set; } = string.Empty;

        protected Block() { }

        // Overrides
        /// <summary>
        /// When implemented in a derived class, parses the current block from the associated <see cref="BinaryParser"/>.
        /// This method should set up the block's data and state based on the binary input.
        /// </summary>
        protected abstract void Parse();
        /// <summary>
        /// When overridden in a derived class, parses and adds any child blocks to this block.
        /// The default implementation does nothing. Override to add custom child block parsing logic.
        /// </summary>
        protected virtual void ParseBlocks() { }

        public void SetText(string format, params object[] args)
        {
            Text = !string.IsNullOrEmpty(format) ? string.Format(format, args) : string.Empty;
        }

        private List<Block> _children { get; set; } = new List<Block>();
        public IReadOnlyList<Block> Children => _children.AsReadOnly();

        public void ShiftOffset(long shift)
        {
            Offset += shift;
            foreach (var child in Children)
            {
                child.ShiftOffset(shift);
            }
        }

        private uint _source;
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

        public bool IsHeader => Size == 0 && Offset == 0;
        public bool HasData => !string.IsNullOrEmpty(Text) || Children.Count > 0;

        // Add child blocks of various types
        public void AddChild(Block child)
        {
            if (child != null && child.Parsed)
            {
                _children.Add(child);
            }
        }

        public void AddChild(Block child, string text)
        {
            if (child != null && child.Parsed)
            {
                child.Text = text ?? string.Empty;
                _children.Add(child);
            }
        }

        public void AddChild(Block child, string format, params object[] args)
        {
            if (child != null && child.Parsed)
            {
                child.Text = string.Format(format, args);
                _children.Add(child);
            }
        }

        // Add a text only node with no size/offset and no children
        public void AddHeader(string text) => AddChild(Create(text));

        public void AddHeader(string format, params object[] args) => AddHeader(string.Format(format, args));

        // Add a text only node with size/offset matching the child node so that it "contains" the child
        public void AddLabeledChild(string text, Block _block)
        {
            if (_block != null && _block.Parsed)
            {
                var node = Create();
                node.SetText(text);
                node.Offset = _block.Offset;
                node.Size = _block.Size;
                node.AddChild(_block);
                AddChild(node);
            }
        }

        // Add a text only node with size/offset matching the parent node so that it matches the parent
        public void AddSubHeader(string text)
        {
            var node = Create();
            node.SetText(text);
            node.Offset = Offset;
            node.Size = Size;
            AddChild(node);
        }

        public void AddSubHeader(string format, params object[] args) => AddSubHeader(string.Format(format, args));

        // Static create functions returns a non parsing block
        public static Block Create() => new ScratchBlock();

        public static Block Create(long size, long offset, string format, params object[] args)
        {
            var ret = Create();
            ret.Size = size;
            ret.Offset = offset;
            ret.SetText(format, args);
            return ret;
        }

        public static Block Create(string format, params object[] args)
        {
            var ret = Create();
            ret.SetText(format, args);
            return ret;
        }

        // Static parse function returns a parsing block based on a stream at it's current position
        // Advance the stream by the size of the block after parsing
        public static T Parse<T>(Stream stream, bool enableJunk = false) where T : Block, new()
        {
            var block = Parse<T>(new BinaryParser(stream, stream.Position), enableJunk);
            block.parser.Advance((int)stream.Position);
            stream.Seek(block.Size, SeekOrigin.Current);
            return block;
        }

        // Static parse function returns a parsing block based on a BinaryParser
        public static T Parse<T>(BinaryParser parser, bool enableJunk = false) where T : Block, new()
        {
            return Parse<T>(parser, 0, enableJunk);
        }

        // TODO: If we don't start using this while converting, remove it
        public static T Parse<T>(BinaryParser parser, int cbBin, bool enableJunk = false) where T : Block, new()
        {
            var ret = new T();
            ret.Parse(parser, cbBin, enableJunk);
            return ret;
        }

        // Non-static parse functions actually do the parsing
        public void Parse(BinaryParser parser, bool enableJunk = false) => Parse(parser, 0, enableJunk);

        public void Parse(BinaryParser parser, int cbBin, bool enableJunk = false)
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
                    var junkData = BlockBytes.Parse(parser, parser.RemainingBytes);
                    AddLabeledChild(string.Format("Unparsed data size = 0x{0:X8}", junkData.Size), junkData);
                }

                Size = parser.Offset - Offset;
            }

            var stringArray = ToStringsInternal();
            _stringBlock = Strings.TrimWhitespace(string.Join(string.Empty, stringArray));
            _stringBlock = _stringBlock.Replace('\0', '.');
        }

        private string _stringBlock;
        public override string ToString()
        {
            EnsureParsed();
            return _stringBlock;
        }

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

        public byte[] PeekBytes => parser.PeekBytes;
    }
}
