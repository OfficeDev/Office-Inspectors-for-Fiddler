using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Contains a list of propValues.
    /// </summary>
    public class PropList : Block
    {
        /// <summary>
        /// A list of PropValue objects.
        /// </summary>
        public PropValue[] PropValues;

        protected override void Parse()
        {
            var propValuesList = new List<PropValue>();

            while (PropValue.Verify(parser))
            {
                propValuesList.Add(PropValue.ParseFrom(parser));
            }

            PropValues = propValuesList.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("PropList");
            AddLabeledChildren(PropValues, "PropValues");
        }
    }
}
