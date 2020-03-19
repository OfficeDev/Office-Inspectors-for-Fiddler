namespace FSSHTTPandWOPIInspector.Parsers
{
    using System.Xml.Serialization;
    using System.Xml;
    using System.Xml.Schema;
    using System.ComponentModel;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Envelope message for request, defined in section 2.2.2.1
    /// </summary>
    [XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class RequestEnvelope
    {
        public RequestEnvelopeBody Body { get; set; }
    }

    /// <summary>
    /// Envelope body for request, defined in section 2.2.2.1
    /// </summary>
    [XmlRoot("Body")]
    public class RequestEnvelopeBody
    {
        [XmlElementAttribute("RequestVersion", Namespace = "http://schemas.microsoft.com/sharepoint/soap/")]
        public VersionType RequestVersion { get; set; }

        [XmlElementAttribute("RequestCollection", Namespace = "http://schemas.microsoft.com/sharepoint/soap/")]
        public RequestCollection RequestCollection { get; set; }
    }

    /// <summary>
    /// Request collection type defined in section 2.2.3.3
    /// </summary>
    public class RequestCollection
    {
        [XmlElement()]
        public Request[] Request { get; set; }

        [XmlAttribute()]
        public string CorrelationId { get; set; }
    }

    /// <summary>
    /// Request element that is part of a cell storage service request, defined in section 2.2.3.2
    /// </summary>
    public class Request
    {
        [XmlElementAttribute()]
        GenericPropertiesType GenericProperties { get; set; }

        [XmlElementAttribute()]
        public SubRequestElementGenericType[] SubRequest { get; set; }

        [XmlAttribute()]
        public string Url { get; set; }

        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string Interval { get; set; }

        [XmlAttribute(DataType = "integer")]
        public string MetaData { get; set; }

        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string RequestToken { get; set; }

        [XmlAttribute()]
        public string UserAgent { get; set; }

        [XmlAttribute()]
        public string UserAgentClient { get; set; }

        [XmlAttribute()]
        public string UserAgentPlatform { get; set; }

        [XmlAttribute()]
        public string Build { get; set; }

        [XmlAttribute()]
        public string ParentFolderResourceID { get; set; }

        [XmlAttribute()]
        public bool ShouldReturnDisambiguatedFileName { get; set; }

        [XmlIgnore]
        public bool ShouldReturnDisambiguatedFileNameSpecified { get; set; }

        [XmlAttribute()]
        public string ResourceID { get; set; }

        [XmlAttribute()]
        public string UseResourceID { get; set; }
    }

    /// <summary>
    /// Subrequest type, defined in section 2.2.4.5
    /// </summary>
    public class SubRequestType
    {
        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string SubRequestToken { get; set; }

        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string DependsOn { get; set; }

        [XmlAttribute()]
        public DependencyTypes DependencyType { get; set; }
    }

    /// <summary>
    /// A generic subrequest type, defined in section 2.2.4.4
    /// </summary>
    public class SubRequestElementGenericType : SubRequestType
    {
        public SubRequestDataGenericType SubRequestData { get; set; }

        [XmlTextAttribute()]
        public string[] Text { get; set; }

        [XmlAttribute()]
        public SubRequestAttributeType Type { get; set; }
    }

    /// <summary>
    /// A generic subrequest data type, defined in section 2.2.4.3
    /// </summary>
    public class SubRequestDataGenericType
    {
        [XmlElement(Namespace = "http://www.w3.org/2004/08/xop/include")]
        public Include Include { get; set; }

        [XmlElementAttribute()]
        public object IncludeObject { get; set; }

        [XmlTextAttribute()]
        public string[] Text { get; set; }

        [XmlElementAttribute()]
        public object TextObject { get; set; }

        [XmlAttribute()]
        public string ClientID { get; set; }

        [XmlAttribute()]
        public bool ReleaseLockOnConversionToExclusiveFailure { get; set; }

        [XmlIgnore]
        public bool ReleaseLockOnConversionToExclusiveFailureSpecified { get; set; }

        [XmlAttribute()]
        public string SchemaLockID { get; set; }

        [XmlAttribute(DataType = "integer")]
        public string Timeout { get; set; }

        [XmlAttribute()]
        public string AllowFallbackToExclusive { get; set; }

        [XmlIgnore]
        public bool AllowFallbackToExclusiveSpecified { get; set; }

        [XmlAttribute()]
        public string ExclusiveLockID { get; set; }

        [XmlAttribute()]
        public long BinaryDataSize { get; set; }

        [XmlAttribute()]
        public bool AsEditor { get; set; }

        [XmlIgnore]
        public bool AsEditorSpecified { get; set; }

        [XmlAttribute()]
        public string Key { get; set; }

        [XmlAttribute()]
        public string Value { get; set; }

        [XmlAttribute()]
        public string NewFileName { get; set; }

        [XmlAttribute()]
        public string Version { get; set; } //FileVersionNumberType 

        [XmlAttribute]
        public bool Coalesce { get; set; }

        [XmlIgnore]
        public bool CoalesceSpecified { get; set; }

        [XmlAttribute()]
        public bool GetFileProps { get; set; }

        [XmlIgnore()]
        public bool GetFilePropsSpecified { get; set; }

        [XmlAttribute()]
        public bool CoauthVersioning { get; set; }

        [XmlIgnoreAttribute()]
        public bool CoauthVersioningSpecified { get; set; }

        [XmlAttribute()]
        public string Etag { get; set; }

        [XmlAttribute()]
        public string ContentChangeUnit { get; set; }

        [XmlAttribute()]
        public string ClientFileID { get; set; }

        [XmlAttribute()]
        public string PartitionID { get; set; }

        [XmlAttribute()]
        public bool ExpectNoFileExists { get; set; }

        [XmlIgnoreAttribute()]
        public bool ExpectNoFileExistsSpecified { get; set; }

        [XmlAttribute()]
        public string BypassLockID { get; set; }

        [XmlAttribute(DataType = "integer")]
        public string LastModifiedTime { get; set; }

        [XmlAttribute()]
        public CoauthRequestTypes CoauthRequestType { get; set; }

        [XmlIgnoreAttribute()]
        public bool CoauthRequestTypeSpecified { get; set; }

        [XmlAttribute()]
        public SchemaLockRequestTypes SchemaLockRequestType { get; set; }

        [XmlIgnoreAttribute()]
        public bool SchemaLockRequestTypeSpecified { get; set; }

        [XmlAttribute()]
        public ExclusiveLockRequestTypes ExclusiveLockRequestType { get; set; }

        [XmlIgnoreAttribute()]
        public bool ExclusiveLockRequestTypeSpecified { get; set; }

        [XmlAttribute()]
        public EditorsTableRequestTypes EditorsTableRequestType { get; set; }

        [XmlIgnoreAttribute()]
        public bool EditorsTableRequestTypeSpecified { get; set; }

        [XmlAttribute()]
        public FileOperationRequestTypes FileOperation { get; set; }

        [XmlIgnoreAttribute()]
        public bool FileOperationSpecified { get; set; }

        [XmlAttribute()]
        public VersioningRequestTypes VersioningRequestType { get; set; }

        [XmlIgnoreAttribute()]
        public bool VersioningRequestTypeSpecified { get; set; }

    }


    /// <summary>
    /// A GenericPropertiesType, defined in section 2.2.4.1
    /// </summary>
    public class GenericPropertiesType
    {
        public PropertyType[] Property { get; set; }
    }

    /// <summary>
    /// PropertyType, defined in section 2.2.4.2
    /// </summary>
    public class PropertyType
    {
        [XmlAttribute()]
        public ushort Id { get; set; }

        [XmlAttribute()]
        public ushort Value { get; set; }
    }

    /// <summary>
    /// XOP10 section 2.1
    /// </summary>
    public class Include
    {
        [XmlAnyElement()]
        public System.Xml.XmlElement[] Any { get; set; }

        [XmlAttribute(DataType = "anyURI")]
        public string href { get; set; }

        [XmlAnyAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr { get; set; }
    }

    /// <summary>
    /// The VersionType complex type contains information about the version of the cell storage service message.
    /// Defined in section 2.2.4.9
    /// </summary>
    public class VersionType
    {
        [XmlAttributeAttribute("Version")]
        public ushort Version { get; set; }

        [XmlAttributeAttribute("MinorVersion")]
        public ushort MinorVersion { get; set; }
    }

    /// <summary>
    /// The ResponseVersion element contains version-specific information for this cell storage service response message.
    /// Defined in section 2.2.3.7
    /// </summary>
    public class ResponseVersion : VersionType
    {
        [XmlAttribute]
        public GenericErrorCodeTypes ErrorCode { get; set; }

        [XmlAttribute]
        public string ErrorMessage { get; set; }

        [XmlIgnore]
        public bool ErrorCodeSpecified { get; set; }
    }

    /// <summary>
    /// The GenericErrorCodeTypes simple type is used to represent generic error code types that occur during cell storage service subrequest processing.
    /// Defined in section 2.2.5.6
    /// </summary>
    public enum GenericErrorCodeTypes
    {
        Success,
        IncompatibleVersion,
        InvalidUrl,
        FileNotExistsOrCannotBeCreated,
        FileUnauthorizedAccess,
        PathNotFound,
        ResourceIdDoesNotExist,
        ResourceIdDoesNotMatch,
        InvalidSubRequest,
        SubRequestFail,
        BlockedFileType,
        DocumentCheckoutRequired,
        InvalidArgument,
        RequestNotSupported,
        InvalidWebUrl,
        WebServiceTurnedOff,
        ColdStoreConcurrencyViolation,
        HighLevelExceptionThrown,
        Unknown,
    }

    /// <summary>
    /// Envelope message for response, defined in section 2.2.2.2
    /// </summary>
    [XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class ResponseEnvelope
    {
        public ResponseEnvelopeBody Body { get; set; }
    }

    /// <summary>
    /// Envelope body for response, defined in section 2.2.2.2
    /// </summary>
    [XmlRoot("Body")]
    public class ResponseEnvelopeBody
    {
        [XmlElementAttribute("ResponseVersion", Namespace = "http://schemas.microsoft.com/sharepoint/soap/")]
        public ResponseVersion ResponseVersion { get; set; }

        [XmlElementAttribute("ResponseCollection", Namespace = "http://schemas.microsoft.com/sharepoint/soap/")]
        public ResponseCollection ResponseCollection { get; set; }
    }

    /// <summary>
    /// Response collection type defined in section 2.2.3.6
    /// </summary>
    public class ResponseCollection
    {
        [XmlElement()]
        public Response[] Response { get; set; }

        [XmlAttribute()]
        public string WebUrl { get; set; }

        [XmlAttribute()]
        public string WebUrlIsEncoded { get; set; }
    }

    /// <summary>
    /// Response element that is part of a cell storage service response, defined in section 2.2.3.5
    /// </summary>
    public class Response
    {
        [XmlElement()]
        public SubResponseElementGenericType[] SubResponse { get; set; }

        [XmlTextAttribute()]
        public string[] Text { get; set; }

        [XmlAttribute()]
        public string Url { get; set; }

        [XmlAttribute()]
        public string UrlIsEncoded { get; set; }

        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string RequestToken { get; set; }

        [XmlAttribute(DataType = "integer")]
        public string HealthScore { get; set; }

        [XmlAttribute()]
        public GenericErrorCodeTypes ErrorCode { get; set; }

        [XmlIgnore()]
        public bool ErrorCodeSpecified { get; set; }

        [XmlAttribute()]
        public string ErrorMessage { get; set; }

        [XmlAttribute()]
        public string SuggestedFileName { get; set; }

        [XmlAttribute()]
        public string ResourceID { get; set; }

        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string IntervalOverride { get; set; }
    }

    /// <summary>
    /// Subresponse type, defined in section 2.2.4.8
    /// </summary>
    public class SubResponseType
    {
        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string SubRequestToken { get; set; }

        [XmlAttribute()]
        public string ServerCorrelationId { get; set; }

        [XmlAttribute()]
        public string ErrorCode { get; set; }

        [XmlAttribute(DataType = "integer")]
        public string HResult { get; set; }

        [XmlAttribute()]
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// A generic subresponse type, defined in section 2.2.4.7
    /// </summary>
    public class SubResponseElementGenericType : SubResponseType
    {
        public SubResponseDataGenericType SubResponseData { get; set; }

        public object SubResponseStreamInvalid { get; set; }

        public GetVersionsResponseType GetVersionsResponse { get; set; }
    }

    /// <summary>
    /// A generic subresponse data type, defined in section 2.2.4.6
    /// </summary>
    public class SubResponseDataGenericType
    {
        [XmlElement(Namespace = "http://www.w3.org/2004/08/xop/include")]
        public Include Include { get; set; }

        [XmlElementAttribute()]
        public object IncludeObject { get; set; }

        [XmlElementAttribute()]
        public GetDocMetaInfoPropertySetType DocProps { get; set; }

        [XmlElementAttribute()]
        public GetDocMetaInfoPropertySetType FolderProps { get; set; }

        [XmlElementAttribute()]
        public VersioningUserTableType UserTable { get; set; }

        [XmlElementAttribute()]
        public VersioningVersionListType Versions { get; set; }

        [XmlTextAttribute()]
        public string[] Text { get; set; }

        [XmlElementAttribute()]
        public object TextObject { get; set; }




        [XmlAttribute()]
        public string Etag { get; set; }

        [XmlAttribute(DataType = "integer")]
        public string CreateTime { get; set; }

        [XmlAttribute(DataType = "integer")]
        public string LastModifiedTime { get; set; }

        [XmlAttribute(DataType = "NCName")]
        public string ModifiedBy { get; set; }

        [XmlAttribute()]
        public string CoalesceErrorMessage { get; set; }

        [XmlAttribute(DataType = "integer")]
        public string CoalesceHResult { get; set; }

        [XmlAttribute()]
        public string ContainsHotboxData { get; set; }

        [XmlAttribute()]
        public string HaveOnlyDemotionChanges { get; set; }

        [XmlAttribute(DataType = "NCName")]
        public string UserName { get; set; }

        [XmlAttribute()]
        public string UserEmailAddress { get; set; }

        [XmlAttribute()]
        public string UserSIPAddress { get; set; }

        [XmlAttribute()]
        public string UserLogin { get; set; }

        [XmlAttribute()]
        public bool UserIsAnonymous { get; set; }

        [XmlIgnore()]
        public bool UserIsAnonymousSpecified { get; set; }

        [XmlAttribute(DataType = "positiveInteger")]
        public string ServerTime { get; set; }

        [XmlAttribute()]
        public string LockType { get; set; }

        [XmlIgnore()]
        public bool LockTypeSpecified { get; set; }

        [XmlAttribute()]
        public CoauthStatusType CoauthStatus { get; set; }

        [XmlIgnore()]
        public bool CoauthStatusSpecified { get; set; }

        [XmlAttribute()]
        public string TransitionID { get; set; }

        [XmlAttribute()]
        public ExclusiveLockReturnReasonTypes ExclusiveLockReturnReason { get; set; }

        [XmlIgnore()]
        public bool ExclusiveLockReturnReasonSpecified { get; set; }
    }

    /// <summary>
    /// The GetDocMetaInfoPropertySetType complex type contains a sequence of Property elements to describe the set of metainfo related to the file.
    /// Defined in section 2.3.1.28
    /// </summary>
    public class GetDocMetaInfoPropertySetType
    {
        [XmlElement()]
        public GetDocMetaInfoPropertyType[] Property { get; set; }
    }

    /// <summary>
    /// The GetDocMetaInfoProperty complex type contains a metainfo key/value pair that is related either to the file against which
    /// the request is made or its parent directory as part of the corresponding GetDocMetaInfo subrequest. Defined in 2.3.1.29
    /// </summary>
    public class GetDocMetaInfoPropertyType
    {
        [XmlAttribute()]
        public string Key { get; set; }

        [XmlAttribute()]
        public string Value { get; set; }
    }

    /// <summary>
    /// VersioningUserTableType section 2.3.1.40
    /// </summary>
    public class VersioningUserTableType
    {
        [XmlElementAttribute()]
        public UserDataType[] User { get; set; }
    }    

    /// <summary>
    /// VersioningVersionListType 2.3.1.41
    /// </summary>
    public class VersioningVersionListType
    {
        [XmlElementAttribute()]
        public FileVersionDataType[] Version { get; set; }
    }

    /// <summary>
    /// UserDataType 2.3.1.42
    /// </summary>
    public class UserDataType
    {
        [XmlAttribute(DataType = "integer")]
        public string UserId { get; set; }

        [XmlAttribute()]
        public string UserLogin { get; set; }

        [XmlAttribute()]
        public string UserName { get; set; }

        [XmlAttribute()]
        public string UserEmailAddress { get; set; }
    }

    /// <summary>
    /// FileVersionDataType 2.3.1.43
    /// </summary>
    public class FileVersionDataType
    {
        [XmlAttribute()]
        public string IsCurrent { get; set; }

        [XmlAttribute()]
        public string Number { get; set; }

        [XmlAttribute()]
        public string LastModifiedTime { get; set; }

        [XmlAttribute()]
        public string UserId { get; set; }

        [XmlElementAttribute()]
        public EventType Events { get; set; }
    }

    /// <summary>
    /// EventType subelement type defined in 2.3.1.43
    /// </summary>
    public class EventType
    {
        [XmlElementAttribute()]
        public FileVersionEventDataType[] Event { get; set; }
    }

    /// <summary>
    /// FileVersionEventDataType 2.3.1.44
    /// </summary>
    public class FileVersionEventDataType
    {
        [XmlAttribute()]
        public string Id { get; set; }

        [XmlAttribute()]
        public string Type { get; set; }

        [XmlAttribute()]
        public string CreateTime { get; set; }

        [XmlAttribute()]
        public string UserId { get; set; }
    }

    /// <summary>
    /// The LockTypes simple type is used to represent the type of file lock. Defined in 2.2.5.9
    /// </summary>
    public enum LockTypes
    {
        None,
        SchemaLock,
        ExclusiveLock,
    }

    /// <summary>
    /// The CoauthStatusType simple type is used to represent the coauthoring status of a targeted URL for the file. Defined in 2.2.5.1
    /// </summary>
    public enum CoauthStatusType
    {
        None,
        Alone,
        Coauthoring,
    }

    /// <summary>
    /// The ExclusiveLockReturnReasonTypes simple type is used to represent string values that indicate the reason why 
    /// an exclusive lock is granted on a file in a cell storage service response message. Defined in 2.2.5.5
    /// </summary>
    public enum ExclusiveLockReturnReasonTypes
    {
        CoauthoringDisabled,
        CheckedOutByCurrentUser,
        CurrentUserHasExclusiveLock,
    }

    /// <summary>
    /// GetVersionsSubResponseType defined in section 2.3.1.32
    /// </summary>
    public class GetVersionsSubResponseType : SubResponseType
    {
        public GetVersionsResponseType GetVersionsResponse { get; set; }
    }

    /// <summary>
    /// GetVersionsResponseType defined in MS-VERSS section 3.1.4.3.2.2
    /// </summary>
    public class GetVersionsResponseType
    {
        public GetVersionsResult GetVersionsResult { get; set; }
    }

    /// <summary>
    /// GetVersionsResult defined in MS-VERSS section 3.1.4.3.2.2
    /// </summary>
    public class GetVersionsResult
    {
        public Results results { get; set; }
    }

    /// <summary>
    /// The Result complex type, defined in MS-VERSS section 2.2.4.1
    /// </summary>
    public class Results
    {
        public ResultsList list { get; set; }

        public ResultsVersioning versioning { get; set; }

        public ResultsSettings settings { get; set; }

        [XmlElement()]
        public VersionData[] result { get; set; }
    }

    /// <summary>
    /// The ResultsList type, defined in MS-VERSS section 2.2.4.1
    /// </summary>
    public class ResultsList
    {
        [XmlAttribute()]
        public string id { get; set; }
    }

    /// <summary>
    /// The ResultsVersioning type, defined in MS-VERSS section 2.2.4.1
    /// </summary>
    public class ResultsVersioning
    {
        [XmlAttribute()]
        public byte enabled { get; set; }
    }

    /// <summary>
    /// The ResultsSettings type, defined in MS-VERSS section 2.2.4.1
    /// </summary>
    public class ResultsSettings
    {
        [XmlAttribute()]
        public string url { get; set; }
    }

    /// <summary>
    /// The VersionData type, defined in MS-VERSS section 2.2.4.1
    /// </summary>
    public class VersionData
    {
        [XmlAttribute()]
        public string version { get; set; }

        [XmlAttribute()]
        public string url { get; set; }

        [XmlAttribute()]
        public string created { get; set; }

        [XmlAttribute()]
        public string createdRaw { get; set; }

        [XmlAttribute()]
        public string createdBy { get; set; }

        [XmlAttribute()]
        public string createdByName { get; set; }

        [XmlAttribute()]
        public ulong size { get; set; }

        [XmlAttribute()]
        public string comments { get; set; }
    }

    /// <summary>
    /// The CoauthRequestTypes is used to represent the type of coauthoring subrequest. Defined in 2.3.2.2
    /// </summary>
    public enum CoauthRequestTypes
    {
        JoinCoauthoring,
        ExitCoauthoring,
        RefreshCoauthoring,
        ConvertToExclusive,
        CheckLockAvailability,
        MarkTransitionComplete,
        GetCoauthoringStatus,
    }

    /// <summary>
    /// The ExclusiveLockRequestTypes is used to represent the type of exclusive lock subrequest. Define in 2.3.2.3
    /// </summary>
    public enum ExclusiveLockRequestTypes
    {
        GetLock,
        ReleaseLock,
        RefreshLock,
        ConvertToSchemaJoinCoauth,
        ConvertToSchema,
        CheckLockAvailability,
    }

    /// <summary>
    /// The SchemaLockRequestTypes is used to represent the type of schema lock subrequest. Defined in 2.3.2.4
    /// </summary>
    public enum SchemaLockRequestTypes
    {
        GetLock,
        ReleaseLock,
        RefreshLock,
        ConvertToExclusive,
        CheckLockAvailability,
    }

    /// <summary>
    /// The EditorsTableRequestType is used to represent the type of editors table subrequest. Defined in 2.3.2.5
    /// </summary>
    public enum EditorsTableRequestTypes
    {
        JoinEditingSession,
        LeaveEditingSession,
        RefreshEditingSession,
        UpdateEditorMetadata,
        RemoveEditorMetadata,
    }

    /// <summary>
    /// The FileOperationRequestTypes is used to represent the type of file operation subrequest. Defined in 2.3.2.8
    /// </summary>
    public enum FileOperationRequestTypes
    {
        Rename,
    }

    /// <summary>
    /// The VersioningRequestTypes is used to represent the type of Versioning subrequest. Defined in 2.3.2.9
    /// </summary>
    public enum VersioningRequestTypes
    {
        GetVersionList,
        RestoreVersion,
    }

    /// <summary>
    ///  Represent the type of cell storage service subrequest.Defined in 2.2.5.11
    /// </summary>
    public enum SubRequestAttributeType
    {
        Cell,
        Coauth,
        SchemaLock,
        WhoAmI,
        ServerTime,
        ExclusiveLock,
        GetDocMetaInfo,
        GetVersions,
        EditorsTable,
        AmIAlone,
        LockStatus,
        FileOperation,
        Versioning,
        Properties,
    }

    /// <summary>
    /// Represent the type of dependency that a cell storage service subrequest has on another cell storage service subrequest. Defined in 2.2.5.3
    /// </summary>
    public enum DependencyTypes
    {
        OnExecute,
        OnSuccess,
        OnFail,
        OnNotSupported,
        OnSuccessOrNotSupported,
        Invalid, //MSFSSHTTP2010 #531
    }
}