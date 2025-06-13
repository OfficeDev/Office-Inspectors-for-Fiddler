namespace MAPIInspector.Parsers
{
    using System;

    /// <summary>
    /// The messageChangeFull element contains the complete content of a new or changed message: the message properties, the recipients,and the attachments.
    /// </summary>
    public class MessageChangeFull : SyntacticalBase
    {
        /// <summary>
        /// A start marker for MessageChangeFull.
        /// </summary>
        public Markers StartMarker;

        /// <summary>
        /// A MessageChangeHeader value.
        /// </summary>
        public PropList MessageChangeHeader;

        /// <summary>
        /// A second marker for MessageChangeFull.
        /// </summary>
        public Markers SecondMarker;

        /// <summary>
        /// A PropList value.
        /// </summary>
        public PropList PropList;

        /// <summary>
        /// A MessageChildren value.
        /// </summary>
        public MessageChildren MessageChildren;

        /// <summary>
        /// Initializes a new instance of the MessageChangeFull class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public MessageChangeFull(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized messageChangeFull.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains 
        /// a serialized messageChangeFull, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.VerifyMarker(Markers.IncrSyncChg);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.ReadMarker() == Markers.IncrSyncChg)
            {
                this.StartMarker = Markers.IncrSyncChg;
                this.MessageChangeHeader = new PropList(stream);

                if (stream.ReadMarker() == Markers.IncrSyncMessage)
                {
                    this.SecondMarker = Markers.IncrSyncMessage;
                    this.PropList = new PropList(stream);
                    this.MessageChildren = new MessageChildren(stream);
                }
                else
                {
                    throw new Exception("The MessageChangeFull cannot be parsed successfully. The IncrSyncMessage Marker is missed.");
                }
            }
        }
    }
}
