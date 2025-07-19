using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The VarPropTypePropValue class.
    /// </summary>
    public class VarPropTypePropValuePutPartial : PropValue
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
                Partial.PartialPutType = PropType;
                Partial.PartialPutServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                Partial.PartialPutProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                Partial.PartialPutClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (Partial.PartialPutType != 0 &&
                    Partial.PartialPutServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                    Partial.PartialPutProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                    Partial.PartialPutClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    Comment = Partial.CreatePartialComment();

                    ptype = CreateBlock(Partial.PartialPutType, 0, 0);

                    if (Partial.PartialPutRemainSize != -1)
                    {
                        plength = Partial.PartialPutRemainSize;

                        if (plength % 2 != 0 &&
                            (ptype == PropertyDataType.PtypString ||
                            ptype == (PropertyDataType)CodePageType.PtypCodePageUnicode ||
                            ptype == (PropertyDataType)CodePageType.PtypCodePageUnicode52))
                        {
                            Partial.IsOneMorePutByteToRead = true;
                        }

                        Partial.PartialPutRemainSize = -1;
                    }
                    else
                    {
                        Length = ParseT<int>();
                    }

                    // clear
                    Partial.PartialPutType = 0;

                    if (Partial.PartialPutRemainSize == -1)
                    {
                        Partial.PartialPutServerUrl = string.Empty;
                        Partial.PartialPutProcessName = string.Empty;
                        Partial.PartialPutClientInfo = string.Empty;
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
                    Partial.PartialPutType = typeValue;
                    Partial.PartialPutServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                    Partial.PartialPutProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                    Partial.PartialPutClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
                }

                if (LexicalTypeHelper.IsCodePageType(typeValue))
                {
                    switch ((CodePageType)typeValue)
                    {
                        case CodePageType.PtypCodePageUnicode:
                            if (parser.RemainingBytes < blockLength)
                            {
                                Partial.PartialPutRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;

                                if (blockLength != 0)
                                {
                                    var wasOneMorePutByteToRead = Partial.IsOneMorePutByteToRead;
                                    if (Partial.IsOneMorePutByteToRead)
                                    {
                                        var oneMorePutByte = ParseT<byte>();
                                        string skippedChar = System.Text.Encoding.Unicode.GetString(new byte[] { Partial.OneMorePutByte, oneMorePutByte });
                                        SkippedBytes = Create(oneMorePutByte.Size, oneMorePutByte.Offset, $"SkippedBytes: {Partial.OneMorePutByte:X2} {oneMorePutByte.Data:X2} = \"{skippedChar}\"");
                                        Partial.IsOneMorePutByteToRead = false;
                                    }

                                    if ((blockLength / 2) != 0)
                                    {
                                        ValueArray = new PtypString(blockLength);
                                        ValueArray.Parse(parser);
                                    }

                                    // If IsOneMorePutByteToRead was true and we had an even number of bytes to read
                                    // then we skipped the first byte and have an extra byte to skip
                                    // If IsOneMorePutByteToRead was false and we had an odd number of bytes to read
                                    // then there is an extra byte to skip
                                    if ((wasOneMorePutByteToRead && blockLength % 2 == 0) ||
                                        (!wasOneMorePutByteToRead && blockLength % 2 != 0))
                                    {
                                        BlockT<byte> OneMorePutByte = ParseT<byte>();
                                        Partial.IsOneMorePutByteToRead = true;
                                        Partial.OneMorePutByte = OneMorePutByte;
                                    }
                                }
                            }
                            else
                            {
                                if (Partial.IsOneMorePutByteToRead)
                                {
                                    var oneMorePutByte = ParseT<byte>();
                                    string skippedChar = System.Text.Encoding.Unicode.GetString(new byte[] { Partial.OneMorePutByte, oneMorePutByte });
                                    SkippedBytes = Create(oneMorePutByte.Size, oneMorePutByte.Offset, $"SkippedBytes: {Partial.OneMorePutByte:X2} {oneMorePutByte.Data:X2} = \"{skippedChar}\"");
                                    Partial.IsOneMorePutByteToRead = false;
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
                                Partial.PartialPutRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;

                                if (blockLength != 0)
                                {
                                    var wasOneMorePutByteToRead = Partial.IsOneMorePutByteToRead;
                                    if (Partial.IsOneMorePutByteToRead)
                                    {
                                        var oneMorePutByte = ParseT<byte>();
                                        string skippedChar = System.Text.Encoding.Unicode.GetString(new byte[] { Partial.OneMorePutByte, oneMorePutByte });
                                        SkippedBytes = Create(oneMorePutByte.Size, oneMorePutByte.Offset, $"SkippedBytes: {Partial.OneMorePutByte:X2} {oneMorePutByte.Data:X2} = \"{skippedChar}\"");
                                        Partial.IsOneMorePutByteToRead = false;
                                    }

                                    if ((blockLength / 2) != 0)
                                    {
                                        ValueArray = new PtypString(blockLength);
                                        ValueArray.Parse(parser);
                                    }

                                    // If IsOneMorePutByteToRead was true and we had an even number of bytes to read
                                    // then we skipped the first byte and have an extra byte to skip
                                    // If IsOneMorePutByteToRead was false and we had an odd number of bytes to read
                                    // then there is an extra byte to skip
                                    if ((wasOneMorePutByteToRead && blockLength % 2 == 0) ||
                                        (!wasOneMorePutByteToRead && blockLength % 2 != 0))
                                    {
                                        BlockT<byte> oneMorePutByte = ParseT<byte>();
                                        Partial.IsOneMorePutByteToRead = true;
                                        Partial.OneMorePutByte = oneMorePutByte;
                                    }
                                }
                            }
                            else
                            {
                                if (Partial.IsOneMorePutByteToRead)
                                {
                                    var oneMorePutByte = ParseT<byte>();
                                    string skippedChar = System.Text.Encoding.Unicode.GetString(new byte[] { Partial.OneMorePutByte, oneMorePutByte });
                                    SkippedBytes = Create(oneMorePutByte.Size, oneMorePutByte.Offset, $"SkippedBytes: {Partial.OneMorePutByte:X2} {oneMorePutByte.Data:X2} = \"{skippedChar}\"");
                                    Partial.IsOneMorePutByteToRead = false;
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
                                Partial.PartialPutRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;
                            }

                            ValueArray = new PtypString8(blockLength);
                            ValueArray.Parse(parser);
                            break;
                        default:
                            if (parser.RemainingBytes < blockLength)
                            {
                                Partial.PartialPutRemainSize = blockLength - parser.RemainingBytes;
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
                                Partial.PartialPutRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;

                                if (blockLength != 0)
                                {
                                    var wasOneMorePutByteToRead = Partial.IsOneMorePutByteToRead;
                                    if (Partial.IsOneMorePutByteToRead)
                                    {
                                        var oneMorePutByte = ParseT<byte>();
                                        string skippedChar = System.Text.Encoding.Unicode.GetString(new byte[] { Partial.OneMorePutByte, oneMorePutByte });
                                        SkippedBytes = Create(oneMorePutByte.Size, oneMorePutByte.Offset, $"SkippedBytes: {Partial.OneMorePutByte:X2} {oneMorePutByte.Data:X2} = \"{skippedChar}\"");
                                        Partial.IsOneMorePutByteToRead = false;
                                    }

                                    if ((blockLength / 2) != 0)
                                    {
                                        ValueArray = new PtypString(blockLength);
                                        ValueArray.Parse(parser);
                                    }

                                    // If IsOneMorePutByteToRead was true and we had an even number of bytes to read
                                    // then we skipped the first byte and have an extra byte to skip
                                    // If IsOneMorePutByteToRead was false and we had an odd number of bytes to read
                                    // then there is an extra byte to skip
                                    if ((wasOneMorePutByteToRead && blockLength % 2 == 0) ||
                                        (!wasOneMorePutByteToRead && blockLength % 2 != 0))
                                    {
                                        BlockT<byte> oneMorePutByte = ParseT<byte>();
                                        Partial.IsOneMorePutByteToRead = true;
                                        Partial.OneMorePutByte = oneMorePutByte;
                                    }
                                }
                            }
                            else
                            {
                                if (Partial.IsOneMorePutByteToRead)
                                {
                                    var oneMorePutByte = ParseT<byte>();
                                    string skippedChar = System.Text.Encoding.Unicode.GetString(new byte[] { Partial.OneMorePutByte, oneMorePutByte });
                                    SkippedBytes = Create(oneMorePutByte.Size, oneMorePutByte.Offset, $"SkippedBytes: {Partial.OneMorePutByte:X2} {oneMorePutByte.Data:X2} = \"{skippedChar}\"");
                                    Partial.IsOneMorePutByteToRead = false;
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
                                Partial.PartialPutRemainSize = blockLength - parser.RemainingBytes;
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
                                Partial.PartialPutRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;
                            }

                            ValueArray = ParseBytes(blockLength);
                            break;
                        default:
                            if (parser.RemainingBytes < blockLength)
                            {
                                Partial.PartialPutRemainSize = blockLength - parser.RemainingBytes;
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
            SetText("VarPropTypePropValuePutPartial");
            AddChildBlockT(Length, "Length");
            AddChild(SkippedBytes);
            AddChild(ValueArray, $"ValueArray: {ValueArray}");
        }
    }
}
