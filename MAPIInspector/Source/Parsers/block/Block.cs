using System;
using System.Collections.Generic;
using System.Text;

namespace MAPIInspector.Parsers.block
{
    internal class Block
    {
        // Constants
        public const uint MaxBytes = 0xFFFF;
        public const uint MaxDepth = 25;
        public const uint MaxEID = 500;
        public const uint MaxEntriesSmall = 500;
        public const uint MaxEntriesLarge = 1000;
        public const uint MaxEntriesExtraLarge = 1500;
        public const uint MaxEntriesEnormous = 10000;

        // Fields
        protected BinaryParser parser;
        protected bool parsed = false;
        protected bool enableJunk = true;

        private ulong offset = 0;
        private ulong cb = 0;
        private uint source = 0;
        private string text = string.Empty;
        private readonly List<Block> children = new List<Block>();

        // Constructors
        public Block() { }

        // Getters and setters
        public string Text => text;
        public IReadOnlyList<Block> Children => children.AsReadOnly();
        public ulong Size => cb;
        public void SetSize(ulong size) => cb = size;
        public ulong Offset => offset;
        public void SetOffset(ulong value) => offset = value;
        public void ShiftOffset(ulong shift)
        {
            offset += shift;
            foreach (var child in children)
            {
                child.ShiftOffset(shift);
            }
        }

        public uint Source => source;
        public void SetSource(uint value)
        {
            source = value;
            foreach (var child in children)
            {
                child.SetSource(value);
            }
        }

        public bool IsSet => parsed;
        public bool IsHeader => cb == 0 && offset == 0;
        public bool HasData => !string.IsNullOrEmpty(text) || children.Count > 0;

        // Set text
        public void SetText(string value) => text = value;
        public void SetText(string format, params object[] args) => text = string.Format(format, args);

        // Add child blocks
        public void AddChild(Block child)
        {
            if (child != null && child.IsSet)
            {
                children.Add(child);
            }
        }

        public void AddChild(Block child, string value)
        {
            if (child != null && child.IsSet)
            {
                child.text = value;
                children.Add(child);
            }
        }

        public void AddChild(Block child, string format, params object[] args)
        {
            if (child != null && child.IsSet)
            {
                child.text = string.Format(format, args);
                children.Add(child);
            }
        }

        // Add a text only node with no size/offset and no children
        public void AddHeader(string value)
        {
            var header = new Block();
            header.text = value;
            children.Add(header);
        }
        public void AddHeader(string format, params object[] args)
        {
            AddHeader(string.Format(format, args));
        }

        // Add a text only node with size/offset matching the child node so that it "contains" the child
        public void AddLabeledChild(string value, Block block)
        {
            if (block != null)
            {
                var labeled = new Block();
                labeled.text = value;
                labeled.cb = block.cb;
                labeled.offset = block.offset;
                labeled.children.Add(block);
                children.Add(labeled);
            }
        }

        // Add a text only node with size/offset matching the parent node so that it matches the parent
        public void AddSubHeader(string value)
        {
            var subHeader = new Block();
            subHeader.text = value;
            subHeader.cb = cb;
            subHeader.offset = offset;
            children.Add(subHeader);
        }
        public void AddSubHeader(string format, params object[] args)
        {
            AddSubHeader(string.Format(format, args));
        }

        // Static create functions returns a non parsing block
        public static Block Create()
        {
            return new Block();
        }

        public static Block Create(ulong size, ulong offset, string format, params object[] args)
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

        // Static parse functions (generic)
        public static T Parse<T>(BinaryParser binaryParser, bool enableJunk) where T : Block, new()
        {
            return Parse<T>(binaryParser, 0, enableJunk);
        }

        public static T Parse<T>(BinaryParser binaryParser, ulong cbBin, bool enableJunk) where T : Block, new()
        {
            var ret = new T();
            ret.Parse(binaryParser, cbBin, enableJunk);
            return ret;
        }

        // Non-static parse functions
        public void Parse(BinaryParser binaryParser, bool enableJunk)
        {
            Parse(binaryParser, 0, enableJunk);
        }

        public void Parse(BinaryParser binaryParser, ulong cbBin, bool enableJunk)
        {
            parser = binaryParser;
            parser.SetCap(cbBin);
            this.enableJunk = enableJunk;
            EnsureParsed();
            parser.ClearCap();
        }

        // Protected
        protected virtual void EnsureParsed()
        {
            if (!parsed)
            {
                Parse();
                ParseBlocks();
                parsed = true;
            }
        }

        // ToString
        public override string ToString()
        {
            var sb = new StringBuilder();
            ToStringInternal(sb, 0);
            return sb.ToString();
        }

        private void ToStringInternal(StringBuilder sb, int depth)
        {
            sb.AppendLine(new string(' ', depth * 2) + text);
            foreach (var child in children)
            {
                child.ToStringInternal(sb, depth + 1);
            }
        }

        // Abstract/virtual methods
        protected virtual void Parse()
        {
            // To be implemented in derived classes
        }

        protected virtual void ParseBlocks()
        {
            // Optional override
        }

        protected virtual bool UsePipes() => false;
    }
}
