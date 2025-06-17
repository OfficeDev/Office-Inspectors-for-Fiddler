using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The MetaPropValue represents identification information and the value of the Meta property.
    /// </summary>
    public class MetaPropValue : Block
    {
        /// <summary>
        /// The property type.
        /// </summary>
        public BlockT<PropertyDataType> PropType;

        /// <summary>
        /// The property id.
        /// </summary>
        public BlockT<PidTagPropertyEnum> PropID;

        /// <summary>
        /// The property value.
        /// </summary>
        public Block PropValue;

        protected override void Parse()
        {
            PropType = BlockT<PropertyDataType>.Parse(parser);
            PropID = BlockT<PidTagPropertyEnum>.Parse(parser);

            if (PropID.Data != PidTagPropertyEnum.MetaTagNewFXFolder &&
                PropID.Data != PidTagPropertyEnum.MetaTagDnPrefix)
            {
                PropValue = BlockT<int>.Parse(parser);
            }
            else
            {
                if (PropID.Data != PidTagPropertyEnum.MetaTagNewFXFolder)
                {
                    PropValue = Parse<FolderReplicaInfo>(parser);
                }
                else
                {
                    PropValue = Parse<PtypString8>(parser);
                }
            }
        }

        protected override void ParseBlocks()
        {
            SetText("MetaPropValue");
            AddLabeledChild("PropType", PropType);
            if (PropID != null) AddChild(PropID, $"PropID:{MapiInspector.Utilities.EnumToString(PropID.Data)}");
            AddLabeledChild("PropValue", PropValue);
        }
    }
}
