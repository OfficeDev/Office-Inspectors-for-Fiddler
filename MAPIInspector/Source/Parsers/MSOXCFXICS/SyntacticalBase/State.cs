namespace MAPIInspector.Parsers
{
    using BlockParser;

    /// <summary>
    /// The state element contains the final ICS state of the synchronization download operation. 
    /// </summary>
    public class State : Block
    {
        /// <summary>
        /// The start marker of ReadStateChange.
        /// </summary>
        public BlockT<Markers> StartMarker;

        /// <summary>
        /// A PropList value.
        /// </summary>
        public PropList PropList;

        /// <summary>
        /// The end marker of ReadStateChange.
        /// </summary>
        public BlockT<Markers> EndMarker;

        protected override void Parse()
        {
            StartMarker = BlockT<Markers>(parser);
            if (StartMarker.Data == Markers.IncrSyncStateBegin)
            {
                PropList = Parse<PropList>(parser);

                EndMarker = BlockT<Markers>(parser);
                if (EndMarker.Data != Markers.IncrSyncStateEnd)
                {
                    Parsed = false;
                }
            }
        }

        protected override void ParseBlocks()
        {
            SetText("State");
            if (StartMarker != null) AddChild(StartMarker, $"StartMarker:{StartMarker.Data}");
            AddLabeledChild(PropList, "PropList");
            if (EndMarker != null) if (EndMarker != null) AddChild(EndMarker, $"EndMarker:{EndMarker.Data}");
        }
    }
}
