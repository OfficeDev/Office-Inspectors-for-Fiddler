using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.7.9 RopOptionsData
    /// A class indicates the RopOptionsData ROP Response Buffer.
    /// </summary>
    public class RopOptionsDataResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<ErrorCodes> ReturnValue;

        /// <summary>
        /// Reserved byte
        /// </summary>
        public BlockT<byte> Reserved;

        /// <summary>
        /// An unsigned integer that specifies the size of the OptionsInfo field.
        /// </summary>
        public BlockT<ushort> OptionalInfoSize;

        /// <summary>
        /// An array of bytes that contains opaque data from the server.
        /// </summary>
        public BlockBytes OptionalInfo;

        /// <summary>
        /// An unsigned integer that specifies the size of the HelpFile field.
        /// </summary>
        public BlockT<ushort> HelpFileSize;

        /// <summary>
        /// An array of bytes that contains the help file associated with the specified address type.
        /// </summary>
        public BlockBytes HelpFile;

        /// <summary>
        /// A null-terminated multibyte string that specifies the name of the help file that is associated with the specified address type.
        /// </summary>
        public BlockString HelpFileName;

        /// <summary>
        /// Parse the RopOptionsDataResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            this.AddError(ReturnValue, "ReturnValue");

            if (ReturnValue == ErrorCodes.Success)
            {
                Reserved = ParseT<byte>();
                OptionalInfoSize = ParseT<ushort>();
                OptionalInfo = ParseBytes((int)OptionalInfoSize);
                HelpFileSize = ParseT<ushort>();

                if (HelpFileSize != 0)
                {
                    HelpFile = ParseBytes((int)HelpFileSize);
                    HelpFileName = ParseStringA();
                }
            }
        }

        protected override void ParseBlocks()
        {
            Text = "RopOptionsDataResponse";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
            AddChildBlockT(Reserved, "Reserved");
            AddChildBlockT(OptionalInfoSize, "OptionalInfoSize");
            AddChildBytes(OptionalInfo, "OptionalInfo");
            AddChildBlockT(HelpFileSize, "HelpFileSize");
            AddChildBytes(HelpFile, "HelpFile");
            AddChildString(HelpFileName, "HelpFileName");
        }
    }
}
