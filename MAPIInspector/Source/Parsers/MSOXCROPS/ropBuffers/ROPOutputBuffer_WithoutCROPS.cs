using System.Collections.Generic;
using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the ROP output buffer, which is sent by the server, includes an array of ROP response buffers. 
    /// </summary>
    public class ROPOutputBuffer_WithoutCROPS : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the size of both this field and the RopsList field.
        /// </summary>
        public ushort RopSize;

        /// <summary>
        /// An array of ROP request buffers.
        /// </summary>
        public byte[] RopsList;

        /// <summary>
        /// An array of 32-bit values. Each 32-bit value specifies a Server object handle that is referenced by a ROP buffer.
        /// </summary>
        public uint[] ServerObjectHandleTable;

        /// <summary>
        /// Parse the ROPOutputBuffer_WithoutCROPS structure.
        /// </summary>
        /// <param name="s">A stream containing the ROPOutputBuffer structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopSize = ReadUshort();
            List<object> ropsList = new List<object>();
            List<uint> serverObjectHandleTable = new List<uint>();
            byte[] ropListBytes = ReadBytes(RopSize - 2);
            RopsList = ropListBytes;

            while (s.Position < s.Length)
            {
                uint serverObjectHandle = ReadUint();
                serverObjectHandleTable.Add(serverObjectHandle);
            }

            ServerObjectHandleTable = serverObjectHandleTable.ToArray();
        }
    }
}
