namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The readStateChanges element contains information of Message objects that had their read state changed
    /// </summary>
    public class ReadStateChanges : SyntacticalBase
    {
        /// <summary>
        /// The start marker of ReadStateChange.
        /// </summary>
        public Markers StartMarker;

        /// <summary>
        /// A PropList value.
        /// </summary>
        public PropList PropList;

        /// <summary>
        /// Initializes a new instance of the ReadStateChanges class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public ReadStateChanges(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized ReadStateChange.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized ReadStateChange, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.VerifyMarker(Markers.IncrSyncRead);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.ReadMarker() == Markers.IncrSyncRead)
            {
                this.StartMarker = Markers.IncrSyncRead;
                this.PropList = new PropList(stream);
            }
        }
    }
}
