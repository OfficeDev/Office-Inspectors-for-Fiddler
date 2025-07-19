using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.4.3.13 messageChangeFull Element
    /// The messageChangeFull element contains the complete content of a new or changed message: the message properties, the recipients,and the attachments.
    /// </summary>
    public class MessageChangeFull : Block
    {
        /// <summary>
        /// A start marker for MessageChangeFull.
        /// </summary>
        public BlockT<Markers> StartMarker;

        /// <summary>
        /// A MessageChangeHeader value.
        /// </summary>
        public PropList MessageChangeHeader;

        /// <summary>
        /// A second marker for MessageChangeFull.
        /// </summary>
        public BlockT<Markers> SecondMarker;

        /// <summary>
        /// A PropList value.
        /// </summary>
        public PropList PropList;

        /// <summary>
        /// A MessageChildren value.
        /// </summary>
        public MessageChildren MessageChildren;

        /// <summary>
        /// Verify that a stream's current position contains a serialized messageChangeFull.
        /// </summary>
        /// <param name="parser">A BinaryParser.</param>
        /// <returns>If the stream's current position contains
        /// a serialized messageChangeFull, return true, else false.</returns>
        public static bool Verify(BinaryParser parser)
        {
            return MarkersHelper.VerifyMarker(parser, Markers.IncrSyncChg);
        }

        protected override void Parse()
        {
            StartMarker = ParseT<Markers>();
            if (StartMarker == Markers.IncrSyncChg)
            {
                MessageChangeHeader = Parse<PropList>();

                SecondMarker = ParseT<Markers>();
                if (SecondMarker == Markers.IncrSyncMessage)
                {
                    PropList = Parse<PropList>();
                    MessageChildren = Parse<MessageChildren>();
                }
                else
                {
                    Parsed = false;
                }
            }
        }

        protected override void ParseBlocks()
        {
            Text = "MessageChangeFull";
            AddChildBlockT(StartMarker, "StartMarker");
            AddLabeledChild(MessageChangeHeader, "MessageChangeHeader");
            AddChildBlockT(SecondMarker, "SecondMarker");
            AddLabeledChild(PropList, "PropList");
            AddLabeledChild(MessageChildren, "MessageChildren");
        }
    }
}
