using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MAPIInspector.Parsers
{
    /// <summary>
    ///  A class indicates the DnToMinIdRequest structure.
    ///  2.2.5 Request Types for Address Book Server Endpoint
    ///  2.2.5.4 DnToMinId
    /// </summary>
    public class DnToMinIdRequest : BaseStructure
    {
        /// <summary>
        /// The reserved field
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// A Boolean value that specifies whether the NameCount and NameValues fields are present.
        /// </summary>
        public bool HasNames;

        /// <summary>
        /// An unsigned integer that specifies the number of null-terminated Unicode strings in the NameValues field. 
        /// </summary>
        public uint? NameCount;

        /// <summary>
        /// An array of null-terminated ASCII strings which are distinguished names (DNs) to be mapped to Minimal Entry IDs. 
        /// </summary>
        public MAPIString[] NameValues;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data returned from the server. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the DnToMinIdRequest structure.
        /// </summary>
        /// <param name="s">A stream containing DnToMinIdRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            Reserved = ReadUint();
            HasNames = ReadBoolean();
            uint count = ReadUint();
            List<MAPIString> nameValues = new List<MAPIString>();

            if (count == 0)
            {
                s.Position -= 4;
            }
            else
            {
                NameCount = count;

                for (int i = 0; i < NameCount; i++)
                {
                    MAPIString mapiString = new MAPIString(Encoding.ASCII);
                    mapiString.Parse(s);
                    nameValues.Add(mapiString);
                }
            }

            NameValues = nameValues.ToArray();
            AuxiliaryBufferSize = ReadUint();

            if (AuxiliaryBufferSize > 0)
            {
                AuxiliaryBuffer = new ExtendedBuffer();
                AuxiliaryBuffer.Parse(s);
            }
            else
            {
                AuxiliaryBuffer = null;
            }
        }
    }
}