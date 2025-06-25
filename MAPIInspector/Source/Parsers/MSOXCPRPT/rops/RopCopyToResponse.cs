namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.2.11 RopCopyTo
    ///  A class indicates the RopCopyTo ROP Response Buffer.
    /// </summary>
    public class RopCopyToResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the SourceHandleIndex field specified in the request.
        /// </summary>
        public byte SourceHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of PropertyProblem structures in the PropertyProblems field. 
        /// </summary>
        public ushort? PropertyProblemCount;

        /// <summary>
        /// An array of PropertyProblem structures. 
        /// </summary>
        public PropertyProblem[] PropertyProblems;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the DestHandleIndex field in the request.
        /// </summary>
        public uint? DestHandleIndex;

        /// <summary>
        /// Parse the RopCopyToResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopCopyToResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            SourceHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                PropertyProblemCount = ReadUshort();
                PropertyProblem[] interPropertyProblem = new PropertyProblem[(int)PropertyProblemCount];

                for (int i = 0; i < PropertyProblemCount; i++)
                {
                    interPropertyProblem[i] = new PropertyProblem();
                    interPropertyProblem[i].Parse(s);
                }

                PropertyProblems = interPropertyProblem;
            }
            else if ((AdditionalErrorCodes)ReturnValue == AdditionalErrorCodes.NullDestinationObject)
            {
                DestHandleIndex = ReadUint();
            }
        }
    }
}
