namespace MAPIInspector.Parsers
{
    using System;

    /// <summary>
    /// The state element contains the final ICS state of the synchronization download operation. 
    /// </summary>
    public class State : SyntacticalBase
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
        /// The end marker of ReadStateChange.
        /// </summary>
        public Markers EndMarker;

        /// <summary>
        /// Initializes a new instance of the State class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public State(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized State.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized State, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.VerifyMarker(Markers.IncrSyncStateBegin);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.ReadMarker() == Markers.IncrSyncStateBegin)
            {
                this.StartMarker = Markers.IncrSyncStateBegin;
                this.PropList = new PropList(stream);

                if (stream.ReadMarker() == Markers.IncrSyncStateEnd)
                {
                    this.EndMarker = Markers.IncrSyncStateEnd;
                }
                else
                {
                    throw new Exception("The State cannot be parsed successfully. The IncrSyncStateEnd Marker is missed.");
                }
            }
        }
    }
}
