namespace BlockParser
{
    public abstract class BlockString : Block
    {
        internal string data = string.Empty;
        internal int cchChar = -1;

        public static implicit operator string(BlockString block) => block.data;

        public string Data => data;
        public int Length => data.Length;
        public bool Empty => string.IsNullOrEmpty(data);
    }
}
