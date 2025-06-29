using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.4.3.8 groupInfo Element
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
            StartMarker = ParseT<Markers>();
            if (StartMarker == Markers.IncrSyncGroupInfo)
            {
                PropertiesTag = ParseT<uint>();
                PropertiesLength = ParseT<uint>();
                PropList = Parse<PropertyGroupInfo>();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("GroupInfo");
            AddChildBlockT(StartMarker, "StartMarker");
            if (PropertiesTag != null) AddChild(PropertiesTag, $"PropertiesTag:0x{PropertiesTag.Data:X8}");
            AddChildBlockT(PropertiesLength, "PropertiesLength");
            if (PropList != null) AddChild(PropList);
        }
    }
}
