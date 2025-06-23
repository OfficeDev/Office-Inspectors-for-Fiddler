namespace MAPIInspector.Parsers
{
    using BlockParser;

    public static class BlockGuidExtensions
    {
        public static void AddChildGuid(this Block parent, BlockGuid child, string label)
        {
            if (child == null || !child.Parsed) return;
            parent.AddChild(child, $"{label}:{Guids.ToString(child.value.Data)}");
        }
    }
}