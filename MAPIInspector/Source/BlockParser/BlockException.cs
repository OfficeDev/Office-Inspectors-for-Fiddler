using System.Drawing;

namespace BlockParser
{
    public class BlockException : Block
    {
        public static readonly Color BackColor = Color.LightPink;

        public static BlockException Create(string message, System.Exception ex, long offset)
        {
            var node = new BlockException();
            node.Text = message;
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

            node.Parsed = true; // Mark as parsed to avoid re-parsing
            return node;
        }

        protected override void Parse() { }
        protected override void ParseBlocks() { }
    }
}
