namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The MetaTagFxDelPropMessageList is used to parse folderMessages class.
    /// </summary>
    public class MetaTagFxDelPropMessageList : SyntacticalBase
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
        /// Initializes a new instance of the MetaTagFxDelPropMessageList class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public MetaTagFxDelPropMessageList(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized MetaTagFxDelPropMessageList
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized MetaTagFxDelPropMessageList, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return !stream.IsEndOfStream && stream.VerifyMetaProperty(MetaProperties.MetaTagFXDelProp);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            this.MetaTagFXDelProp = new MetaPropValue(stream);
            this.MessageLists = new MessageList(stream);
        }
    }
}
