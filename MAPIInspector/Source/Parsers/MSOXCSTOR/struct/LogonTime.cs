namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  A class indicates the RopLogon time.
    /// </summary>
    public class LogonTime : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the current second.
        /// </summary>
        public byte Seconds;

        /// <summary>
        /// An unsigned integer that specifies the current Minutes.
        /// </summary>
        public byte Minutes;

        /// <summary>
        /// An unsigned integer that specifies the current Hour.
        /// </summary>
        public byte Hour;

        /// <summary>
        /// An enumeration that specifies the current day of the week.
        /// </summary>
        public DayOfWeek DayOfWeek;

        /// <summary>
        /// An unsigned integer that specifies the current day of the month.
        /// </summary>
        public byte Day;

        /// <summary>
        /// An unsigned integer that specifies the current month 
        /// </summary>
        public Month Month;

        /// <summary>
        /// An unsigned integer that specifies the current year.
        /// </summary>
        public ushort Year;

        /// <summary>
        /// Parse the LogonTime structure.
        /// </summary>
        /// <param name="s">A stream containing LogonTime structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            Seconds = ReadByte();
            Minutes = ReadByte();
            Hour = ReadByte();
            DayOfWeek = (DayOfWeek)ReadByte();
            Day = ReadByte();
            Month = (Month)ReadByte();
            Year = ReadUshort();
        }
    }
}
