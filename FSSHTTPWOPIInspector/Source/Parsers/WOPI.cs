namespace FSSHTTPandWOPIInspector.Parsers
{
    using System.Xml.Serialization;
    using System.Runtime.Serialization;
    using System;
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Text;

    /// <summary>
    /// 3.3.5.2.1	CheckFolderInfo
    /// </summary>
    public class CheckFileInfo : ResponseBodyBase
    {
        [DataMember(Order = 1)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool AllowExternalMarketplace { get; set; }

        [DataMember(Order = 2)]
        public string BaseFileName { get; set; }

        [DataMember(Order = 3)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string BreadcrumbBrandName { get; set; }

        [DataMember(Order = 4)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string BreadcrumbBrandUrl { get; set; }

        [DataMember(Order = 5)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string BreadcrumbDocName { get; set; }

        [DataMember(Order = 6)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string BreadcrumbDocUrl { get; set; }

        [DataMember(Order = 7)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string BreadcrumbFolderUrl { get; set; }

        [DataMember(Order = 9)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string ClientUrl { get; set; }

        [DataMember(Order = 10)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool CloseButtonClosesWindow { get; set; }

        [DataMember(Order = 11)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string CloseUrl { get; set; }

        [DataMember(Order = 12)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool DisableBrowserCachingOfUserContent { get; set; }

        [DataMember(Order = 13)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool DisablePrint { get; set; }

        [DataMember(Order = 14)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool DisableTranslation { get; set; }

        [DataMember(Order = 15)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string DownloadUrl { get; set; }

        [DataMember(Order = 16)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string FileSharingUrl { get; set; }

        [DataMember(Order = 17)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string FileUrl { get; set; }

        [DataMember(Order = 18)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostAuthenticationId { get; set; }

        [DataMember(Order = 19)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostEditUrl { get; set; }

        [DataMember(Order = 20)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostEmbeddedEditUrl { get; set; }

        [DataMember(Order = 21)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostEmbeddedViewUrl { get; set; }

        [DataMember(Order = 22)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostName { get; set; }

        [DataMember(Order = 23)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostNotes { get; set; }

        [DataMember(Order = 24)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostRestUrl { get; set; }

        [DataMember(Order = 25)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostViewUrl { get; set; }

        [DataMember(Order = 26)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string IrmPolicyDescription { get; set; }

        [DataMember(Order = 27)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string IrmPolicyTitle { get; set; }

        [DataMember(Order = 28)]
        public string OwnerId { get; set; }

        [DataMember(Order = 29)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string PresenceProvider { get; set; }

        [DataMember(Order = 30)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string PresenceUserId { get; set; }

        [DataMember(Order = 31)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string PrivacyUrl { get; set; }

        [DataMember(Order = 32)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string ProtectInClient { get; set; }

        [DataMember(Order = 33)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public bool ReadOnly { get; set; }

        [DataMember(Order = 34)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool RestrictedWebViewOnly { get; set; }

        [DataMember(Order = 35)]
        public string SHA256 { get; set; }

        [DataMember(Order = 36)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string SignoutUrl { get; set; }

        [DataMember(Order = 37)]
        public int Size { get; set; }

        [DataMember(Order = 38)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsCoauth { get; set; }

        [DataMember(Order = 39)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsCobalt { get; set; }

        [DataMember(Order = 40)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsFolders { get; set; }

        [DataMember(Order = 41)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsLocks { get; set; }

        [DataMember(Order = 42)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsScenarioLinks { get; set; }

        [DataMember(Order = 43)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsSecureStore { get; set; }

        [DataMember(Order = 43)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsUpdate { get; set; }

        [DataMember(Order = 44)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string TenantId { get; set; }

        [DataMember(Order = 45)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string TermsOfUseUrl { get; set; }

        [DataMember(Order = 46)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string TimeZone { get; set; }

        [DataMember(Order = 47)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool UserCanAttend { get; set; }

        [DataMember(Order = 48)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool UserCanNotWriteRelative { get; set; }

        [DataMember(Order = 49)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool UserCanPresent { get; set; }

        [DataMember(Order = 50)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool UserCanWrite { get; set; }

        [DataMember(Order = 51)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string UserFriendlyName { get; set; }

        [DataMember(Order = 52)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string UserId { get; set; }

        [DataMember(Order = 53)]
        public string Version { get; set; }

        [DataMember(Order = 54)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool WebEditingDisabled { get; set; }
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
    /// 3.3.5.1.1	CheckFileInfo
    /// </summary>
    public class CheckFolderInfo
    {
        [DataMember(Order = 1)]
        public string FolderName { get; set; }

        [DataMember(Order = 2)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string BreadcrumbBrandIconUrl { get; set; }

        [DataMember(Order = 3)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string BreadcrumbBrandName { get; set; }

        [DataMember(Order = 4)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string BreadcrumbBrandUrl { get; set; }

        [DataMember(Order = 5)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string BreadcrumbDocName { get; set; }

        [DataMember(Order = 6)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string BreadcrumbDocUrl { get; set; }

        [DataMember(Order = 7)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string BreadcrumbFolderName { get; set; }

        [DataMember(Order = 8)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string BreadcrumbFolderUrl { get; set; }

        [DataMember(Order = 9)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string ClientUrl { get; set; }

        [DataMember(Order = 10)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool CloseButtonClosesWindow { get; set; }

        [DataMember(Order = 11)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string CloseUrl { get; set; }

        [DataMember(Order = 12)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string FileSharingUrl { get; set; }

        [DataMember(Order = 13)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostAuthenticationId { get; set; }

        [DataMember(Order = 14)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostEditUrl { get; set; }

        [DataMember(Order = 15)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostEmbeddedEditUrl { get; set; }

        [DataMember(Order = 16)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostEmbeddedViewUrl { get; set; }

        [DataMember(Order = 17)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostName { get; set; }

        [DataMember(Order = 18)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string HostViewUrl { get; set; }

        [DataMember(Order = 19)]
        public string OwnerId { get; set; }

        [DataMember(Order = 20)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string PresenceProvider { get; set; }

        [DataMember(Order = 21)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string PresenceUserId { get; set; }

        [DataMember(Order = 22)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string PrivacyUrl { get; set; }

        [DataMember(Order = 23)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string SignoutUrl { get; set; }

        [DataMember(Order = 24)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsSecureStore { get; set; }

        [DataMember(Order = 25)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string TenantId { get; set; }

        [DataMember(Order = 26)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string TermsOfUseUrl { get; set; }

        [DataMember(Order = 27)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool UserCanWrite { get; set; }

        [DataMember(Order = 28)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string UserFriendlyName { get; set; }

        [DataMember(Order = 29)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string UserId { get; set; }

        [DataMember(Order = 30)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool WebEditingDisabled { get; set; }
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
        mobileview,

        /// <remarks/>
        embedview,

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
    {}

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
        private static string[] jsonRequireItemsForCheckFileInfo = { "BaseFileName", "OwnerId", "SHA256", "Size", "Version" };
        private static string[] jsonRequireItemsForPutRelativeFile = { "Name", "Url" };
        private static string[] jsonRequireItemsForReadSecureStore = { "UserName", "Password" };
        private static string[] jsonRequireItemsForCheckFolderInfo = { "FolderName", "OwnerId" };

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
