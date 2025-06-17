using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The MetaTagFxDelPropMessageList is used to parse folderMessages class.
    /// </summary>
    public class MetaTagFxDelPropMessageList : Block
    {
        /// <summary>
        /// A MetaTagFXDelProp property. 
        /// </summary>
        public MetaPropValue MetaTagFXDelProp;

        /// <summary>
        /// A list of messageList.
        /// </summary>
        public MessageList MessageLists;

        /// <summary>
        /// Verify that a stream's current position contains a serialized MetaTagFxDelPropMessageList
        /// </summary>
        /// <param name="parser">A BinaryParser.</param>
        /// <returns>If the stream's current position contains a serialized MetaTagFxDelPropMessageList, return true, else false.</returns>
        public static bool Verify(BinaryParser parser)
        {
            return !parser.Empty && MarkersHelper.VerifyMetaProperty(parser, MetaProperties.MetaTagFXDelProp);
        }

        protected override void Parse()
        {
            MetaTagFXDelProp = Parse<MetaPropValue>(parser);
            MessageLists = Parse<MessageList>(parser);
        }

        protected override void ParseBlocks()
        {
            SetText("MetaTagFxDelPropMessageList");
            AddLabeledChild(MetaTagFXDelProp, "MetaTagFXDelProp");
            AddLabeledChild(MessageLists, "MessageLists");
        }
    }
}
