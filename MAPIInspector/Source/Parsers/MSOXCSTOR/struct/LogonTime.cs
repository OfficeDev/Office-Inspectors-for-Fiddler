using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the RopLogon time.
    /// </summary>
    public class LogonTime : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the current second.
        /// </summary>
        public BlockT<byte> Seconds;

        /// <summary>
        /// An unsigned integer that specifies the current Minutes.
        /// </summary>
        public BlockT<byte> Minutes;

        /// <summary>
        /// An unsigned integer that specifies the current Hour.
        /// </summary>
        public BlockT<byte> Hour;

        /// <summary>
        /// An enumeration that specifies the current day of the week.
        /// </summary>
        public BlockT<DayOfWeek> DayOfWeek;

        /// <summary>
        /// An unsigned integer that specifies the current day of the month.
        /// </summary>
        public BlockT<byte> Day;

        /// <summary>
        /// An unsigned integer that specifies the current month
        /// </summary>
        public BlockT<Month> Month;

        /// <summary>
        /// An unsigned integer that specifies the current year.
        /// </summary>
        public BlockT<ushort> Year;

        /// <summary>
        /// Parse the LogonTime structure.
        /// </summary>
        protected override void Parse()
        {
            Seconds = ParseT<byte>();
            Minutes = ParseT<byte>();
            Hour = ParseT<byte>();
            DayOfWeek = ParseT<DayOfWeek>();
            Day = ParseT<byte>();
            Month = ParseT<Month>();
            Year = ParseT<ushort>();
        }

        protected override void ParseBlocks()
        {
            Text = $"LogonTime: {Year?.Data:D4}-{Month?.Data}-{Day?.Data:D2} {Hour?.Data:D2}:{Minutes?.Data:D2}:{Seconds?.Data:D2} ({DayOfWeek?.Data})";
            AddChildBlockT(Seconds, "Seconds");
            AddChildBlockT(Minutes, "Minutes");
            AddChildBlockT(Hour, "Hour");
            AddChildBlockT(DayOfWeek, "DayOfWeek");
            AddChildBlockT(Day, "Day");
            AddChildBlockT(Month, "Month");
            AddChildBlockT(Year, "Year");
        }
    }
}
