using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.7.3 RopGetAddressTypes
    /// A class indicates the RopGetAddressTypes ROP Response Buffer.
    /// </summary>
    public class RopGetAddressTypesResponse : BaseStructure
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
        /// An unsigned integer that specifies the number of strings in the AddressTypes field.
        /// </summary>
        public ushort? AddressTypeCount;

        /// <summary>
        /// An unsigned integer that specifies the length of the AddressTypes field.
        /// </summary>
        public ushort? AddressTypeSize;

        /// <summary>
        /// A list of null-terminated ASCII strings.
        /// </summary>
        public MAPIString[] AddressTypes;

        /// <summary>
        /// Parse the RopGetAddressTypesResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetAddressTypesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                AddressTypeCount = ReadUshort();
                AddressTypeSize = ReadUshort();
                List<MAPIString> listAddressTypes = new List<MAPIString>();

                for (int i = 0; i < AddressTypeCount; i++)
                {
                    MAPIString tempAddressTypes = new MAPIString(Encoding.ASCII);
                    tempAddressTypes.Parse(s);
                    listAddressTypes.Add(tempAddressTypes);
                }

                AddressTypes = listAddressTypes.ToArray();
            }
        }
    }
}
