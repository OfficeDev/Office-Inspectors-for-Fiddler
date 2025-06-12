namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Syntactical markers
    /// </summary>
    public enum Markers : uint
    {
        /// <summary>
        /// StartTopFld marker 
        /// </summary>
        StartTopFld = 0x40090003,

        /// <summary>
        /// EndFolder marker
        /// </summary>
        EndFolder = 0x400B0003,

        /// <summary>
        /// StartSubFld marker
        /// </summary>
        StartSubFld = 0x400A0003,

        /// <summary>
        /// StartMessage marker
        /// </summary>
        StartMessage = 0x400C0003,

        /// <summary>
        /// EndMessage marker 
        /// </summary>
        EndMessage = 0x400D0003,

        /// <summary>
        /// StartFAIMsg marker
        /// </summary>
        StartFAIMsg = 0x40100003,

        /// <summary>
        /// StartEmbed marker
        /// </summary>
        StartEmbed = 0x40010003,

        /// <summary>
        /// EndEmbed marker
        /// </summary>
        EndEmbed = 0x40020003,

        /// <summary>
        /// StartRecip marker
        /// </summary>
        StartRecip = 0x40030003,

        /// <summary>
        /// EndToRecip marker
        /// </summary>
        EndToRecip = 0x40040003,

        /// <summary>
        /// NewAttach marker
        /// </summary>
        NewAttach = 0x40000003,

        /// <summary>
        /// EndAttach marker
        /// </summary>
        EndAttach = 0x400E0003,

        /// <summary>
        /// IncrSyncChg marker
        /// </summary>
        IncrSyncChg = 0x40120003,

        /// <summary>
        /// IncrSyncChgPartial marker
        /// </summary>
        IncrSyncChgPartial = 0x407D0003,

        /// <summary>
        /// IncrSyncDel marker
        /// </summary>
        IncrSyncDel = 0x40130003,

        /// <summary>
        /// IncrSyncEnd marker
        /// </summary>
        IncrSyncEnd = 0x40140003,

        /// <summary>
        /// IncrSyncRead marker
        /// </summary>
        IncrSyncRead = 0x402F0003,

        /// <summary>
        /// IncrSyncStateBegin marker
        /// </summary>
        IncrSyncStateBegin = 0x403A0003,

        /// <summary>
        /// IncrSyncStateEnd marker
        /// </summary>
        IncrSyncStateEnd = 0x403B0003,

        /// <summary>
        /// IncrSyncProgressMode marker
        /// </summary>
        IncrSyncProgressMode = 0x4074000B,

        /// <summary>
        /// IncrSyncProgressPerMsg marker
        /// </summary>
        IncrSyncProgressPerMsg = 0x4075000B,

        /// <summary>
        /// IncrSyncMessage marker
        /// </summary>
        IncrSyncMessage = 0x40150003,

        /// <summary>
        /// IncrSyncGroupInfo marker
        /// </summary>
        IncrSyncGroupInfo = 0x407B0102,

        /// <summary>
        /// FXErrorInfo marker
        /// </summary>
        FXErrorInfo = 0x40180003,
    }
}
