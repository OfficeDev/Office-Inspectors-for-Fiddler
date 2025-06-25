namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.2.7 RopDeleteProperties
    ///  A class indicates the RopDeleteProperties ROP Response Buffer.
    /// </summary>
    public class RopDeletePropertiesResponse : BaseStructure
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
        /// An unsigned integer that specifies the number of PropertyProblem structures in the PropertyProblems field. 
        /// </summary>
        public ushort? PropertyProblemCount;

        /// <summary>
        /// An array of PropertyProblem structures. The number of structures contained in this field is specified by the PropertyProblemCount field. 
        /// </summary>
        public PropertyProblem[] PropertyProblems;

        /// <summary>
        /// Parse the RopDeletePropertiesResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopDeletePropertiesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
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
        }
    }
}
