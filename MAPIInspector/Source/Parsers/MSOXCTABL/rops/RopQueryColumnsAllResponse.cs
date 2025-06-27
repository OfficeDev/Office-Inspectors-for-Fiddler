using BlockParser;
using System.Collections.Generic;
using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.13 RopQueryColumnsAll ROP
    ///  A class indicates the RopQueryColumnsAll ROP Response Buffer.
    /// </summary>
    public class RopQueryColumnsAllResponse : BaseStructure
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
        /// An unsigned integer that specifies how many tags are present in the PropertyTags field.
        /// </summary>
        public ushort? PropertyTagCount;

        /// <summary>
        /// An array of PropertyTag structures that specifies the columns of the table. 
        /// </summary>
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopQueryColumnsAllResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopQueryColumnsAllResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                PropertyTagCount = ReadUshort();
                List<PropertyTag> tempPropertyTags = new List<PropertyTag>();
                for (int i = 0; i < PropertyTagCount; i++)
                {
                    PropertyTag tempPropertyTag = Block.Parse<PropertyTag>(s);
                    tempPropertyTags.Add(tempPropertyTag);
                }

                PropertyTags = tempPropertyTags.ToArray();
            }
        }
    }
}
