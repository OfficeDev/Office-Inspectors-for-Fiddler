namespace MAPIInspector.Parsers
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents GLOBSET structure is a set of GLOBCNT structures, that are reduced to one or more GLOBCNT ranges. A GLOBCNT range is created using any of the commands  
    /// 2.2.2.6 GLOBSET Structure
    /// </summary>
    public class GLOBSET : BaseStructure
    {
        /// <summary>
        /// Commands composed a GLOBCNT range, which indicates a GLOBSET structure.
        /// </summary>
        public Command[] Commands;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains GLOBSET.</param>
        public void Parse(FastTransferStream stream)
        {
            // A UInt value indicates the bytes length in common stacks.
            uint commonStackLength = 0;

            // A UInt list indicates the pushed or popped count of bytes in common stack.
            List<uint> commonStackCollection = new List<uint>();

            byte tmp = stream.ReadByte();
            stream.Position -= 1;

            List<Command> commands = new List<Command>();
            while (tmp != 0X00)
            {
                switch (tmp)
                {
                    case 0x01:
                    case 0x02:
                    case 0x03:
                    case 0x04:
                    case 0x05:
                    case 0x06:
                        Command pushCommand = new PushCommand();
                        pushCommand.Parse(stream);
                        commands.Add(pushCommand);
                        if ((commonStackLength + (uint)(pushCommand as PushCommand).Command) < 6)
                        {
                            commonStackCollection.Add((pushCommand as PushCommand).Command);
                            commonStackLength += (uint)(pushCommand as PushCommand).Command;
                        }

                        break;
                    case 0x50:
                        Command popCommand = new PopCommand();
                        popCommand.Parse(stream);
                        commands.Add(popCommand);
                        commonStackLength -= commonStackCollection[commonStackCollection.Count - 1];
                        commonStackCollection.RemoveAt(commonStackCollection.Count - 1);
                        break;
                    case 0x42:
                        Command bitmaskCommand = new BitmaskCommand();
                        bitmaskCommand.Parse(stream);
                        commands.Add(bitmaskCommand);
                        break;
                    case 0x52:
                        Command rangeCommand = new RangeCommand(6 - commonStackLength);
                        rangeCommand.Parse(stream);
                        commands.Add(rangeCommand);
                        break;
                    default:
                        break;
                }

                tmp = stream.ReadByte();
                stream.Position -= 1;
            }

            Command endCommand = new EndCommand();
            endCommand.Parse(stream);
            commands.Add(endCommand);
            this.Commands = commands.ToArray();
        }
    }
}
