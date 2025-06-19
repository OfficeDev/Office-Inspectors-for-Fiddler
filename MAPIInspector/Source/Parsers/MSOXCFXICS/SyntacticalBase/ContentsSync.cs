namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.Collections.Generic;

    /// <summary>
    /// The ContentsSync element contains the result of the contents synchronization download operation.
    /// </summary>
    public class ContentsSync : Block
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
        public BlockT<Markers> EndMarker;

        protected override void Parse()
        {
            var interProgressPerMessageChanges = new List<ProgressPerMessageChange>();

            if (ProgressTotal.Verify(parser))
            {
                ProgressTotal = Parse<ProgressTotal>();
            }

            while (ProgressPerMessageChange.Verify(parser))
            {
                interProgressPerMessageChanges.Add(Parse<ProgressPerMessageChange>());
            }

            ProgressPerMessageChanges = interProgressPerMessageChanges.ToArray();

            if (Deletions.Verify(parser))
            {
                Deletions = Parse<Deletions>();
            }

            if (ReadStateChanges.Verify(parser))
            {
                ReadStateChanges = Parse<ReadStateChanges>();
            }

            State = Parse<State>();

            EndMarker = ParseT<Markers>();
            if (EndMarker.Data != Markers.IncrSyncEnd)
            {
                Parsed = false;
            }
        }

        protected override void ParseBlocks()
        {
            SetText("ContentsSync");
            AddChild(ProgressTotal);
            if (ProgressPerMessageChanges != null)
            {
                foreach (var progress in ProgressPerMessageChanges)
                {
                    AddChild(progress);
                }
            }

            AddChild(Deletions);
            AddChild(ReadStateChanges);
            AddChild(State);
            AddChildBlockT(EndMarker, "EndMarker");
        }
    }
}
