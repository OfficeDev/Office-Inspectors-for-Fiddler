namespace MAPIInspector.Parsers
{
    using System;

    /// <summary>
    /// Supply help functions for manipulate Markers.
    /// </summary>
    public class MarkersHelper
    {
        /// <summary>
        /// Indicate whether a UInt is a Marker.
        /// </summary>
        /// <param name="marker">The UInts value.</param>
        /// <returns>If is a Marker, return true, else false.</returns>
        public static bool IsMarker(uint marker)
        {
            foreach (Markers ma in Enum.GetValues(typeof(Markers)))
            {
                if ((uint)ma == marker)
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
        public static bool IsMetaTag(uint metaTag)
        {
            foreach (MetaProperties me in Enum.GetValues(typeof(MetaProperties)))
            {
                if (metaTag == (uint)me)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
