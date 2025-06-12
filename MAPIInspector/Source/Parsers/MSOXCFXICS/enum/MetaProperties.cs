namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Meta properties
    /// 2.2.4.1.5 Meta-Properties
    /// </summary>
    public enum MetaProperties : uint
    {
        /// <summary>
        /// The MetaTagDnPrefix meta-property MUST be ignored when received
        /// </summary>
        MetaTagDnPrefix = 0x4008001E,

        /// <summary>
        /// The MetaTagEcWarning meta-property contains a warning that occurred when producing output for an element in context
        /// </summary>
        MetaTagEcWarning = 0x400f0003,

        /// <summary>
        /// The MetaTagNewFXFolder meta-property provides information about alternative replicas (1) for a public folder in context
        /// </summary>
        MetaTagNewFXFolder = 0x40110102,

        /// <summary>
        /// The MetaTagFXDelProp meta-property represents a directive to a client to delete specific subobjects of the object in context
        /// </summary>
        MetaTagFXDelProp = 0x40160003,

        /// <summary>
        /// The MetaTagIncrementalSyncMessagePartial meta-property specifies an index of a property group within a property group mapping currently in context
        /// </summary>
        MetaTagIncrementalSyncMessagePartial = 0x407a0003,

        /// <summary>
        /// The MetaTagIncrSyncGroupId meta-property specifies an identifier of a property group mapping
        /// </summary>
        MetaTagIncrSyncGroupId = 0x407c0003
    }
}
