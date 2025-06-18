using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The GroupInfo element provides a definition for the property group mapping.
    /// </summary>
    public class GroupInfo : Block
    {
        /// <summary>
        /// The start marker of GroupInfo.
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
        public PropertyGroupInfo PropList;

        /// <summary>
        /// Verify that a stream's current position contains a serialized GroupInfo.
        /// </summary>
        /// <param name="parser">A BinaryParser.</param>
        /// <returns>If the stream's current position contains a serialized GroupInfo, return true, else false.</returns>
        public static bool Verify(BinaryParser parser)
        {
            return MarkersHelper.VerifyMarker(parser, Markers.IncrSyncGroupInfo);
        }

        protected override void Parse()
        {
            StartMarker = new BlockT<Markers>(parser);
            if (StartMarker.Data == Markers.IncrSyncGroupInfo)
            {
                PropertiesTag = ParseT<uint>(parser);
                PropertiesLength = ParseT<uint>(parser);
                PropList = Parse<PropertyGroupInfo>(parser);
            }
        }

        protected override void ParseBlocks()
        {
            SetText("GroupInfo");
            if (StartMarker != null) AddChild(StartMarker, $"StartMarker:{StartMarker.Data}");
            if (PropertiesTag != null) AddChild(PropertiesTag, $"PropertiesTag:0x{PropertiesTag.Data:X8}");
            if (PropertiesLength != null) AddChild(PropertiesLength, $"PropertiesLength:{PropertiesLength.Data}");
            if (PropList != null) AddChild(PropList);
        }
    }
}
