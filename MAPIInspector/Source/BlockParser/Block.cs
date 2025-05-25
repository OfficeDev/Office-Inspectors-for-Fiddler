using System.Collections.Generic;
using System.Xml;

namespace Parser
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
        protected Block() { }

        // Getters and setters
        public string Text { get; protected set; } = string.Empty;

        public virtual string ToStringBlock()
        {
            EnsureParsed();
            var stringArray = ToStringsInternal();
            var parsedString = strings.TrimWhitespace(strings.Join(stringArray, strings.EmptyString));
            parsedString = parsedString.Replace('\0', '.');
            return parsedString;
        }

        public void SetText(string format, params object[] args)
        {
            Text = !string.IsNullOrEmpty(format) ? string.Format(format, args) : string.Empty;
        }

        public IReadOnlyList<Block> Children => children.AsReadOnly();

        public long Size { get; protected set; }
        public void SetSize(long size) => Size = size;

        public long Offset { get; protected set; }
        public void SetOffset(long offset) => Offset = offset;

        public void ShiftOffset(long shift)
        {
            Offset += shift;
            foreach (var child in children)
            {
                child.ShiftOffset(shift);
            }
        }

        public uint Source { get; protected set; }
        public void SetSource(uint source)
        {
            Source = source;
            foreach (var child in children)
            {
                child.SetSource(source);
            }
        }

        public bool IsSet => parsed;
        public bool IsHeader => Size == 0 && Offset == 0;
        public bool HasData => !string.IsNullOrEmpty(Text) || children.Count > 0;

        // Add child blocks of various types
        public void AddChild(Block child)
        {
            if (child != null && child.IsSet)
            {
                children.Add(child);
            }
        }

        public void AddChild(Block child, string text)
        {
            if (child != null && child.IsSet)
            {
                child.Text = text ?? string.Empty;
                children.Add(child);
            }
        }

        public void AddChild(Block child, string format, params object[] args)
        {
            if (child != null && child.IsSet)
            {
                child.Text = string.Format(format, args);
                children.Add(child);
            }
        }

        // Add a text only node with no size/offset and no children
        public void AddHeader(string text)
        {
            AddChild(Create(text));
        }

        public void AddHeader(string format, params object[] args)
        {
            AddHeader(string.Format(format, args));
        }

        // Add a text only node with size/offset matching the child node so that it "contains" the child
        public void AddLabeledChild(string text, Block _block)
        {
            if (_block != null && _block.IsSet)
            {
                var node = Create();
                node.SetText(text);
                node.SetOffset(_block.Offset);
                node.SetSize(_block.Size);
                node.AddChild(_block);
                AddChild(node);
            }
        }

        // Add a text only node with size/offset matching the parent node so that it matches the parent
        public void AddSubHeader(string text)
        {
            var node = Create();
            node.SetText(text);
            node.SetOffset(Offset);
            node.SetSize(Size);
            AddChild(node);
        }

        public void AddSubHeader(string format, params object[] args)
        {
            AddSubHeader(string.Format(format, args));
        }

        // Static create functions returns a non parsing block
        public static Block Create()
        {
            return new ScratchBlock();
        }

        public static Block Create(long size, long offset, string format, params object[] args)
        {
            var ret = Create();
            ret.SetSize(size);
            ret.SetOffset(offset);
            ret.SetText(format, args);
            return ret;
        }

        public static Block Create(string format, params object[] args)
        {
            var ret = Create();
            ret.SetText(format, args);
            return ret;
        }

        // Static parse functions return a parsing block based on a BinaryParser
        public static T Parse<T>(BinaryParser parser, bool enableJunk) where T : Block, new()
        {
            return Parse<T>(parser, 0, enableJunk);
        }

        public static T Parse<T>(BinaryParser parser, int cbBin, bool enableJunk) where T : Block, new()
        {
            var ret = new T();
            ret.Parse(parser, cbBin, enableJunk);
            return ret;
        }

        // Non-static parse functions actually do the parsing
        public void Parse(BinaryParser parser, bool enableJunk)
        {
            Parse(parser, 0, enableJunk);
        }

        public void Parse(BinaryParser parser, int cbBin, bool enableJunk)
        {
            this.parser = parser;
            parser.SetCap(cbBin);
            this.enableJunk = enableJunk;
            EnsureParsed();
            parser.ClearCap();
        }

        protected void EnsureParsed()
        {
            if (parsed || parser == null || parser.Empty) return;
            parsed = true; // parse can unset this if needed
            SetOffset(parser.Offset);

            Parse();
            ParseBlocks();


            if (HasData && enableJunk && parser.GetSize() > 0)
            {
                var junkData = BlockBytes.Parse(parser, parser.GetSize());
                AddLabeledChild(string.Format("Unparsed data size = 0x{0:X8}", junkData.Size), junkData);
            }

            SetSize(parser.Offset - Offset);
        }

        protected abstract void Parse();
        protected virtual void ParseBlocks() { }
        protected virtual bool UsePipes() => false;

        protected long OffsetInternal => Offset;
        protected long SizeInternal => Size;

        protected BinaryParser parser;
        protected bool parsed = false;
        protected bool enableJunk = true;

        private List<Block> children = new List<Block>();

        private List<string> ToStringsInternal()
        {
            var strings = new List<string>(children.Count + 1);
            if (!string.IsNullOrEmpty(Text)) strings.Add(Text + "\r\n");

            foreach (var child in children)
            {
                var childStrings = child.ToStringsInternal();
                if (!string.IsNullOrEmpty(Text)) childStrings = TabStrings(childStrings, UsePipes());
                strings.AddRange(childStrings);
            }

            return strings;
        }

        private static List<string> TabStrings(List<string> elems, bool usePipes)
        {
            if (elems == null || elems.Count == 0) return new List<string>();
            var strings = new List<string>(elems.Count);
            foreach (var elem in elems)
            {
                if (usePipes)
                {
                    strings.Add("|\t" + elem);
                }
                else
                {
                    strings.Add("\t" + elem);
                }
            }
            return strings;
        }
    }
}
