using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The progressTotal element contains data that describes the approximate size of all the messageChange elements.
    /// </summary>
    public class ProgressTotal : Block
    {
        /// <summary>
        /// The start marker of progressTotal.
        /// </summary>
        public BlockT<Markers> StartMarker;

        /// <summary>
        /// The propertyTag for ProgressInformation.
        /// </summary>
        public BlockT<uint> PropertiesTag;

        /// <summary>
        /// The count of the PropList.
        /// </summary>
        public BlockT<uint> PropertiesLength;

        /// <summary>
        /// A PropList value.
        /// </summary>
        public ProgressInformation PropList;

        /// <summary>
        /// Verify that a stream's current position contains a serialized progressTotal.
        /// </summary>
        /// <param name="parser">A BinaryParser.</param>
        /// <returns>If the stream's current position contains a serialized progressTotal, return true, else false.</returns>
        public static bool Verify(BinaryParser parser)
        {
            return MarkersHelper.VerifyMarker(parser, Markers.IncrSyncProgressMode);
        }

        protected override void Parse()
        {
            StartMarker = BlockT<Markers>(parser);
            if (StartMarker.Data == Markers.IncrSyncProgressMode)
            {
                PropertiesTag = BlockT<uint>(parser);
                PropertiesLength = BlockT<uint>(parser);
                PropList = Parse<ProgressInformation>(parser);
            }
        }

        protected override void ParseBlocks()
        {
            SetText("ProgressTotal");
            if (StartMarker != null) AddChild(StartMarker, $"StartMarker:{StartMarker.Data}");
            if (PropertiesTag != null) AddChild(PropertiesTag, $"PropertiesTag:{PropertiesTag.Data}");
            if (PropertiesLength != null) AddChild(PropertiesLength, $"PropertiesLength:{PropertiesLength.Data}");
            if (PropList != null) AddChild(PropList, "PropList:");
        }
    }
}
