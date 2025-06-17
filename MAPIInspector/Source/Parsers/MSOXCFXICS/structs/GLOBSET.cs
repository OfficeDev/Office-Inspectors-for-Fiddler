namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.Collections.Generic;

    /// <summary>
    /// Represents GLOBSET structure is a set of GLOBCNT structures, that are reduced to one or more GLOBCNT ranges. A GLOBCNT range is created using any of the commands  
    /// 2.2.2.6 GLOBSET Structure
    /// </summary>
    public class GLOBSET : Block
    {
        /// <summary>
        /// Commands composed a GLOBCNT range, which indicates a GLOBSET structure.
        /// </summary>
        public Command[] Commands;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        protected override void Parse()
        {
            // A UInt value indicates the bytes length in common stacks.
            uint commonStackLength = 0;

            // A UInt list indicates the pushed or popped count of bytes in common stack.
            var commonStackCollection = new List<uint>();

            var tmp = BlockParser.BlockT<byte>.TestParse(parser);

            var commands = new List<Command>();
            while (tmp.Parsed && tmp.Data != 0x00)
            {
                switch (tmp.Data)
                {
                    case 0x01:
                    case 0x02:
                    case 0x03:
                    case 0x04:
                    case 0x05:
                    case 0x06:
                        var pushCommand = Parse<PushCommand>(parser);
                        commands.Add(pushCommand);
                        if ((commonStackLength + pushCommand.Command.Data) < 6)
                        {
                            commonStackCollection.Add(pushCommand.Command.Data);
                            commonStackLength += pushCommand.Command.Data;
                        }

                        break;
                    case 0x50:
                        commands.Add(Parse<PopCommand>(parser));
                        commonStackLength -= commonStackCollection[commonStackCollection.Count - 1];
                        commonStackCollection.RemoveAt(commonStackCollection.Count - 1);
                        break;
                    case 0x42:
                        commands.Add(Parse<BitmaskCommand>(parser));
                        break;
                    case 0x52:
                        var rangeCommand = new RangeCommand(6 - commonStackLength);
                        rangeCommand.Parse(parser);
                        commands.Add(rangeCommand);
                        break;
                    default:
                        break;
                }

                tmp = BlockParser.BlockT<byte>.TestParse(parser);
            }

            commands.Add(Parse<EndCommand>(parser));
            Commands = commands.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("GLOBSET");
            foreach (var command in Commands)
            {
                AddChild(command);
            }
        }
    }
}
