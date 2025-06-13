namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The ProgressPerMessage element contains data that describes the approximate size of message change data that follows.
    /// </summary>
    public class ProgressPerMessage : SyntacticalBase
    {
        /// <summary>
        /// The start marker of ProgressPerMessage.
        /// </summary>
        public Markers StartMarker;

        /// <summary>
        /// A PropList value.
        /// </summary>
        public PropList PropList;

        /// <summary>
        /// Initializes a new instance of the ProgressPerMessage class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public ProgressPerMessage(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized ProgressPerMessage.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized ProgressPerMessage, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.VerifyMarker(Markers.IncrSyncProgressPerMsg);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.ReadMarker() == Markers.IncrSyncProgressPerMsg)
            {
                this.StartMarker = Markers.IncrSyncProgressPerMsg;
                this.PropList = new PropList(stream);
            }
        }
    }
}
