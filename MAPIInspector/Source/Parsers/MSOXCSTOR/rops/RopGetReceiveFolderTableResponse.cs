namespace MAPIInspector.Parsers
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    ///  2.2.1.4 RopGetReceiveFolderTable
    ///  A class indicates the RopGetReceiveFolderTable ROP Response Buffer.
    /// </summary>
    public class RopGetReceiveFolderTableResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of row structures contained in the Rows field.
        /// </summary>
        public uint? RowCount;

        /// <summary>
        /// An array of row structures. This field contains the rows of the Receive folder table. Each row is returned in either a StandardPropertyRow or a FlaggedPropertyRow structure.
        /// </summary>
        public PropertyRow[] Rows;

        /// <summary>
        /// Parse the RopGetReceiveFolderTableResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetReceiveFolderTableResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            List<PropertyRow> tmpRows = new List<PropertyRow>();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                RowCount = ReadUint();

                for (int i = 0; i < RowCount; i++)
                {
                    // PidTagMessageClass is defined as PtypString8 due to Open Specification said all characters in this property MUST be from the ASCII characters 0x20 through 0x7F. 
                    PropertyTag[] properties_GetReceiveFolderTable = new PropertyTag[3]
                    {
                      new PropertyTag(PropertyDataType.PtypInteger64, PidTagPropertyEnum.PidTagFolderId),
                      new PropertyTag(PropertyDataType.PtypString8, PidTagPropertyEnum.PidTagMessageClass),
                      new PropertyTag(PropertyDataType.PtypTime, PidTagPropertyEnum.PidTagLastModificationTime)
                    };
                    PropertyRow proRow = new PropertyRow(properties_GetReceiveFolderTable);
                    proRow.Parse(s);
                    tmpRows.Add(proRow);
                }

                Rows = tmpRows.ToArray();
            }
        }
    }
}
