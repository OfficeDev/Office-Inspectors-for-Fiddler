using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The SyncMessagePartialPropList is used to parse MessageChangePartial element.
    /// </summary>
    public class SyncMessagePartialPropList : Block
    {
        /// <summary>
        /// A MetaTagIncrementalSyncMessagePartial property.
        /// </summary>
        public MetaPropValue MetaSyncMessagePartial;

        /// <summary>
        /// A PropList value.
        /// </summary>
        public PropList PropList;

        protected override void Parse()
        {
            if (MarkersHelper.VerifyMetaProperty(parser, MetaProperties.MetaTagIncrementalSyncMessagePartial))
            {
                MetaSyncMessagePartial = Parse<MetaPropValue>();
            }

            PropList = Parse<PropList>();
        }

        protected override void ParseBlocks()
        {
            Text = "SyncMessagePartialPropList";
            AddChild(MetaSyncMessagePartial, "MetaSyncMessagePartial");
            AddChild(PropList, "PropList");
        }
    }
}
