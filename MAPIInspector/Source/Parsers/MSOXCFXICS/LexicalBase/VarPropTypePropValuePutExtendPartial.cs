using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The VarPropTypePropValuePutExtendPartial class.
    /// </summary>
    public class VarPropTypePropValuePutExtendPartial : PropValue
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
                Partial.PartialPutExtendType = PropType;
                Partial.PartialPutExtendServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                Partial.PartialPutExtendProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                Partial.PartialPutExtendClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (Partial.PartialPutExtendType != 0 &&
                    Partial.PartialPutExtendServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath &&
                    Partial.PartialPutExtendProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess &&
                    Partial.PartialPutExtendClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    Comment = Partial.CreatePartialComment();

                    ptype = CreateBlock(Partial.PartialPutExtendType, 0, 0);

                    if (Partial.PartialPutExtendRemainSize != -1)
                    {
                        plength = Partial.PartialPutExtendRemainSize;

                        if (plength % 2 != 0 &&
                            (ptype == PropertyDataType.PtypString ||
                            ptype == (PropertyDataType)CodePageType.PtypCodePageUnicode ||
                            ptype == (PropertyDataType)CodePageType.PtypCodePageUnicode52))
                        {
                            Partial.IsOneMorePutExtendByteToRead = true;
                        }

                        Partial.PartialPutExtendRemainSize = -1;
                    }
                    else
                    {
                        Length = ParseT<int>();
                    }

                    // clear
                    Partial.PartialPutExtendType = 0;

                    if (Partial.PartialPutExtendRemainSize == -1)
                    {
                        Partial.PartialPutExtendServerUrl = string.Empty;
                        Partial.PartialPutExtendProcessName = string.Empty;
                        Partial.PartialPutExtendClientInfo = string.Empty;
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
                    Partial.PartialPutExtendType = typeValue;
                    Partial.PartialPutExtendServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                    Partial.PartialPutExtendProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                    Partial.PartialPutExtendClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
                }

                if (LexicalTypeHelper.IsCodePageType(typeValue))
                {
                    switch ((CodePageType)typeValue)
                    {
                        case CodePageType.PtypCodePageUnicode:
                            if (parser.RemainingBytes < blockLength)
                            {
                                Partial.PartialPutExtendRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;

                                if (blockLength != 0)
                                {
                                    var wasOneMorePutExtendByteToRead = Partial.IsOneMorePutExtendByteToRead;
                                    if (Partial.IsOneMorePutExtendByteToRead)
                                    {
                                        var oneMorePutExtendByte = ParseT<byte>();
                                        string skippedChar = System.Text.Encoding.Unicode.GetString(new byte[] { Partial.OneMorePutExtendByte, oneMorePutExtendByte });
                                        SkippedBytes = Create(oneMorePutExtendByte.Size, oneMorePutExtendByte.Offset, $"SkippedBytes: {Partial.OneMorePutExtendByte:X2} {oneMorePutExtendByte.Data:X2} = \"{skippedChar}\"");
                                        Partial.IsOneMorePutExtendByteToRead = false;
                                    }

                                    if ((blockLength / 2) != 0)
                                    {
                                        ValueArray = new PtypString(blockLength);
                                        ValueArray.Parse(parser);
                                    }

                                    // If IsOneMorePutExtendByteToRead was true and we had an even number of bytes to read
                                    // then we skipped the first byte and have an extra byte to skip
                                    // If IsOneMorePutExtendByteToRead was false and we had an odd number of bytes to read
                                    // then there is an extra byte to skip
                                    if ((wasOneMorePutExtendByteToRead && blockLength % 2 == 0) ||
                                        (!wasOneMorePutExtendByteToRead && blockLength % 2 != 0))
                                    {
                                        BlockT<byte> OneMorePutExtendByte = ParseT<byte>();
                                        Partial.IsOneMorePutExtendByteToRead = true;
                                        Partial.OneMorePutExtendByte = OneMorePutExtendByte;
                                    }
                                }
                            }
                            else
                            {
                                if (Partial.IsOneMorePutExtendByteToRead)
                                {
                                    var oneMorePutExtendByte = ParseT<byte>();
                                    string skippedChar = System.Text.Encoding.Unicode.GetString(new byte[] { Partial.OneMorePutExtendByte, oneMorePutExtendByte });
                                    SkippedBytes = Create(oneMorePutExtendByte.Size, oneMorePutExtendByte.Offset, $"SkippedBytes: {Partial.OneMorePutExtendByte:X2} {oneMorePutExtendByte.Data:X2} = \"{skippedChar}\"");
                                    Partial.IsOneMorePutExtendByteToRead = false;
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
                                Partial.PartialPutExtendRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;

                                if (blockLength != 0)
                                {
                                    var wasOneMorePutExtendByteToRead = Partial.IsOneMorePutExtendByteToRead;
                                    if (Partial.IsOneMorePutExtendByteToRead)
                                    {
                                        var oneMorePutExtendByte = ParseT<byte>();
                                        string skippedChar = System.Text.Encoding.Unicode.GetString(new byte[] { Partial.OneMorePutExtendByte, oneMorePutExtendByte });
                                        SkippedBytes = Create(oneMorePutExtendByte.Size, oneMorePutExtendByte.Offset, $"SkippedBytes: {Partial.OneMorePutExtendByte:X2} {oneMorePutExtendByte.Data:X2} = \"{skippedChar}\"");
                                        Partial.IsOneMorePutExtendByteToRead = false;
                                    }


                                    if ((blockLength / 2) != 0)
                                    {
                                        ValueArray = new PtypString(blockLength);
                                        ValueArray.Parse(parser);
                                    }

                                    // If IsOneMorePutExtendByteToRead was true and we had an even number of bytes to read
                                    // then we skipped the first byte and have an extra byte to skip
                                    // If IsOneMorePutExtendByteToRead was false and we had an odd number of bytes to read
                                    // then there is an extra byte to skip
                                    if ((wasOneMorePutExtendByteToRead && blockLength % 2 == 0) ||
                                        (!wasOneMorePutExtendByteToRead && blockLength % 2 != 0))
                                    {
                                        BlockT<byte> oneMorePutExtendByte = ParseT<byte>();
                                        Partial.IsOneMorePutExtendByteToRead = true;
                                        Partial.OneMorePutExtendByte = oneMorePutExtendByte;
                                    }
                                }
                            }
                            else
                            {
                                if (Partial.IsOneMorePutExtendByteToRead)
                                {
                                    var oneMorePutExtendByte = ParseT<byte>();
                                    string skippedChar = System.Text.Encoding.Unicode.GetString(new byte[] { Partial.OneMorePutExtendByte, oneMorePutExtendByte });
                                    SkippedBytes = Create(oneMorePutExtendByte.Size, oneMorePutExtendByte.Offset, $"SkippedBytes: {Partial.OneMorePutExtendByte:X2} {oneMorePutExtendByte.Data:X2} = \"{skippedChar}\"");
                                    Partial.IsOneMorePutExtendByteToRead = false;
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
                                Partial.PartialPutExtendRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;
                            }

                            ValueArray = new PtypString8(blockLength);
                            ValueArray.Parse(parser);
                            break;
                        default:
                            if (parser.RemainingBytes < blockLength)
                            {
                                Partial.PartialPutExtendRemainSize = blockLength - parser.RemainingBytes;
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
                                Partial.PartialPutExtendRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;

                                if (blockLength != 0)
                                {
                                    var wasOneMorePutExtendByteToRead = Partial.IsOneMorePutExtendByteToRead;
                                    if (Partial.IsOneMorePutExtendByteToRead)
                                    {
                                        var oneMorePutExtendByte = ParseT<byte>();
                                        string skippedChar = System.Text.Encoding.Unicode.GetString(new byte[] { Partial.OneMorePutExtendByte, oneMorePutExtendByte });
                                        SkippedBytes = Create(oneMorePutExtendByte.Size, oneMorePutExtendByte.Offset, $"SkippedBytes: {Partial.OneMorePutExtendByte:X2} {oneMorePutExtendByte.Data:X2} = \"{skippedChar}\"");
                                        Partial.IsOneMorePutExtendByteToRead = false;
                                    }


                                    if ((blockLength / 2) != 0)
                                    {
                                        ValueArray = new PtypString(blockLength);
                                        ValueArray.Parse(parser);
                                    }

                                    // If IsOneMorePutExtendByteToRead was true and we had an even number of bytes to read
                                    // then we skipped the first byte and have an extra byte to skip
                                    // If IsOneMorePutExtendByteToRead was false and we had an odd number of bytes to read
                                    // then there is an extra byte to skip
                                    if ((wasOneMorePutExtendByteToRead && blockLength % 2 == 0) ||
                                        (!wasOneMorePutExtendByteToRead && blockLength % 2 != 0))
                                    {
                                        BlockT<byte> oneMorePutExtendByte = ParseT<byte>();
                                        Partial.IsOneMorePutExtendByteToRead = true;
                                        Partial.OneMorePutExtendByte = oneMorePutExtendByte;
                                    }
                                }
                            }
                            else
                            {
                                if (Partial.IsOneMorePutExtendByteToRead)
                                {
                                    var oneMorePutExtendByte = ParseT<byte>();
                                    string skippedChar = System.Text.Encoding.Unicode.GetString(new byte[] { Partial.OneMorePutExtendByte, oneMorePutExtendByte });
                                    SkippedBytes = Create(oneMorePutExtendByte.Size, oneMorePutExtendByte.Offset, $"SkippedBytes: {Partial.OneMorePutExtendByte:X2} {oneMorePutExtendByte.Data:X2} = \"{skippedChar}\"");
                                    Partial.IsOneMorePutExtendByteToRead = false;
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
                                Partial.PartialPutExtendRemainSize = blockLength - parser.RemainingBytes;
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
                                Partial.PartialPutExtendRemainSize = blockLength - parser.RemainingBytes;
                                plength = parser.RemainingBytes;
                                blockLength = plength;
                            }

                            ValueArray = ParseBytes(blockLength);
                            break;
                        default:
                            if (parser.RemainingBytes < blockLength)
                            {
                                Partial.PartialPutExtendRemainSize = blockLength - parser.RemainingBytes;
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
            Text = "VarPropTypePropValuePutExtendPartial";
            AddChildBlockT(Length, "Length");
            AddChild(SkippedBytes);
            AddChild(ValueArray, $"ValueArray: {ValueArray}");
        }
    }
}
