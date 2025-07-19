using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.8 STAT
    /// A class indicates the STAT structure.
    /// </summary>
    public class STAT : Block
    {
        /// <summary>
        /// A DWORD [MS-DTYP] value that specifies a sort order.
        /// </summary>
        public BlockT<uint> SortType;

        /// <summary>
        /// A DWORD value that specifies the Minimal Entry ID of the address book container that STAT structure represents.
        /// </summary>
        public BlockT<uint> ContainerID;

        /// <summary>
        /// A DWORD value that specifies a beginning position in the table for the start of an NSPI method.
        /// </summary>
        public BlockT<uint> CurrentRec;

        /// <summary>
        /// A long value that specifies an offset from the beginning position in the table for the start of an NSPI method.
        /// </summary>
        public BlockT<uint> Delta;

        /// <summary>
        /// A DWORD value that specifies a position in the table.
        /// </summary>
        public BlockT<uint> NumPos;

        /// <summary>
        /// A DWORD value that specifies the number of rows in the table.
        /// </summary>
        public BlockT<uint> TotalRecs;

        /// <summary>
        /// A DWORD value that represents a code page.
        /// </summary>
        public BlockT<uint> CodePage;

        /// <summary>
        /// A DWORD value that represents a language code identifier (LCID).
        /// </summary>
        public BlockT<uint> TemplateLocale;

        /// <summary>
        /// A DWORD value that represents an LCID.
        /// </summary>
        public BlockT<uint> SortLocale;

        /// <summary>
        /// Parse the STAT payload of session.
        /// </summary>
        /// <param name="s">The stream containing STAT structure.</param>
        protected override void Parse()
        {
            SortType = ParseT<uint>();
            ContainerID = ParseT<uint>();
            CurrentRec = ParseT<uint>();
            Delta = ParseT<uint>();
            NumPos = ParseT<uint>();
            TotalRecs = ParseT<uint>();
            CodePage = ParseT<uint>();
            TemplateLocale = ParseT<uint>();
            SortLocale = ParseT<uint>();
        }

        protected override void ParseBlocks()
        {
            Text = "STAT";
            AddChildBlockT(SortType, "SortType");
            AddChildBlockT(ContainerID, "ContainerID");
            AddChildBlockT(CurrentRec, "CurrentRec");
            AddChildBlockT(Delta, "Delta");
            AddChildBlockT(NumPos, "NumPos");
            AddChildBlockT(TotalRecs, "TotalRecs");
            AddChildBlockT(CodePage, "CodePage");
            AddChildBlockT(TemplateLocale, "TemplateLocale");
            AddChildBlockT(SortLocale, "SortLocale");
        }
    }
}
