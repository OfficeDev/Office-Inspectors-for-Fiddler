using BlockParser;
using System;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Supply help functions for manipulating Markers.
    /// </summary>
    public class MarkersHelper
    {
        /// <summary>
        /// Indicate whether a UInt is a Marker.
        /// </summary>
        /// <param name="marker">The UInts value.</param>
        /// <returns>If is a Marker, return true, else false.</returns>
        public static bool IsMarker(Markers marker)
        {
            foreach (Markers ma in Enum.GetValues(typeof(Markers)))
            {
                if (ma == marker)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Indicate whether a UInt is a MetaProperties.
        /// </summary>
        /// <param name="metaTag">The UInts value.</param>
        /// <returns>If is a MetaProperties, return true, else false.</returns>
        public static bool IsMetaTag(MetaProperties metaTag)
        {
            foreach (MetaProperties me in Enum.GetValues(typeof(MetaProperties)))
            {
                if (metaTag == me)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Indicate the MetaProperties at the position equals a specified MetaProperties
        /// </summary>
        /// <param name="meta">A MetaProperties value</param>
        /// <returns>True if the MetaProperties at the position equals the specified MetaProperties, else false.</returns>
        public static bool VerifyMetaProperty(BinaryParser parser, MetaProperties meta)
        {
            return !parser.Empty && Verify(parser, (uint)meta);
        }

        /// <summary>
        /// Indicate the Markers at the position equals a specified Markers.
        /// </summary>
        /// <param name="marker">A Markers value</param>
        /// <returns>True if the Markers at the position equals to the specified Markers, else false.</returns>
        public static bool VerifyMarker(BinaryParser parser, Markers marker)
        {
            return Verify(parser, (uint)marker);
        }

        /// <summary>
        /// Indicate the UInt value at the position plus an offset equals a specified UInt value.
        /// </summary>
        /// <param name="val">A UInt value</param>
        /// <param name="offset">An int value</param>
        /// <returns>True if the UInt at the position plus an offset equals the specified UInt,else false.</returns>
        private static bool Verify(BinaryParser parser, uint val)
        {
            var bufferVal = Block.TestParse<uint>(parser);
            return bufferVal.Parsed && bufferVal == val;
        }
    }
}
