namespace MAPIInspector.Parsers
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The ContentsSync element contains the result of the contents synchronization download operation.
    /// </summary>
    public class ContentsSync : SyntacticalBase
    {
        /// <summary>
        /// A ProgressTotal value
        /// </summary>
        public ProgressTotal ProgressTotal;

        /// <summary>
        /// A list of ProgressPerMessageChange value
        /// </summary>
        public ProgressPerMessageChange[] ProgressPerMessageChanges;

        /// <summary>
        /// A Deletions value
        /// </summary>
        public Deletions Deletions;

        /// <summary>
        /// A readStateChanges value.
        /// </summary>
        public ReadStateChanges ReadStateChanges;

        /// <summary>
        /// A state value.
        /// </summary>
        public State State;

        /// <summary>
        /// A end marker of ContentSync.
        /// </summary>
        public Markers EndMarker;

        /// <summary>
        /// Initializes a new instance of the ContentsSync class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public ContentsSync(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized contentsSync.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized contentsSync, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return (ProgressTotal.Verify(stream)
                || ProgressPerMessageChange.Verify(stream)
                || Deletions.Verify(stream)
                || ReadStateChanges.Verify(stream)
                || State.Verify(stream))
                && stream.VerifyMarker(Markers.IncrSyncEnd, (int)stream.Length - 4 - (int)stream.Position);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            List<ProgressPerMessageChange> interProgressPerMessageChanges = new List<ProgressPerMessageChange>();

            if (ProgressTotal.Verify(stream))
            {
                this.ProgressTotal = new ProgressTotal(stream);
            }

            while (ProgressPerMessageChange.Verify(stream))
            {
                interProgressPerMessageChanges.Add(new ProgressPerMessageChange(stream));
            }

            this.ProgressPerMessageChanges = interProgressPerMessageChanges.ToArray();

            if (Deletions.Verify(stream))
            {
                this.Deletions = new Deletions(stream);
            }

            if (ReadStateChanges.Verify(stream))
            {
                this.ReadStateChanges = new ReadStateChanges(stream);
            }

            this.State = new State(stream);

            if (stream.ReadMarker() == Markers.IncrSyncEnd)
            {
                this.EndMarker = Markers.IncrSyncEnd;
            }
            else
            {
                throw new Exception("The ContentsSync cannot be parsed successfully. The IncrSyncEnd Marker is missed.");
            }
        }
    }
}
