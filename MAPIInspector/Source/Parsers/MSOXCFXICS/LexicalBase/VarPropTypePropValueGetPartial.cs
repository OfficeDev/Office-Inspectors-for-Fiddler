using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The VarPropTypePropValueGetPartial class.
    /// </summary>
    public class VarPropTypePropValueGetPartial : PropValue
    {
        /// <summary>
        /// The length of a variate type value.
        /// </summary>
        public BlockT<int> Length;

        public Block SkippedBytes;

        /// <summary>
        /// The valueArray.
        /// </summary>
        public Block ValueArray;

        /// <summary>
        /// The length value used for partial split
        /// </summary>
        protected int plength;

        private Block Comment;

        protected override void Parse()
        {
            base.Parse();

            if (parser.Empty)
            {
                Partial.PartialGetType = PropType;
                Partial.PartialGetServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                Partial.PartialGetProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                Partial.PartialGetClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (Partial.PartialGetType != 0 &&
                    Partial.PartialGetServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                    Partial.PartialGetProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                    Partial.PartialGetClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    Comment = Partial.CreatePartialComment();

                    ptype = CreateBlock(Partial.PartialGetType, 0, 0);

                    if (Partial.PartialGetRemainSize != -1)
                    {
                        plength = Partial.PartialGetRemainSize;

                        if (plength % 2 != 0 &&
                            (ptype == PropertyDataType.PtypString ||
                            ptype == (PropertyDataType)CodePageType.PtypCodePageUnicode ||
                            ptype == (PropertyDataType)CodePageType.PtypCodePageUnicode52))
                        {
                            Partial.IsOneMoreGetByteToRead = true;
                        }

                        Partial.PartialGetRemainSize = -1;
                    }
                    else
                    {
                        Length = ParseT<int>();
                    }

                    // clear
                    Partial.PartialGetType = 0;

                    if (Partial.PartialGetRemainSize == -1)
                    {
                        Partial.PartialGetServerUrl = string.Empty;
                        Partial.PartialGetProcessName = string.Empty;
                        Partial.PartialGetClientInfo = string.Empty;
                    }
                }
                else
                {
                    Length = ParseT<int>();
                }

                int blockLength = Length != null ? Length : plength;
                PropertyDataType typeValue = PropertyDataType.PtypUnspecified;
                if (PropType != null)
                {
                    typeValue = PropType;
                }
                else if (ptype != null)
                {
                    typeValue = ptype;
                }

                if (parser.RemainingBytes < blockLength)
                {
                    Partial.PartialGetType = typeValue;
                    Partial.PartialGetServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                    Partial.PartialGetProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                    Partial.PartialGetClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
                }

                if (LexicalTypeHelper.IsCodePageType(typeValue))
                {
                    switch ((CodePageType)typeValue)
                    {
                        case CodePageType.PtypCodePageUnicode:
                            if (parser.RemainingBytes < blockLength)
                            {
                                Partial.PartialGetRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;

                                if (blockLength != 0)
                                {
                                    var wasOneMoreGetByteToRead = Partial.IsOneMoreGetByteToRead;
                                    if (Partial.IsOneMoreGetByteToRead)
                                    {
                                        var oneMoreGetByte = ParseT<byte>();
                                        string skippedChar = System.Text.Encoding.Unicode.GetString(new byte[] { Partial.OneMoreGetByte, oneMoreGetByte });
                                        SkippedBytes = Create(oneMoreGetByte.Size, oneMoreGetByte.Offset, $"SkippedBytes: {Partial.OneMoreGetByte:X2} {oneMoreGetByte.Data:X2} = \"{skippedChar}\"");
                                        Partial.IsOneMoreGetByteToRead = false;
                                    }

                                    if ((blockLength / 2) != 0)
                                    {
                                        ValueArray = new PtypString(blockLength);
                                        ValueArray.Parse(parser);
                                    }

                                    // If IsOneMoreGetByteToRead was true and we had an even number of bytes to read
                                    // then we skipped the first byte and have an extra byte to skip
                                    // If IsOneMoreGetByteToRead was false and we had an odd number of bytes to read
                                    // then there is an extra byte to skip
                                    if ((wasOneMoreGetByteToRead && blockLength % 2 == 0) ||
                                        (!wasOneMoreGetByteToRead && blockLength % 2 != 0))
                                    {
                                        BlockT<byte> oneMoreGetByte = ParseT<byte>();
                                        Partial.IsOneMoreGetByteToRead = true;
                                        Partial.OneMoreGetByte = oneMoreGetByte;
                                    }
                                }
                            }
                            else
                            {
                                if (Partial.IsOneMoreGetByteToRead)
                                {
                                    var oneMoreGetByte = ParseT<byte>();
                                    string skippedChar = System.Text.Encoding.Unicode.GetString(new byte[] { Partial.OneMoreGetByte, oneMoreGetByte });
                                    SkippedBytes = Create(oneMoreGetByte.Size, oneMoreGetByte.Offset, $"SkippedBytes: {Partial.OneMoreGetByte:X2} {oneMoreGetByte.Data:X2} = \"{skippedChar}\"");
                                    Partial.IsOneMoreGetByteToRead = false;
                                }

                                if ((blockLength / 2) != 0)
                                {
                                    ValueArray = new PtypString(blockLength);
                                    ValueArray.Parse(parser);
                                }
                            }

                            break;
                        case CodePageType.PtypCodePageUnicode52:
                            if (Length != null)
                            {
                                Length = ParseT<int>();
                                blockLength = Length;
                            }

                            if (parser.RemainingBytes < blockLength)
                            {
                                Partial.PartialGetRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;

                                if (blockLength != 0)
                                {
                                    var wasOneMoreGetByteToRead = Partial.IsOneMoreGetByteToRead;
                                    if (Partial.IsOneMoreGetByteToRead)
                                    {
                                        var oneMoreGetByte = ParseT<byte>();
                                        string skippedChar = System.Text.Encoding.Unicode.GetString(new byte[] { Partial.OneMoreGetByte, oneMoreGetByte });
                                        SkippedBytes = Create(oneMoreGetByte.Size, oneMoreGetByte.Offset, $"SkippedBytes: {Partial.OneMoreGetByte:X2} {oneMoreGetByte.Data:X2} = \"{skippedChar}\"");
                                        Partial.IsOneMoreGetByteToRead = false;
                                    }

                                    if ((blockLength / 2) != 0)
                                    {
                                        ValueArray = new PtypString(blockLength);
                                        ValueArray.Parse(parser);
                                    }

                                    // If IsOneMoreGetByteToRead was true and we had an even number of bytes to read
                                    // then we skipped the first byte and have an extra byte to skip
                                    // If IsOneMoreGetByteToRead was false and we had an odd number of bytes to read
                                    // then there is an extra byte to skip
                                    if ((wasOneMoreGetByteToRead && blockLength % 2 == 0) ||
                                        (!wasOneMoreGetByteToRead && blockLength % 2 != 0))
                                    {
                                        BlockT<byte> oneMoreGetByte = ParseT<byte>();
                                        Partial.IsOneMoreGetByteToRead = true;
                                        Partial.OneMoreGetByte = oneMoreGetByte;
                                    }
                                }
                            }
                            else
                            {
                                if (Partial.IsOneMoreGetByteToRead)
                                {
                                    var oneMoreGetByte = ParseT<byte>();
                                    string skippedChar = System.Text.Encoding.Unicode.GetString(new byte[] { Partial.OneMoreGetByte, oneMoreGetByte });
                                    SkippedBytes = Create(oneMoreGetByte.Size, oneMoreGetByte.Offset, $"SkippedBytes: {Partial.OneMoreGetByte:X2} {oneMoreGetByte.Data:X2} = \"{skippedChar}\"");
                                    Partial.IsOneMoreGetByteToRead = false;
                                }

                                if ((blockLength / 2) != 0)
                                {
                                    ValueArray = new PtypString(blockLength);
                                    ValueArray.Parse(parser);
                                }
                            }

                            break;
                        case CodePageType.PtypCodePageUnicodeBigendian:
                        case CodePageType.PtypCodePageWesternEuropean:
                            if (parser.RemainingBytes < blockLength)
                            {
                                Partial.PartialGetRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;
                            }

                            ValueArray = new PtypString8(blockLength);
                            ValueArray.Parse(parser);
                            break;
                        default:
                            if (parser.RemainingBytes < blockLength)
                            {
                                Partial.PartialGetRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;
                            }

                            ValueArray = new PtypString8(blockLength);
                            ValueArray.Parse(parser);
                            break;
                    }
                }
                else
                {
                    switch (typeValue)
                    {
                        case PropertyDataType.PtypString:
                            if (parser.RemainingBytes < blockLength)
                            {
                                Partial.PartialGetRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;

                                if (blockLength != 0)
                                {
                                    var wasOneMoreGetByteToRead = Partial.IsOneMoreGetByteToRead;
                                    if (Partial.IsOneMoreGetByteToRead)
                                    {
                                        var oneMoreGetByte = ParseT<byte>();
                                        string skippedChar = System.Text.Encoding.Unicode.GetString(new byte[] { Partial.OneMoreGetByte, oneMoreGetByte });
                                        SkippedBytes = Create(oneMoreGetByte.Size, oneMoreGetByte.Offset, $"SkippedBytes: {Partial.OneMoreGetByte:X2} {oneMoreGetByte.Data:X2} = \"{skippedChar}\"");
                                        Partial.IsOneMoreGetByteToRead = false;
                                    }

                                    if ((blockLength / 2) != 0)
                                    {
                                        ValueArray = new PtypString(blockLength);
                                        ValueArray.Parse(parser);
                                    }

                                    // If IsOneMoreGetByteToRead was true and we had an even number of bytes to read
                                    // then we skipped the first byte and have an extra byte to skip
                                    // If IsOneMoreGetByteToRead was false and we had an odd number of bytes to read
                                    // then there is an extra byte to skip
                                    if ((wasOneMoreGetByteToRead && blockLength % 2 == 0) ||
                                        (!wasOneMoreGetByteToRead && blockLength % 2 != 0))
                                    {
                                        BlockT<byte> oneMoreGetByte = ParseT<byte>();
                                        Partial.IsOneMoreGetByteToRead = true;
                                        Partial.OneMoreGetByte = oneMoreGetByte;
                                    }
                                }
                            }
                            else
                            {
                                if (Partial.IsOneMoreGetByteToRead)
                                {
                                    var oneMoreGetByte = ParseT<byte>();
                                    string skippedChar = System.Text.Encoding.Unicode.GetString(new byte[] { Partial.OneMoreGetByte, oneMoreGetByte });
                                    SkippedBytes = Create(oneMoreGetByte.Size, oneMoreGetByte.Offset, $"SkippedBytes: {Partial.OneMoreGetByte:X2} {oneMoreGetByte.Data:X2} = \"{skippedChar}\"");
                                    Partial.IsOneMoreGetByteToRead = false;
                                }

                                if ((blockLength / 2) != 0)
                                {
                                    ValueArray = new PtypString(blockLength);
                                    ValueArray.Parse(parser);
                                }
                            }

                            break;
                        case PropertyDataType.PtypString8:
                            if (parser.RemainingBytes < blockLength)
                            {
                                Partial.PartialGetRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;
                            }

                            ValueArray = new PtypString8(blockLength);
                            ValueArray.Parse(parser);
                            break;
                        case PropertyDataType.PtypBinary:
                        case PropertyDataType.PtypServerId:
                        case PropertyDataType.PtypObject_Or_PtypEmbeddedTable:
                            if (parser.RemainingBytes < blockLength)
                            {
                                Partial.PartialGetRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;
                            }

                            ValueArray = ParseBytes(blockLength);
                            break;
                        default:
                            if (parser.RemainingBytes < blockLength)
                            {
                                Partial.PartialGetRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;
                            }

                            ValueArray = ParseBytes(blockLength);
                            break;
                    }
                }
            }
        }

        protected override void ParseBlocks()
        {
            base.ParseBlocks();
            AddChild(Comment);
            Text = "VarPropTypePropValueGetPartial";
            AddChildBlockT(Length, "Length");
            AddChild(SkippedBytes);
            AddChild(ValueArray, $"ValueArray: {ValueArray}");
        }
    }
}
