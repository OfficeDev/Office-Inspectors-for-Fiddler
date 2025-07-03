using System.Collections.Generic;
using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.15.2 RopBackoff
    /// A class indicates the RopBackoff ROP Response Buffer.
    /// </summary>
    public class RopBackoffResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP. For this operation this field is set to 0x01.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer that specifies the number of milliseconds to apply a ROP BackOff.
        /// </summary>
        public uint Duration;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the BackoffRopData field.
        /// </summary>
        public byte BackoffRopCount;

        /// <summary>
        /// An array of BackoffRop structures. 
        /// </summary>
        public BackoffRop[] BackoffRopData;

        /// <summary>
        /// An unsigned integer that specifies the size of the AdditionalData field.
        /// </summary>
        public ushort AdditionalDataSize;

        /// <summary>
        /// An array of bytes that specifies additional information about the ROP BackOff response. 
        /// </summary>
        public byte[] AdditionalData;

        /// <summary>
        /// Parse the RopBackoffResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopBackoffResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            Duration = ReadUint();
            BackoffRopCount = ReadByte();
            List<BackoffRop> backoffRopDataList = new List<BackoffRop>();

            for (int i = 0; i < BackoffRopCount; i++)
            {
                BackoffRop subBackoffRop = new BackoffRop();
                subBackoffRop.Parse(s);
                backoffRopDataList.Add(subBackoffRop);
            }

            BackoffRopData = backoffRopDataList.ToArray();
            AdditionalDataSize = ReadUshort();
            AdditionalData = ReadBytes(AdditionalDataSize);
        }
    }
}
