using System.IO;
using System.Text;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.7.9 RopOptionsData
    /// A class indicates the RopOptionsData ROP Response Buffer.
    /// </summary>
    public class RopOptionsDataResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Reserved byte
        /// </summary>
        public byte? Reserved;

        /// <summary>
        /// An unsigned integer that specifies the size of the OptionsInfo field.
        /// </summary>
        public ushort? OptionalInfoSize;

        /// <summary>
        /// An array of bytes that contains opaque data from the server.
        /// </summary>
        public byte?[] OptionalInfo;

        /// <summary>
        /// An unsigned integer that specifies the size of the HelpFile field.
        /// </summary>
        public ushort? HelpFileSize;

        /// <summary>
        /// An array of bytes that contains the help file associated with the specified address type.
        /// </summary>
        public byte?[] HelpFile;

        /// <summary>
        /// A null-terminated multibyte string that specifies the name of the help file that is associated with the specified address type.
        /// </summary>
        public MAPIString HelpFileName;

        /// <summary>
        /// Parse the RopOptionsDataResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopOptionsDataResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                Reserved = ReadByte();
                OptionalInfoSize = ReadUshort();
                OptionalInfo = ConvertArray(ReadBytes((int)OptionalInfoSize));
                HelpFileSize = ReadUshort();

                if (HelpFileSize != 0)
                {
                    HelpFile = ConvertArray(ReadBytes((int)HelpFileSize));
                    HelpFileName = new MAPIString(Encoding.ASCII);
                    HelpFileName.Parse(s);
                }
            }
        }
    }
}
