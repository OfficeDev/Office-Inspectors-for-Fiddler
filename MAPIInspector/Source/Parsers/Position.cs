using BlockParser;

namespace MAPIInspector.Parsers
{
    public partial class BaseStructure
    {
        /// <summary>
        /// Record start position and byte counts consumed
        /// </summary>
        public class Position
        {
            /// <summary>
            /// Int value specifies field start position
            /// </summary>
            public int StartIndex;

            /// <summary>
            /// Int value specifies field length
            /// </summary>
            public int Offset;

            /// <summary>
            /// Boolean value specifies if field is in the compressed payload
            /// </summary>
            public bool IsCompressedXOR;

            /// <summary>
            /// Boolean value specifies if field is in the auxiliary payload
            /// </summary>
            public bool IsAuxiliaryPayload;

            /// <summary>
            /// Int value specifies the buffer index of a field
            /// </summary>
            public int BufferIndex = 0;

            /// <summary>
            /// Source block
            /// </summary>
            public Block SourceBlock = null;

            /// <summary>
            /// Initializes a new instance of the Position class
            /// </summary>
            /// <param name="startIndex">The start position of field</param>
            /// <param name="offset">The Length of field </param>
            public Position(int startIndex, int offset)
            {
                StartIndex = startIndex;
                Offset = offset;
                IsAuxiliaryPayload = false;
            }
        }
    }
}
