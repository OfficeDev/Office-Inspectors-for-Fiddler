namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The progressTotal element contains data that describes the approximate size of all the messageChange elements.
    /// </summary>
    public class ProgressTotal : SyntacticalBase
    {
        /// <summary>
        /// The start marker of progressTotal.
        /// </summary>
        public Markers StartMarker;

        /// <summary>
        /// The propertyTag for ProgressInformation.
        /// </summary>
        public uint PropertiesTag;

        /// <summary>
        /// The count of the PropList.
        /// </summary>
        public uint PropertiesLength;

        /// <summary>
        /// A PropList value.
        /// </summary>
        public ProgressInformation PropList;

        /// <summary>
        /// Initializes a new instance of the ProgressTotal class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public ProgressTotal(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized progressTotal.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized progressTotal, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.VerifyMarker(Markers.IncrSyncProgressMode);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.ReadMarker() == Markers.IncrSyncProgressMode)
            {
                this.StartMarker = Markers.IncrSyncProgressMode;
                this.PropertiesTag = stream.ReadUInt32();
                this.PropertiesLength = stream.ReadUInt32();
                ProgressInformation tmpProgressInfo = new ProgressInformation();
                tmpProgressInfo.Parse(stream);
                this.PropList = tmpProgressInfo;
            }
        }
    }
}
