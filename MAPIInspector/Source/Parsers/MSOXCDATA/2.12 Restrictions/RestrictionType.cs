using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCDATA] 2.12 Restrictions
    /// </summary>
    public class RestrictionType : Block
    {
        /// <summary>
        /// The restriction
        /// </summary>
        public Block Restriction;

        /// <summary>
        /// The parsing context that determines count field widths.
        /// </summary>
        private PropertyCountContext context;

        /// <summary>
        /// Initializes a new instance of the RestrictionType class
        /// </summary>
        /// <param name="countContext">The parsing context that determines count field widths.</param>
        public RestrictionType(PropertyCountContext countContext = PropertyCountContext.RopBuffers)
        {
            context = countContext;
        }

        /// <summary>
        /// Parse the RestrictionType structure.
        /// </summary>
        protected override void Parse()
        {
            var restrictType = ParseT<RestrictTypeEnum>();
            if (!restrictType.Parsed) return;
            parser.Offset -= sizeof(RestrictTypeEnum);
            switch (restrictType.Data)
            {
                case RestrictTypeEnum.AndRestriction:
                    {
                        Restriction = new AndRestriction(context);
                        Restriction.Parse(parser);
                        break;
                    }

                case RestrictTypeEnum.OrRestriction:
                    {
                        Restriction = new OrRestriction(context);
                        Restriction.Parse(parser);
                        break;
                    }

                case RestrictTypeEnum.NotRestriction:
                    {
                        Restriction = new NotRestriction(context);
                        Restriction.Parse(parser);
                        break;
                    }

                case RestrictTypeEnum.ContentRestriction:
                    {
                        Restriction = new ContentRestriction(context);
                        Restriction.Parse(parser);
                        break;
                    }

                case RestrictTypeEnum.PropertyRestriction:
                    {
                        Restriction = new PropertyRestriction(context);
                        Restriction.Parse(parser);
                        break;
                    }

                case RestrictTypeEnum.ComparePropertiesRestriction:
                    {
                        Restriction = new ComparePropertiesRestriction(context);
                        Restriction.Parse(parser);
                        break;
                    }

                case RestrictTypeEnum.BitMaskRestriction:
                    {
                        Restriction = new BitMaskRestriction(context);
                        Restriction.Parse(parser);
                        break;
                    }

                case RestrictTypeEnum.SizeRestriction:
                    {
                        Restriction = new SizeRestriction();
                        Restriction.Parse(parser);
                        break;
                    }

                case RestrictTypeEnum.ExistRestriction:
                    {
                        Restriction = new ExistRestriction();
                        Restriction.Parse(parser);
                        break;
                    }

                case RestrictTypeEnum.CommentRestriction:
                    {
                        Restriction = new CommentRestriction(context);
                        Restriction.Parse(parser);
                        break;
                    }

                case RestrictTypeEnum.CountRestriction:
                    {
                        Restriction = new CountRestriction(context);
                        Restriction.Parse(parser);
                        break;
                    }

                default:
                    break;
            }
        }

        protected override void ParseBlocks()
        {
            Text = "Restriction";
            AddLabeledChild(Restriction, "Restriction");
        }
    }
}
