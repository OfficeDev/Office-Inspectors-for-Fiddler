using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.4.3.18 progressPerMessage Element
    /// The ProgressPerMessage element contains data that describes the approximate size of message change data that follows.
    /// </summary>
    public class ProgressPerMessage : Block
    {
        /// <summary>
        /// The start marker of ProgressPerMessage.
        /// </summary>
        public BlockT<Markers> StartMarker;

        /// <summary>
        /// A PropList value.
        /// </summary>
        public PropList PropList;

        /// <summary>
        /// Verify that a stream's current position contains a serialized ProgressPerMessage.
        /// </summary>
        /// <param name="parser">A BinaryParser.</param>
        /// <returns>If the stream's current position contains a serialized ProgressPerMessage, return true, else false.</returns>
        public static bool Verify(BinaryParser parser)
        {
            return MarkersHelper.VerifyMarker(parser, Markers.IncrSyncProgressPerMsg);
        }

        protected override void Parse()
        {
            StartMarker = ParseT<Markers>();
            if (StartMarker == Markers.IncrSyncProgressPerMsg)
            {
                PropList = Parse<PropList>();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("ProgressPerMessage");
            AddChildBlockT(StartMarker, "StartMarker");
            AddLabeledChild(PropList, "PropList");
        }
    }
}
