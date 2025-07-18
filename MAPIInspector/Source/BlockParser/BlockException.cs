namespace BlockParser
{
    public class BlockException : Block
    {
        public static BlockException Create(string message, System.Exception ex, long offset)
        {
            var node = new BlockException();
            node.SetText(message);
            node.Offset = offset;
            node.Size = 1; // We just want to signal where the exception occurred, so size is minimal
            if (ex != null)
            {
                node.AddHeader($"{ex.Message} at offset {offset}");
                var exType = Create($"Exception Type: {ex.GetType()}");
                node.AddChild(exType);
                if (ex.StackTrace != null)
                {
                    var stackLines = ex.StackTrace.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in stackLines)
                    {
                        exType.AddHeader(line.Trim());
                    }
                }
            }

            return node;
        }

        protected override void Parse() { }
        protected override void ParseBlocks() { }
    }
}
