namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The Deletions element contains information of messages that have been deleted expired or moved out of the sync scope.
    /// </summary>
    public class Deletions : SyntacticalBase
    {
        /// <summary>
        /// The start marker of Deletions.
        /// </summary>
        public Markers StartMarker;

        /// <summary>
        /// A PropList value.
        /// </summary>
        public PropList PropList;

        /// <summary>
        /// Initializes a new instance of the Deletions class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public Deletions(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized Deletions.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized Deletions, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.VerifyMarker(Markers.IncrSyncDel);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.ReadMarker() == Markers.IncrSyncDel)
            {
                this.StartMarker = Markers.IncrSyncDel;
                this.PropList = new PropList(stream);
            }
        }
    }
}
