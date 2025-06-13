namespace MAPIInspector.Parsers
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The hierarchySync element contains the result of the hierarchy synchronization download operation.
    /// </summary>
    public class HierarchySync : SyntacticalBase
    {
        /// <summary>
        /// A list of FolderChange value.
        /// </summary>
        public FolderChange[] FolderChanges;

        /// <summary>
        /// A Deletions value.
        /// </summary>
        public Deletions Deletions;

        /// <summary>
        /// The State value.
        /// </summary>
        public State State;

        /// <summary>
        /// The end marker of hierarchySync.
        /// </summary>
        public Markers EndMarker;

        /// <summary>
        /// Initializes a new instance of the HierarchySync class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public HierarchySync(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized hierarchySync.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized hierarchySync, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return (FolderChange.Verify(stream)
                || Deletions.Verify(stream)
                || State.Verify(stream))
                && stream.VerifyMarker(Markers.IncrSyncEnd, (int)stream.Length - 4 - (int)stream.Position);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            List<FolderChange> interFolderChanges = new List<FolderChange>();

            while (FolderChange.Verify(stream))
            {
                interFolderChanges.Add(new FolderChange(stream));
            }

            this.FolderChanges = interFolderChanges.ToArray();

            if (Deletions.Verify(stream))
            {
                this.Deletions = new Deletions(stream);
            }

            this.State = new State(stream);

            if (stream.ReadMarker() == Markers.IncrSyncEnd)
            {
                this.EndMarker = Markers.IncrSyncEnd;
            }
            else
            {
                throw new Exception("The HierarchySync cannot be parsed successfully. The IncrSyncEnd Marker is missed.");
            }
        }
    }
}
