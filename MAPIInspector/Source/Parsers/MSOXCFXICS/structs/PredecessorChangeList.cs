using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Contains a set of XIDs that represent change numbers of messaging objects in different replicas.
    /// 2.2.2.3 PredecessorChangeList Structure
    /// </summary>
    public class PredecessorChangeList : Block
    {
        /// <summary>
        /// A SizedXid list.
        /// </summary>
        public SizedXid[] SizedXidList;

        /// <summary>
        /// A unsigned int value specifies the length in bytes of the SizedXidList.
        /// </summary>
        private int length;

        /// <summary>
        /// Initializes a new instance of the PredecessorChangeList class.
        /// </summary>
        /// <param name="length">The length of the sizedXid structure.</param>
        public PredecessorChangeList(int length)
        {
            this.length = length;
        }

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        protected override void Parse()
        {
            var interSizeXid = new List<SizedXid>();
            for (long i = 0; i < length;)
            {
                var tmpSizedXid = Parse<SizedXid>();
                interSizeXid.Add(tmpSizedXid);
                i += tmpSizedXid.Size;
            }

            SizedXidList = interSizeXid.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("PredecessorChangeList");
            foreach (var sizedXid in SizedXidList)
            {
                AddLabeledChild(sizedXid, "SizedXid");
            }
        }
    }
}
