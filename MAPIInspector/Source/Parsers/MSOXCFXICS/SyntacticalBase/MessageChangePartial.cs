namespace MAPIInspector.Parsers
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The MessageChangePartial element represents the difference in message content since the last download, as identified by the initial ICS state.
    /// </summary>
    public class MessageChangePartial : SyntacticalBase
    {
        /// <summary>
        /// A groupInfo value.
        /// </summary>
        public GroupInfo GroupInfo;

        /// <summary>
        /// A MetaTagIncrSyncGroupId property.
        /// </summary>
        public MetaPropValue MetaTagIncrSyncGroupId;

        /// <summary>
        /// The MessageChangePartial marker.
        /// </summary>
        public Markers Marker;

        /// <summary>
        /// A MessageChangeHeader value.
        /// </summary>
        public PropList MessageChangeHeader;

        /// <summary>
        /// A list of SyncMessagePartialPropList values.
        /// </summary>
        public SyncMessagePartialPropList[] SyncMessagePartialPropList;

        /// <summary>
        /// A MessageChildren field.
        /// </summary>
        public MessageChildren MessageChildren;

        /// <summary>
        /// Initializes a new instance of the MessageChangePartial class.
        /// </summary>
        /// <param name="stream">A FastTransferStream object.</param>
        public MessageChangePartial(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized MessageChangePartial.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized MessageChangePartial, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return GroupInfo.Verify(stream);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            List<SyncMessagePartialPropList> interMessagePartialList = new List<SyncMessagePartialPropList>();
            this.GroupInfo = new GroupInfo(stream);

            if (stream.VerifyMetaProperty(MetaProperties.MetaTagIncrSyncGroupId))
            {
                this.MetaTagIncrSyncGroupId = new MetaPropValue(stream);
            }

            if (stream.ReadMarker() == Markers.IncrSyncChgPartial)
            {
                this.Marker = Markers.IncrSyncChgPartial;
                this.MessageChangeHeader = new PropList(stream);

                while (stream.VerifyMetaProperty(MetaProperties.MetaTagIncrementalSyncMessagePartial))
                {
                    interMessagePartialList.Add(new SyncMessagePartialPropList(stream));
                }

                this.SyncMessagePartialPropList = interMessagePartialList.ToArray();
                this.MessageChildren = new MessageChildren(stream);
            }
            else
            {
                throw new Exception("The MessageChangePartial cannot be parsed successfully. The IncrSyncChgPartial Marker is missed.");
            }
        }
    }
}
