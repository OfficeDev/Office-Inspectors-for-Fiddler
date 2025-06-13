namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The GroupInfo element provides a definition for the property group mapping.
    /// </summary>
    public class GroupInfo : SyntacticalBase
    {
        /// <summary>
        /// The start marker of GroupInfo.
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
        public PropertyGroupInfo PropList;

        /// <summary>
        /// Initializes a new instance of the GroupInfo class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public GroupInfo(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized GroupInfo.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized GroupInfo, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.VerifyMarker(Markers.IncrSyncGroupInfo);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.ReadMarker() == Markers.IncrSyncGroupInfo)
            {
                this.StartMarker = Markers.IncrSyncGroupInfo;
                this.PropertiesTag = stream.ReadUInt32();
                this.PropertiesLength = stream.ReadUInt32();
                PropertyGroupInfo tmpGroupInfo = new PropertyGroupInfo();
                tmpGroupInfo.Parse(stream);
                this.PropList = tmpGroupInfo;
            }
        }
    }
}
