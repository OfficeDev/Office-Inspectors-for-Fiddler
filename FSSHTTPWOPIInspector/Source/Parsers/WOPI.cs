namespace FSSHTTPandWOPIInspector.Parsers
{
    using System.Xml.Serialization;
    using System.Runtime.Serialization;
    using System;
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Text;

    /// <summary>
    /// 3.3.5.1.2	CheckFileInfo
    /// </summary>
    public class CheckFileInfo : ResponseBodyBase
    {
        /// <remarks/>
        [DataMember(Order = 1)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string AADUserObjectId { get; set; }

        /// <remarks/>
        [DataMember(Order = 2)]
        [System.ComponentModel.DefaultValueAttribute(0)]
        public long AccessTokenExpiry { get; set; }

        /// <remarks/>
        [DataMember(Order =3)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool AllowAddActivitiesUserBatching { get; set; }

        /// <remarks/>
        [DataMember(Order = 4, EmitDefaultValue = false)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool AllowAdditionalMicrosoftServices { get; set; }

        /// <remarks/>
        [DataMember(Order =5)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool AllowEarlyFeatures { get; set; }

        /// <remarks/>
        [DataMember(Order =6)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool AllowErrorReportPrompt { get; set; }       
        
        /// <remarks/>
        [DataMember(Order = 7)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool AllowExternalMarketplace { get; set; }

        /// <remarks/>
        [DataMember(Order =8)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string AppCatalogUrl { get; set; }

        /// <remarks/>
        [DataMember(Order =9)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string AppliedPolicyId { get; set; }

        /// <remarks/>
        [DataMember(Order = 10, IsRequired = true)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string BaseFileName { get; set; }

        /// <remarks/>
        [DataMember(Order = 11)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string BreadcrumbBrandName { get; set; }

        /// <remarks/>
        [DataMember(Order = 12)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string BreadcrumbBrandUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 13)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string BreadcrumbDocName { get; set; }

        /// <remarks/>
        [DataMember(Order = 14)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string BreadcrumbDocUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 15)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string BreadcrumbFolderName { get; set; }

        /// <remarks/>
        [DataMember(Order = 16)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string BreadcrumbFolderUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 17)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string ClientUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 18)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool CloseButtonClosesWindow { get; set; }

        /// <remarks/>
        [DataMember(Order = 19)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool ClosePostMessage { get; set; }

        /// <remarks/>
        [DataMember(Order = 20)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string CloseUrl { get; set; }

        /// <remarks/>
        [DataMember(Order =21)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string DirectInvokeDAVUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 22)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool DisableBrowserCachingOfUserContent { get; set; }

        /// <remarks/>
        [DataMember(Order = 23)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool DisablePrint { get; set; }

        /// <remarks/>
        [DataMember(Order = 24)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool DisableTranslation { get; set; }

        /// <remarks/>
        [DataMember(Order = 25)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string DownloadUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 26)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string EditAndReplyUrl { get; set; }

        /// <remarks/>
        [DataMember(Order =27)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool EditingCannotSave { get; set; }

        /// <remarks/>
        [DataMember(Order = 28)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool EditModePostMessage { get; set; }

        /// <remarks/>
        [DataMember(Order = 29)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool EditNotificationPostMessage { get; set; }

        /// <remarks/>
        [DataMember(Order =30)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string EmbeddingPageOrigin { get; set; }

        /// <remarks/>
        [DataMember(Order =31)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string EmbeddingPageSessionInfo { get; set; }

        /// <remarks/>
        [DataMember(Order =32)]    
        [System.ComponentModel.DefaultValueAttribute(new string[] { })]
        public string[] EnabledApplicationFeatures { get; set; }

        /// <remarks/>
        [DataMember(Order =33)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool FileEmbedCommandPostMessage { get; set; }

        /// <remarks/>
        [DataMember(Order =34)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string FileEmbedCommandUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 35)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string FileExtension { get; set; }

        /// <remarks/>
        [DataMember(Order = 36)]
        [System.ComponentModel.DefaultValueAttribute(250)]
        public int FileNameMaxLength { get; set; }

        /// <remarks/>
        [DataMember(Order = 37)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool FileSharingPostMessage { get; set; }

        /// <remarks/>
        [DataMember(Order = 38)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string FileSharingUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 39)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string FileUrl { get; set; }

        /// <remarks/>
        [DataMember(Order =40)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool FileVersionPostMessage { get; set; }

        /// <remarks/>
        [DataMember(Order =41)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string FileVersionUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 42)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostAuthenticationId { get; set; }

        /// <remarks/>
        [DataMember(Order = 43)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostAuthenticationIdType { get; set; }
        
        /// <remarks/>
        [DataMember(Order =44)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostDivSyndicationViewUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 45)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostEditUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 46)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostEmbeddedEditUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 47)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostEmbeddedViewUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 48)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostName { get; set; }

        /// <remarks/>
        [DataMember(Order = 49)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostNotes { get; set; }

        /// <remarks/>
        [DataMember(Order = 50)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostRestUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 51)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostViewUrl { get; set; }

        /// <remarks/>
        [DataMember(Order =52)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool InsertImagePostMessage { get; set; }
        
        /// <remarks/>
        [DataMember(Order = 53)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string IrmPolicyDescription { get; set; }

        /// <remarks/>
        [DataMember(Order = 54)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string IrmPolicyTitle { get; set; }

        /// <remarks/>
        [DataMember(Order =55)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool IsAnonymousUser { get; set; }

        /// <remarks/>
        [DataMember(Order =56)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool IsEduUser { get; set; }

        /// <remarks/>
        [DataMember(Order =57)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool IsYammerEnabled { get; set; }

        /// <remarks/>
        [DataMember(Order =58)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public string LastModifiedTime { get; set; }

        /// <remarks/>
        [DataMember(Order = 59)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool LicenseCheckForEditIsEnabled { get; set; }

        /// <remarks/>
        [DataMember(Order =60)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string LicensedOrganization { get; set; }

        /// <remarks/>
        [DataMember(Order =61,IsRequired =true)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string OfficeCollaborationServiceEndpointUrl { get; set; }

        /// <remarks/>
        [DataMember(Order =62,IsRequired =true)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string OpenInClientCommandUrl { get; set; }

        /// <remarks/>
        [DataMember(Order =63,IsRequired =true)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public  bool OpenInClientPostMessage { get; set; }

        /// <remarks/>
        [DataMember(Order = 64,IsRequired =true)]
        public string OwnerId { get; set; }

        /// <remarks/>
        [DataMember(Order =65)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool PermissionsPostMessage { get; set; }

        [DataMember(Order =66)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool PolicyCheckPostMessage { get; set; }

        /// <remarks/>
        [DataMember(Order = 67)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string PostMessageOrigin { get; set; }

        /// <remarks/>
        [DataMember(Order = 68)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string PresenceProvider { get; set; }

        /// <remarks/>
        [DataMember(Order = 69)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string PresenceUserId { get; set; }

        /// <remarks/>
        [DataMember(Order = 70)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string PrivacyUrl { get; set; }

        /// <remarks/>
        [DataMember(Order =71)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool ProtectedFile { get; set; }

        /// <remarks/>
        [DataMember(Order = 72)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool ProtectInClient { get; set; }

        /// <remarks/>
        [DataMember(Order = 74)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool ReadOnly { get; set; }

        [DataMember(Order =75)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool ReportAbusePostMessage { get; set; }

        [DataMember(Order =76)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string ReportAbuseUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 77)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool RestrictedWebViewOnly { get; set; }

        /// <remarks/>
        [DataMember(Order =78)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string SafeLinksStatus { get; set; }

        /// <remarks/>
        [DataMember(Order = 79)]
        public string SHA256 { get; set; }

        /// <remarks/>
        [DataMember(Order = 80)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string SignInUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 81)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string SignoutUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 82,IsRequired =true)]
        [System.ComponentModel.DefaultValueAttribute(-1)]
        public int Size { get; set; }

        /// <remarks/>
        [DataMember(Order = 83)]
        [System.ComponentModel.DefaultValueAttribute(new string[] { })]
        public string[] SupportedShareUrlTypes { get; set; }

        /// <remarks/>
        [DataMember(Order =84 )]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsAddActivities { get; set; }

        /// <remarks/>
        [DataMember(Order = 85 )]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsCheckPolicy { get; set; }

        /// <remarks/>
        [DataMember(Order =86 )]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsCheckUserAccess { get; set; }

        /// <remarks/>
        [DataMember(Order = 87)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsCoauth { get; set; }

        /// <remarks/>
        [DataMember(Order = 88)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsCobalt { get; set; }

        /// <remarks/>
        [DataMember(Order = 89)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsContactsResolution { get; set; }

        /// <remarks/>
        [DataMember(Order =90 )]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsContainers { get; set; }

        /// <remarks/>
        [DataMember(Order =91 )]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsDeleteFile { get; set; }

        /// <remarks/>
        [DataMember(Order =92 )]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsEcosystem { get; set; }

        /// <remarks/>
        [DataMember(Order = 93)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsExtendedLockLength { get; set; }

        /// <remarks/>
        [DataMember(Order = 94)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsFileCreation { get; set; }

        /// <remarks/>
        [DataMember(Order =95 )]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsFileUserValue { get; set; }

        /// <remarks/>
        [DataMember(Order = 96)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsFolders { get; set; }

        /// <remarks/>
        [DataMember(Order = 97)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsGetActivities { get; set; }

        /// <remarks/>
        [DataMember(Order = 98)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsGetLock { get; set; }

        /// <remarks/>
        [DataMember(Order = 99)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsGrantUserAccess { get; set; }

        /// <remarks/>
        [DataMember(Order = 100)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsLocks { get; set; }

        /// <remarks/>
        [DataMember(Order =101)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsPolicies { get; set; }

        /// <remarks/>
        [DataMember(Order = 102)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsRename { get; set; }

        /// <remarks/>
        [DataMember(Order = 103)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsReviewing { get; set; }

        /// <remarks/>
        [DataMember(Order = 104)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsScenarioLinks { get; set; }

        /// <remarks/>
        [DataMember(Order = 105)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsSecureStore { get; set; }

        /// <remarks/>
        [DataMember(Order = 106)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsUpdate { get; set; }

        /// <remarks/>
        [DataMember(Order = 107)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsUserInfo { get; set; }

        /// <remarks/>
        [DataMember(Order = 108)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string TenantId { get; set; }

        /// <remarks/>
        [DataMember(Order = 109)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string TermsOfUseUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 110)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string TimeZone { get; set; }

        /// <remarks/>
        [DataMember(Order = 111)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string UniqueContentId { get; set; }

        /// <remarks/>
        [DataMember(Order = 112)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool UserCanAttend { get; set; }

        /// <remarks/>
        [DataMember(Order = 113)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool UserCanNotWriteRelative { get; set; }

        /// <remarks/>
        [DataMember(Order = 114)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool UserCanPresent { get; set; }

        /// <remarks/>
        [DataMember(Order = 115)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool UserCanRename { get; set; }

        /// <remarks/>
        [DataMember(Order = 116)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool UserCanReview { get; set; }

        /// <remarks/>
        [DataMember(Order = 117)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool UserCanWrite { get; set; }

        /// <remarks/>
        [DataMember(Order = 118)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string UserFriendlyName { get; set; }

        /// <remarks/>
        [DataMember(Order = 119)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string UserId { get; set; }

        /// <remarks/>
        [DataMember(Order = 120)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string UserInfo { get; set; }

        /// <remarks/>
        [DataMember(Order = 121,IsRequired =true)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string UserPrincipalName { get; set; }

        /// <remarks/>
        [DataMember(Order = 122,IsRequired =true)]
        public string Version { get; set; }

        /// <remarks/>
        [DataMember(Order = 123)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool WebEditingDisabled { get; set; }

        /// <remarks/>
        [DataMember(Order = 124)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool WorkflowPostMessage { get; set; }

        /// <remarks/>
        [DataMember(Order = 125)]
        [System.ComponentModel.DefaultValueAttribute(new string[] { })]
        public string[] WorkflowType { get; set; }

        /// <remarks/>
        [DataMember(Order = 126)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string WorkflowUrl { get; set; }
    }

    /// <summary>
    /// 3.3.5.1.2	PutRelativeFile
    /// </summary>
    public class PutRelativeFile
    {
        [DataMember(Order = 1)]
        public string Name { get; set; }

        [DataMember(Order = 2)]
        public string Url { get; set; }

        [DataMember(Order = 3)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostViewUrl { get; set; }

        [DataMember(Order = 4)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostEditUrl { get; set; }
    }

    /// <summary>
    /// 3.3.5.1.11	ReadSecureStore
    /// </summary>
    public class ReadSecureStore
    {
        [System.Xml.Serialization.XmlElementAttribute("ReadSecureStore")]
        [DataMember(Order = 1)]
        public string UserName { get; set; }

        [DataMember(Order = 2)]
        public string Password { get; set; }

        [DataMember(Order = 3)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool IsWindowsCredentials { get; set; }

        [DataMember(Order = 4)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool IsGroup { get; set; }
    }

    /// <summary>
    /// 3.3.5.2.1	CheckFolderInfo
    /// </summary>
    public class CheckFolderInfo
    {
        /// <remarks/>
        [DataMember(Order = 1)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string AADUserObjectId { get; set; }

        /// <remarks/>
        [DataMember(Order =2 )]
        [System.ComponentModel.DefaultValueAttribute(0)]
        public long AccessTokenExpiry { get; set; }

        /// <remarks/>
        [DataMember(Order = 3)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool AllowEarlyFeatures { get; set; }

        /// <remarks/>
        [DataMember(Order =4 )]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool AllowExternalMarketplace { get; set; }

        /// <remarks/>
        [DataMember(Order = 5)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string AppCatalogUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 6,IsRequired =true)]
        public string FolderName { get; set; }

        /// <remarks/>
        [DataMember(Order = 7)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string BreadcrumbBrandIconUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 8)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string BreadcrumbBrandName { get; set; }

        /// <remarks/>
        [DataMember(Order = 9)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string BreadcrumbBrandUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 10)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string BreadcrumbDocName { get; set; }

        /// <remarks/>
        [DataMember(Order = 11)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string BreadcrumbDocUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 12)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string BreadcrumbFolderName { get; set; }

        /// <remarks/>
        [DataMember(Order = 13)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string BreadcrumbFolderUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 14)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string ClientUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 15)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool CloseButtonClosesWindow { get; set; }

        /// <remarks/>
        [DataMember(Order =16 )]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool ClosePostMessage { get; set; }

        [DataMember(Order = 17)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string CloseUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 18)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string DirectInvokeDAVUrl { get; set; }

        /// <remarks/>
        [DataMember(Order =19 )]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool DisablePrint { get; set; }

        /// <remarks/>
        [DataMember(Order = 20)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool EditNotificationPostMessage { get; set; }

        /// <remarks/>
        [DataMember(Order =21 )]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool FileSharingPostMessage { get; set; }

        /// <remarks/>
        [DataMember(Order = 22)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string FileSharingUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 23)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool FileVersionPostMessage { get; set; }

        /// <remarks/>
        [DataMember(Order = 24)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string FileVersionUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 25)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostAuthenticationId { get; set; }

        /// <remarks/>
        [DataMember(Order = 26)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostAuthenticationIdType { get; set; }

        /// <remarks/>
        [DataMember(Order = 27)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostEditUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 28)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostEmbeddedEditUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 29)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostEmbeddedViewUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 30)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostName { get; set; }


        /// <remarks/>
        [DataMember(Order = 31)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostNotes { get; set; }

        /// <remarks/>
        [DataMember(Order = 31)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostViewUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 32)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string IrmPolicyDescription { get; set; }

        /// <remarks/>
        [DataMember(Order =33 )]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string IrmPolicyTitle { get; set; }

        /// <remarks/>
        [DataMember(Order =34 )]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool IsAnonymousUser { get; set; }

        /// <remarks/>
        [DataMember(Order =35,IsRequired =true )]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string OpenInClientCommandUrl { get; set; }

        /// <remarks/>
        [DataMember(Order =36,IsRequired =true )]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool OpenInClientPostMessage { get; set; }

        /// <remarks/>
        [DataMember(Order = 37,IsRequired =true)]
        public string OwnerId { get; set; }

        /// <remarks/>
        [DataMember(Order =38 )]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string PostMessageOrigin { get; set; }

        /// <remarks/>
        [DataMember(Order = 39)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string PresenceProvider { get; set; }

        /// <remarks/>
        [DataMember(Order = 40)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string PresenceUserId { get; set; }

        [DataMember(Order = 41)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string PrivacyUrl { get; set; }

        /// <remarks/>
        [DataMember(Order =42 )]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool ProtectInClient { get; set; }

        /// <remarks/>
        [DataMember(Order = 43)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool ReportAbusePostMessage{ get; set; }

        /// <remarks/>
        [DataMember(Order = 44)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string ReportAbuseUrl { get; set; }

        /// <remarks/>
        [DataMember(Order = 45)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string SafeLinksStatus { get; set; }

        [DataMember(Order = 46)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string SignoutUrl { get; set; }

        /// <remarks/>
        [DataMember(Order =47 )]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsFileUserValue { get; set; }

        [DataMember(Order = 48)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsSecureStore { get; set; }

        [DataMember(Order = 49)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string TenantId { get; set; }

        [DataMember(Order = 50)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string TermsOfUseUrl { get; set; }

        /// <remarks/>
        [DataMember(Order =51 )]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool UserCanReview { get; set; }

        /// <remarks/>
        [DataMember(Order = 52)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool UserCanWrite { get; set; }

        /// <remarks/>
        [DataMember(Order = 53)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string UserFriendlyName { get; set; }

        /// <remarks/>
        [DataMember(Order = 54)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string UserId { get; set; }

        /// <remarks/>
        [DataMember(Order =55,IsRequired =true )]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string UserPrincipalName { get; set; }

        [DataMember(Order = 56)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool WebEditingDisabled { get; set; }

        /// <remarks/>
        [DataMember(Order = 57)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool WorkflowPostMessage { get; set; }

        /// <remarks/>
        [DataMember(Order = 58)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string WorkflowType { get; set; }

        /// <remarks/>
        [DataMember(Order =59 )]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string WorkflowUrl { get; set; }
    }

    /// <summary>
    /// 3.3.5.4.1	EnumerateChildren
    /// </summary>
    public class EnumerateChildren
    {
        [System.Xml.Serialization.XmlElementAttribute("Children")]
        [DataMember(Order = 1)]
        public ChildrenItem[] Children { get; set; }
    }

    /// <summary>
    /// 3.3.5.4.1	ChildrenItem
    /// </summary>
    public class ChildrenItem
    {
        [DataMember(Order = 1)]
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name { get; set; }

        [DataMember(Order = 2)]
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Url { get; set; }

        [DataMember(Order = 3)]
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Version { get; set; }
    }

    /// <summary>
    /// 3.3.5.1.14	RenameFile
    /// </summary>
    public class RenameFile
    {
        [DataMember(Order = 1)]
        public string Name { get; set; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "")]
    [System.Xml.Serialization.XmlRootAttribute("wopi-discovery", Namespace = "", IsNullable = false)]
    public class wopidiscovery
    {
        [System.Xml.Serialization.XmlElementAttribute("net-zone", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ct_netzone[] netzone { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("proof-key", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ct_proofkey proofkey { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "ct_net-zone", Namespace = "http://microsoft.com/wsdl/types/")]
    public partial class ct_netzone
    {
        [System.Xml.Serialization.XmlElementAttribute("app", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ct_appname[] app { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public st_wopizone name { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool nameSpecified { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "ct_app-name", Namespace = "http://microsoft.com/wsdl/types/")]
    public partial class ct_appname
    {
        public ct_appname()
        {
            this.checkLicense = false;
        }

        [System.Xml.Serialization.XmlElementAttribute("action", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ct_wopiaction[] action { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string favIconUrl { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool checkLicense { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "ct_wopi-action", Namespace = "http://microsoft.com/wsdl/types/")]
    public partial class ct_wopiaction
    {
        public ct_wopiaction()
        {
            this.@default = false;
            this.useParent = false;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public st_wopiactionvalues name { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool @default { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string requires { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string urlsrc { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ext { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string progid { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string newprogid { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string newext { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool useParent { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public string targetext { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "st_wopi-action-values", Namespace = "http://microsoft.com/wsdl/types/")]
    public enum st_wopiactionvalues
    {

        /// <remarks/>
        view,

        /// <remarks/>
        edit,

        /// <remarks/>
        mobileView,

        /// <remarks/>
        embedview,

        /// <remarks/>
        embededit,

        /// <remarks/>
        mobileclient,

        /// <remarks/>
        present,

        /// <remarks/>
        presentservice,

        /// <remarks/>
        attend,

        /// <remarks/>
        attendservice,

        /// <remarks/>
        editnew,

        /// <remarks/>
        imagepreview,

        /// <remarks/>
        interactivepreview,

        /// <remarks/>
        formsubmit,

        /// <remarks/>
        formedit,

        /// <remarks/>
        rest,

        /// <remarks/>
        preloadview,

        /// <remarks/>
        preloadedit,

        /// <remarks/>
        rtc,

        /// <remarks/>
        getinfo,

        /// <remarks/>
        convert,

        /// <remarks/>
        syndicate,

        /// <remarks/>
        legacywebservice,

        /// <remarks/>
        collab,

        /// <remarks/>
        formpreview,

        /// <remarks/>
        documentchat
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "ct_proof-key", Namespace = "http://microsoft.com/wsdl/types/")]
    public partial class ct_proofkey
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string exponent { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]

        public string modulus { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]

        public string oldexponent { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]

        public string oldmodulus { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string oldvalue { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value { get; set; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "st_wopi-zone", Namespace = "http://microsoft.com/wsdl/types/")]
    public enum st_wopizone
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("internal-http")]
        internalhttp,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("internal-https")]
        internalhttps,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("external-http")]
        externalhttp,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("external-https")]
        externalhttps,
    }

    /// <summary>
    /// WOPI Operations
    /// </summary>
    public enum WOPIOperations
    {
        Discovery,
        CheckFileInfo,
        PutRelativeFile,
        Lock,
        Unlock,
        RefreshLock,
        UnlockAndRelock,
        GetLock,
        ExecuteCellStorageRequest,
        ExecuteCellStorageRelativeRequest,
        DeleteFile,
        ReadSecureStore,
        GetRestrictedLink,
        RevokeRestrictedLink,
        CheckFolderInfo,
        GetFile,
        PutFile,
        EnumerateChildren,
        RenameFile,
        PutUserInfo,
        Unknown
    }

    public class ResponseBodyBase
    { }

    public class WOPIResponseMessage
    {
        public ResponseBodyBase Body;
        public uint StatusCode;
    }

    /// <summary>
    /// perform serializer operations for MS-WOPI
    /// </summary>
    public static class WOPISerilizer
    {
        private static string[] jsonRequireItemsForCheckFileInfo = { "BaseFileName", "OwnerId", "Size", "Version" };
        private static string[] jsonRequireItemsForPutRelativeFile = { "Name", "Url" };
        private static string[] jsonRequireItemsForReadSecureStore = { "UserName", "Password" };
        private static string[] jsonRequireItemsForCheckFolderInfo = { "FolderName", "OwnerId" };
        private static string[] jsonRequireItemsForRenameFile = { "Name" };

        /// <summary>
        /// Convert the JSON string to the specified Object.
        /// </summary>
        /// <typeparam name="T">The type of the JSON object which is defined in MS-WOPI</typeparam>
        /// <param name="jsonValue">The value of the JSON strings.</param>
        /// <returns>the object which is de-serialize from JSON string.</returns>
        public static T JsonToObject<T>(string jsonValue) where T : class
        {
            Type currentType = typeof(T);

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(currentType);

            MemoryStream memoryStreamInstance = new MemoryStream(Encoding.Default.GetBytes(jsonValue));

            if (currentType.Name.Equals("CheckFileInfo"))
            {
                CheckRequiredJsonItem(jsonRequireItemsForCheckFileInfo, jsonValue);
            }
            else if (currentType.Name.Equals("PutRelativeFile"))
            {
                CheckRequiredJsonItem(jsonRequireItemsForPutRelativeFile, jsonValue);
            }
            else if (currentType.Name.Equals("ReadSecureStore"))
            {
                CheckRequiredJsonItem(jsonRequireItemsForReadSecureStore, jsonValue);
            }
            else if (currentType.Name.Equals("CheckFolderInfo"))
            {
                CheckRequiredJsonItem(jsonRequireItemsForCheckFolderInfo, jsonValue);
            }
            else if (currentType.Name.Equals("RenameFile"))
            {
                CheckRequiredJsonItem(jsonRequireItemsForRenameFile, jsonValue);
            }

            T expectedInstance = serializer.ReadObject(memoryStreamInstance) as T;
            memoryStreamInstance.Dispose();
            return expectedInstance;
        }

        /// <summary>
        /// A method is used to check whether the required items exists in the JSON strings.
        /// </summary>
        /// <param name="jsonItems">The collection for the require items.</param>
        /// <param name="jsonString">The JSON string.</param>
        /// <returns>Return true indicating all required item exist in the JSON string. </returns>
        public static bool CheckRequiredJsonItem(string[] jsonItems, string jsonString)
        {
            foreach (string item in jsonItems)
            {
                if (!jsonString.Contains("\"" + item + "\"" + ":"))
                {
                    throw new InvalidOperationException("The require item" + item + "doesn't exist in the" + jsonString + "Json string.");
                }
            }

            return true;
        }
    }
}
