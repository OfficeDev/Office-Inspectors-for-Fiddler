namespace MAPIInspector.Parsers
{
    using BlockParser;

    /// <summary>
    ///  2.12   Restrictions
    /// </summary>
    public class RestrictionType : Block
    {
        /// <summary>
        /// The restriction
        /// </summary>
        public Block Restriction;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the RestrictionType class
        /// </summary>
        /// <param name="ptypMultiCountSize">The Count wide size of ptypMutiple type.</param>
        public RestrictionType(CountWideEnum ptypMultiCountSize = CountWideEnum.twoBytes)
        {
            countWide = ptypMultiCountSize;
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
                        var restriction = new AndRestriction(countWide);
                        restriction.Parse(parser);
                        Restriction = restriction;
                        break;
                    }

                case RestrictTypeEnum.OrRestriction:
                    {
                        var restriction = new OrRestriction(countWide);
                        restriction.Parse(parser);
                        Restriction = restriction;
                        break;
                    }

                case RestrictTypeEnum.NotRestriction:
                    {
                        var restriction = new NotRestriction(countWide);
                        restriction.Parse(parser);
                        Restriction = restriction;
                        break;
                    }

                case RestrictTypeEnum.ContentRestriction:
                    {
                        var restriction = new ContentRestriction(countWide);
                        restriction.Parse(parser);
                        Restriction = restriction;
                        break;
                    }

                case RestrictTypeEnum.PropertyRestriction:
                    {
                        var restriction = new PropertyRestriction(countWide);
                        restriction.Parse(parser);
                        Restriction = restriction;
                        break;
                    }

                case RestrictTypeEnum.ComparePropertiesRestriction:
                    {
                        var restriction = new ComparePropertiesRestriction();
                        restriction.Parse(parser);
                        Restriction = restriction;
                        break;
                    }

                case RestrictTypeEnum.BitMaskRestriction:
                    {
                        var restriction = new BitMaskRestriction();
                        restriction.Parse(parser);
                        Restriction = restriction;
                        break;
                    }

                case RestrictTypeEnum.SizeRestriction:
                    {
                        var restriction = new SizeRestriction();
                        restriction.Parse(parser);
                        Restriction = restriction;
                        break;
                    }

                case RestrictTypeEnum.ExistRestriction:
                    {
                        var restriction = new ExistRestriction();
                        restriction.Parse(parser);
                        Restriction = restriction;
                        break;
                    }

                case RestrictTypeEnum.SubObjectRestriction:
                    {
                        var restriction = new SubObjectRestriction(countWide);
                        restriction.Parse(parser);
                        Restriction = restriction;
                        break;
                    }

                case RestrictTypeEnum.CommentRestriction:
                    {
                        var restriction = new CommentRestriction(countWide);
                        restriction.Parse(parser);
                        Restriction = restriction;
                        break;
                    }

                case RestrictTypeEnum.CountRestriction:
                    {
                        var restriction = new CountRestriction(countWide);
                        restriction.Parse(parser);
                        Restriction = restriction;
                        break;
                    }

                default:
                    break;
            }
        }

        protected override void ParseBlocks()
        {
            SetText("Restriction");
            AddLabeledChild(Restriction, "Restriction");
        }
    }
}
