using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCFXICS] 2.2.4.3.25 state Element
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
            StartMarker = ParseT<Markers>();
            if (StartMarker == Markers.IncrSyncStateBegin)
            {
                PropList = Parse<PropList>();

                EndMarker = ParseT<Markers>();
                if (EndMarker != Markers.IncrSyncStateEnd)
                {
                    Parsed = false;
                }
            }
        }

        protected override void ParseBlocks()
        {
            Text = "State";
            AddChildBlockT(StartMarker, "StartMarker");
            AddLabeledChild(PropList, "PropList");
            AddChildBlockT(EndMarker, "EndMarker");
        }
    }
}
