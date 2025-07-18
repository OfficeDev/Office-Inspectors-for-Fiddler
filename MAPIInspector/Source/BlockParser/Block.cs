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
        private uint _source;

        // Overrides
        /// <summary>
        /// When implemented in a derived class, parses the current block from the associated <see cref="BinaryParser"/>.
        /// This method should set up the block's data and state based on the binary input.
        /// </summary>
        protected abstract void Parse();
        /// <summary>
        /// When overridden in a derived class, parses and adds any child blocks to this block, building a tree of blocks
        /// No default implementation is provided, as this method is expected to be specific to the derived class's structure.
        /// Do NOT attempt to parse data from the stream or parser here; that should be done in the <see cref="Parse"/> method.
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
            EnableJunk = enableJunk;
            EnsureParsed();
            parser.PopCap();
        }

        protected void EnsureParsed()
        {
            if (!Parsed && parser != null && !parser.Empty)
            {
                Parsed = true; // parse can unset this if needed
                Offset = parser.Offset;

                // Parse and ParseBlocks are seperate so that if one throws an exception, the other can still
                // run and we can still get the tree layout, even if the data parsing failed.
                try
                {
                    Parse();
                }
                catch (System.Exception e)
                {
                    var typeName = e.GetType().FullName;
                    if (typeName == "MAPIInspector.Parsers.MissingInformationException") throw e;
                    if (typeName == "MAPIInspector.Parsers.MissingPartialInformationException") throw e;
                    children.Add(BlockException.Create("Buffer Parsing Exception", e, Offset));
                }

                // Compute Size so ParseBlocks can use it
                Size = parser.Offset - Offset;

                try
                {
                    ParseBlocks();
                }
                catch (System.Exception e)
                {
                    children.Add(BlockException.Create("Tree Layout Exception", e, Offset));
                }

                if (HasData && EnableJunk && parser.RemainingBytes > 0)
                {
                    AddChild(ParseJunk("Unparsed data"));
                }

                // Recompute Size just in case ParseBlocks changed it
                Size = parser.Offset - Offset;
            }
        }

        // This is purely for testing now
        public string FullString()
        {
            EnsureParsed();
            var stringArray = ToStringsInternal();
            var _stringBlock = Strings.TrimWhitespace(string.Join(string.Empty, stringArray));
            _stringBlock = _stringBlock.Replace('\0', '.');
            return _stringBlock;
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
    }
}
