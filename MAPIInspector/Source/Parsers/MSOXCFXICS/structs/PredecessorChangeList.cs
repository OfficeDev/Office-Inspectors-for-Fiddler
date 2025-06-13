namespace MAPIInspector.Parsers
{
    using System.Collections.Generic;

    /// <summary>
    /// Contains a set of XIDs that represent change numbers of messaging objects in different replicas. 
    /// 2.2.2.3 PredecessorChangeList Structure
    /// </summary>
    public class PredecessorChangeList : BaseStructure
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
        /// <param name="stream">A stream contains PredecessorChangeList.</param>
        public void Parse(FastTransferStream stream)
        {
            List<SizedXid> interSizeXid = new List<SizedXid>();
            for (int i = 0; i < this.length;)
            {
                int position = (int)stream.Position;
                SizedXid tmpSizedXid = new SizedXid();
                tmpSizedXid.Parse(stream);
                interSizeXid.Add(tmpSizedXid);

                i += (int)stream.Position - position;
            }

            this.SizedXidList = interSizeXid.ToArray();
        }
    }
}
