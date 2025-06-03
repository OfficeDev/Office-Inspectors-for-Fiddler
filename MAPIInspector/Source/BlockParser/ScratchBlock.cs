namespace BlockParser
{
	// A non-parsing block used as a placeholder or for text-only nodes
	public class ScratchBlock : Block
	{
		public ScratchBlock()
		{
			Parsed = true;
		}

		protected override void Parse()
		{
			// No parsing logic for ScratchBlock
		}
	}
}