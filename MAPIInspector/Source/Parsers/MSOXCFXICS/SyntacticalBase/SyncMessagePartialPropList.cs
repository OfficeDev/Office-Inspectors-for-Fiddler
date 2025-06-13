namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The SyncMessagePartialPropList is used to parse MessageChangePartial element.
    /// </summary>
    public class SyncMessagePartialPropList : SyntacticalBase
    {
        /// <summary>
        /// A MetaTagIncrementalSyncMessagePartial property.
        /// </summary>
        public MetaPropValue MetaSyncMessagePartial;

        /// <summary>
        /// A PropList value.
        /// </summary>
        public PropList PropList;

        /// <summary>
        /// Initializes a new instance of the SyncMessagePartialPropList class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public SyncMessagePartialPropList(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized SyncMessagePartialPropList.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized SyncMessagePartialPropList, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.VerifyUInt32() == (uint)MetaProperties.MetaTagIncrementalSyncMessagePartial;
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.VerifyMetaProperty(MetaProperties.MetaTagIncrementalSyncMessagePartial))
            {
                this.MetaSyncMessagePartial = new MetaPropValue(stream);
            }

            this.PropList = new PropList(stream);
        }
    }
}
