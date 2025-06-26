using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The hierarchySync element contains the result of the hierarchy synchronization download operation.
    /// </summary>
    public class HierarchySync : Block
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
        public BlockT<Markers> EndMarker;

        protected override void Parse()
        {
            var interFolderChanges = new List<FolderChange>();

            while (FolderChange.Verify(parser))
            {
                interFolderChanges.Add(Parse<FolderChange>());
            }

            FolderChanges = interFolderChanges.ToArray();

            if (Deletions.Verify(parser))
            {
                Deletions = Parse<Deletions>();
            }

            State = Parse<State>();

            EndMarker = ParseT<Markers>();
            if (EndMarker.Data == Markers.IncrSyncEnd)
            {
                Parsed = false;
            }
        }

        protected override void ParseBlocks()
        {
            SetText("HierarchySync");
            if (FolderChanges != null)
            {
                foreach (var folderChange in FolderChanges)
                {
                    AddChild(folderChange, "FolderChange");
                }
            }

            AddLabeledChild(Deletions, "Deletions");
            AddLabeledChild(State, "State");

            AddChildBlockT(EndMarker, "EndMarker");
        }
    }
}
