namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  A class indicates the LongTermIdRange structure.
    ///  2.2.13.12.1.1 LongTermIdRange Structure
    /// </summary>
    public class LongTermIdRange : BaseStructure
    {
        /// <summary>
        /// A LongTermId structure that specifies the beginning of a range. 
        /// </summary>
        public LongTermID MinLongTermId;

        /// <summary>
        /// A LongTermId structure that specifies the end of a range.
        /// </summary>
        public LongTermID MaxLongTermId;

        /// <summary>
        /// Parse the LongTermIdRange structure.
        /// </summary>
        /// <param name="s">A stream containing LongTermIdRange structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.MinLongTermId = new LongTermID();
            this.MinLongTermId.Parse(s);
            this.MaxLongTermId = new LongTermID();
            this.MaxLongTermId.Parse(s);
        }
    }
}
