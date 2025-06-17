namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.Collections.Generic;

    /// <summary>
    /// The MessageChangePartial element represents the difference in message content since the last download, as identified by the initial ICS state.
    /// </summary>
    public class MessageChangePartial : Block
    {
        /// <summary>
        /// A groupInfo value.
        /// </summary>
        public GroupInfo GroupInfo;

        /// <summary>
        /// A MetaTagIncrSyncGroupId property.
        /// </summary>
        public MetaPropValue MetaTagIncrSyncGroupId;

        /// <summary>
        /// The MessageChangePartial marker.
        /// </summary>
        public BlockT<Markers> Marker;

        /// <summary>
        /// A MessageChangeHeader value.
        /// </summary>
        public PropList MessageChangeHeader;

        /// <summary>
        /// A list of SyncMessagePartialPropList values.
        /// </summary>
        public SyncMessagePartialPropList[] SyncMessagePartialPropList;

        /// <summary>
        /// A MessageChildren field.
        /// </summary>
        public MessageChildren MessageChildren;

        /// <summary>
        /// Verify that a stream's current position contains a serialized MessageChangePartial.
        /// </summary>
        /// <param name="parser">A BinaryParser.</param>
        /// <returns>If the stream's current position contains a serialized MessageChangePartial, return true, else false.</returns>
        public static bool Verify(BinaryParser parser)
        {
            return GroupInfo.Verify(parser);
        }

        protected override void Parse()
        {
            List<SyncMessagePartialPropList> interMessagePartialList = new List<SyncMessagePartialPropList>();
            GroupInfo = Parse<GroupInfo>(parser);

            if (MarkersHelper.VerifyMetaProperty(parser, MetaProperties.MetaTagIncrSyncGroupId))
            {
                MetaTagIncrSyncGroupId = Parse<MetaPropValue>(parser);
            }

            Marker = BlockT<Markers>.Parse(parser);
            if (Marker.Data == Markers.IncrSyncChgPartial)
            {
                MessageChangeHeader = Parse<PropList>(parser);

                while (MarkersHelper.VerifyMetaProperty(parser, MetaProperties.MetaTagIncrementalSyncMessagePartial))
                {
                    interMessagePartialList.Add(Parse<SyncMessagePartialPropList>(parser));
                }

                SyncMessagePartialPropList = interMessagePartialList.ToArray();
                MessageChildren = Parse<MessageChildren>(parser);
            }
            else
            {
                Parsed = false;
            }
        }

        protected override void ParseBlocks()
        {
            SetText("MessageChangePartial");
            AddChild(GroupInfo, "GroupInfo");
            AddChild(MetaTagIncrSyncGroupId, "MetaTagIncrSyncGroupId");
            if (Marker != null) AddChild(Marker, $"Marker:{Marker.Data}");
            AddChild(MessageChangeHeader, "MessageChangeHeader");
            if (SyncMessagePartialPropList != null)
            {
                AddLabeledChildren("SyncMessagePartialPropList", SyncMessagePartialPropList);
            }
            AddChild(MessageChildren, "MessageChildren");
        }
    }
}
