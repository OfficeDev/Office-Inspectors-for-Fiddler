namespace MAPIInspector.Parsers
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The enum of Tag Property ID related to Canonical name.
    /// </summary>
    public enum PidTagPropertyEnum : ushort
    {
        /// <summary>
        /// Indicates the operations available to the client for the object.
        /// </summary>
        PidTagAccess = 0x0FF4,

        /// <summary>
        /// Contains a permissions list for a folder.
        /// </summary>
        PidTagAccessControlListData = 0x3FE0,

        /// <summary>
        /// Indicates the client's access level to the object.
        /// </summary>
        PidTagAccessLevel = 0x0FF7,

        /// <summary>
        /// Contains the alias of an Address Book object, which is an alternative name by which the object can be identified.
        /// </summary>
        PidTagAccount = 0x3A00,

        /// <summary>
        /// Contains the indexed entry IDs for several special folders related to conflicts, sync issues, local failures, server failures, junk email and spam.
        /// </summary>
        PidTagAdditionalRenEntryIds = 0x36D8,

        /// <summary>
        /// Contains an array of blocks that specify the EntryIDs of several special folders.
        /// </summary>
        PidTagAdditionalRenEntryIdsEx = 0x36D9,

        /// <summary>
        /// Indicates whether delivery restrictions exist for a recipient.
        /// </summary>
        PidTagAddressBookAuthorizedSenders = 0x8CD8,

        /// <summary>
        /// Contains the ID of a container on an NSPI server.
        /// </summary>
        PidTagAddressBookContainerId = 0xFFFD,

        /// <summary>
        /// Specifies the maximum size, in bytes, of a message that a recipient can receive.
        /// </summary>
        PidTagAddressBookDeliveryContentLength = 0x806A,

        /// <summary>
        /// Contains the printable string version of the display name.
        /// </summary>
        PidTagAddressBookDisplayNamePrintable = 0x39FF,

        /// <summary>
        /// Contains a value that indicates how to display an Address Book object in a table or as a recipient on a message.
        /// </summary>
        PidTagAddressBookDisplayTypeExtended = 0x8C93,

        /// <summary>
        /// Contains the number of external recipients in the distribution list.
        /// </summary>
        PidTagAddressBookDistributionListExternalMemberCount = 0x8CE3,

        /// <summary>
        /// Contains the total number of recipients in the distribution list.
        /// </summary>
        PidTagAddressBookDistributionListMemberCount = 0x8CE2,

        /// <summary>
        /// Indicates that delivery restrictions exist for a recipient.
        /// </summary>
        PidTagAddressBookDistributionListMemberSubmitAccepted = 0x8073,

        /// <summary>
        /// Indicates that delivery restrictions exist for a recipient.
        /// </summary>
        PidTagAddressBookDistributionListMemberSubmitRejected = 0x8CDA,

        /// <summary>
        /// Indicates that delivery restrictions exist for a recipient.
        /// </summary>
        PidTagAddressBookDistributionListRejectMessagesFromDLMembers = 0x8CDB,

        /// <summary>
        /// Contains the name-service EntryID of a directory object that refers to a public folder.
        /// </summary>
        PidTagAddressBookEntryId = 0x663B,

        /// <summary>
        /// Contains custom values defined and populated by the organization that modified the display templates.
        /// </summary>
        PidTagAddressBookExtensionAttribute1 = 0x802D,

        /// <summary>
        /// Contains custom values defined and populated by the organization that modified the display templates.
        /// </summary>
        PidTagAddressBookExtensionAttribute10 = 0x8036,

        /// <summary>
        /// Contains custom values defined and populated by the organization that modified the display templates.
        /// </summary>
        PidTagAddressBookExtensionAttribute11 = 0x8C57,

        /// <summary>
        /// Contains custom values defined and populated by the organization that modified the display templates.
        /// </summary>
        PidTagAddressBookExtensionAttribute12 = 0x8C58,

        /// <summary>
        /// Contains custom values defined and populated by the organization that modified the display templates.
        /// </summary>
        PidTagAddressBookExtensionAttribute13 = 0x8C59,

        /// <summary>
        /// Contains custom values defined and populated by the organization that modified the display templates.
        /// </summary>
        PidTagAddressBookExtensionAttribute14 = 0x8C60,

        /// <summary>
        /// Contains custom values defined and populated by the organization that modified the display templates.
        /// </summary>
        PidTagAddressBookExtensionAttribute15 = 0x8C61,

        /// <summary>
        /// Contains custom values defined and populated by the organization that modified the display templates.
        /// </summary>
        PidTagAddressBookExtensionAttribute2 = 0x802E,

        /// <summary>
        /// Contains custom values defined and populated by the organization that modified the display templates.
        /// </summary>
        PidTagAddressBookExtensionAttribute3 = 0x802F,

        /// <summary>
        /// Contains custom values defined and populated by the organization that modified the display templates.
        /// </summary>
        PidTagAddressBookExtensionAttribute4 = 0x8030,

        /// <summary>
        /// Contains custom values defined and populated by the organization that modified the display templates.
        /// </summary>
        PidTagAddressBookExtensionAttribute5 = 0x8031,

        /// <summary>
        /// Contains custom values defined and populated by the organization that modified the display templates.
        /// </summary>
        PidTagAddressBookExtensionAttribute6 = 0x8032,

        /// <summary>
        /// Contains custom values defined and populated by the organization that modified the display templates.
        /// </summary>
        PidTagAddressBookExtensionAttribute7 = 0x8033,

        /// <summary>
        /// Contains custom values defined and populated by the organization that modified the display templates.
        /// </summary>
        PidTagAddressBookExtensionAttribute8 = 0x8034,

        /// <summary>
        /// Contains custom values defined and populated by the organization that modified the display templates.
        /// </summary>
        PidTagAddressBookExtensionAttribute9 = 0x8035,

        /// <summary>
        /// This property is deprecated and is to be ignored.
        /// </summary>
        PidTagAddressBookFolderPathname = 0x8004,

        /// <summary>
        /// Contains the child departments in a hierarchy of departments.
        /// </summary>
        PidTagAddressBookHierarchicalChildDepartments = 0x8C9A,

        /// <summary>
        /// Contains all of the mail users that belong to this department.
        /// </summary>
        PidTagAddressBookHierarchicalDepartmentMembers = 0x8C97,

        /// <summary>
        /// Indicates whether the distribution list represents a departmental group.
        /// </summary>
        PidTagAddressBookHierarchicalIsHierarchicalGroup = 0x8CDD,

        /// <summary>
        /// Contains all of the departments to which this department is a child.
        /// </summary>
        PidTagAddressBookHierarchicalParentDepartment = 0x8C99,

        /// <summary>
        /// Contains the distinguished name (DN) of either the root Department object or the root departmental group in the department hierarchy for the organization.
        /// </summary>
        PidTagAddressBookHierarchicalRootDepartment = 0x8C98,

        /// <summary>
        /// Lists all Department objects of which the mail user is a member.
        /// </summary>
        PidTagAddressBookHierarchicalShowInDepartments = 0x8C94,

        /// <summary>
        /// Contains the DN expressed in the X500 DN format. This property is returned from a name service provider interface (NSPI) server as a PtypEmbeddedTable. Otherwise, the data type is PtypString8.
        /// </summary>
        PidTagAddressBookHomeMessageDatabase = 0x8006,

        /// <summary>
        /// Contains a Boolean value of TRUE if it is possible to create Address Book objects in that container, and FALSE otherwise.
        /// </summary>
        PidTagAddressBookIsMaster = 0xFFFB,

        /// <summary>
        /// Lists all of the distribution lists for which the object is a member. This property is returned from an NSPI server as a PtypEmbeddedTable. Otherwise, the data type is PtypString8.
        /// </summary>
        PidTagAddressBookIsMemberOfDistributionList = 0x8008,

        /// <summary>
        /// Contains information for use in display templates for distribution lists.
        /// </summary>
        PidTagAddressBookManageDistributionList = 0x6704,

        /// <summary>
        /// Contains one row that references the mail user's manager.
        /// </summary>
        PidTagAddressBookManager = 0x8005,

        /// <summary>
        /// Contains the DN of the mail user's manager.
        /// </summary>
        PidTagAddressBookManagerDistinguishedName = 0x8005,

        /// <summary>
        /// Contains the members of the distribution list.
        /// </summary>
        PidTagAddressBookMember = 0x8009,

        /// <summary>
        /// Contains the Short-term Message ID (MID) ([MS-OXCDATA] section 2.2.1.2) of the first message in the local site's offline address book public folder.
        /// </summary>
        PidTagAddressBookMessageId = 0x674F,

        /// <summary>
        /// Indicates whether moderation is enabled for the mail user or distribution list.
        /// </summary>
        PidTagAddressBookModerationEnabled = 0x8CB5,

        /// <summary>
        /// Contains a list of names by which a server is known to the various transports in use by the network.
        /// </summary>
        PidTagAddressBookNetworkAddress = 0x8170,

        /// <summary>
        /// Contains the DN of the Address Book object.
        /// </summary>
        PidTagAddressBookObjectDistinguishedName = 0x803C,

        /// <summary>
        /// Contains a GUID that identifies an Address Book object.
        /// </summary>
        PidTagAddressBookObjectGuid = 0x8C6D,

        /// <summary>
        /// Contains the DN of the Organization object of the mail user's organization.
        /// </summary>
        PidTagAddressBookOrganizationalUnitRootDistinguishedName = 0x8CA8,

        /// <summary>
        /// Contains one row that references the distribution list's owner.
        /// </summary>
        PidTagAddressBookOwner = 0x800C,

        /// <summary>
        /// Contains a list of the distribution lists owned by a mail user.
        /// </summary>
        PidTagAddressBookOwnerBackLink = 0x8024,

        /// <summary>
        /// Contains the EntryID of the parent container in a hierarchy of address book containers.
        /// </summary>
        PidTagAddressBookParentEntryId = 0xFFFC,

        /// <summary>
        /// Contains the phonetic representation of the PidTagCompanyName property (section 2.630).
        /// </summary>
        PidTagAddressBookPhoneticCompanyName = 0x8C91,

        /// <summary>
        /// Contains the phonetic representation of the PidTagDepartmentName property (section 2.663).
        /// </summary>
        PidTagAddressBookPhoneticDepartmentName = 0x8C90,

        /// <summary>
        /// Contains the phonetic representation of the PidTagDisplayName property (section 2.667).
        /// </summary>
        PidTagAddressBookPhoneticDisplayName = 0x8C92,

        /// <summary>
        /// Contains the phonetic representation of the PidTagGivenName property (section 2.705).
        /// </summary>
        PidTagAddressBookPhoneticGivenName = 0x8C8E,

        /// <summary>
        /// Contains the phonetic representation of the PidTagSurname property (section 2.1026).
        /// </summary>
        PidTagAddressBookPhoneticSurname = 0x8C8F,

        /// <summary>
        /// Contains alternate email addresses for the Address Book object.
        /// </summary>
        PidTagAddressBookProxyAddresses = 0x800F,

        /// <summary>
        /// Contains a list of mail users who are allowed to send email on behalf of the mailbox owner.
        /// </summary>
        PidTagAddressBookPublicDelegates = 0x8015,

        /// <summary>
        /// Lists all of the mail user’s direct reports.
        /// </summary>
        PidTagAddressBookReports = 0x800E,

        /// <summary>
        /// Contains the maximum occupancy of the room.
        /// </summary>
        PidTagAddressBookRoomCapacity = 0x0807,

        /// <summary>
        /// Contains a list of DNs that represent the address book containers that hold Resource objects, such as conference rooms and equipment.
        /// </summary>
        PidTagAddressBookRoomContainers = 0x8C96,

        /// <summary>
        /// Contains a description of the Resource object.
        /// </summary>
        PidTagAddressBookRoomDescription = 0x0809,

        /// <summary>
        /// Contains the locale ID and translations of the default mail tip.
        /// </summary>
        PidTagAddressBookSenderHintTranslations = 0x8CAC,

        /// <summary>
        /// Contains a signed integer that specifies the seniority order of Address Book objects that represent members of a department and are referenced by a Department object or departmental group, with larger values specifying members that are more senior.
        /// </summary>
        PidTagAddressBookSeniorityIndex = 0x8CA0,

        /// <summary>
        /// Contains the foreign system email address of an Address Book object.
        /// </summary>
        PidTagAddressBookTargetAddress = 0x8011,

        /// <summary>
        /// Indicates whether delivery restrictions exist for a recipient.
        /// </summary>
        PidTagAddressBookUnauthorizedSenders = 0x8CD9,

        /// <summary>
        /// Contains the ASN_1 DER encoded X.509 certificates for the mail user.
        /// </summary>
        PidTagAddressBookX509Certificate = 0x8C6A,

        /// <summary>
        /// Contains the email address type of a Message object.
        /// </summary>
        PidTagAddressType = 0x3002,

        /// <summary>
        /// Specifies whether the sender permits the message to be auto-forwarded.
        /// </summary>
        PidTagAlternateRecipientAllowed = 0x0002,

        /// <summary>
        /// Contains a filter value used in ambiguous name resolution.
        /// </summary>
        PidTagAnr = 0x360C,

        /// <summary>
        /// Specifies the date, in UTC, after which a Message object is archived by the server.
        /// </summary>
        PidTagArchiveDate = 0x301F,

        /// <summary>
        /// Specifies the number of days that a Message object can remain unarchived.
        /// </summary>
        PidTagArchivePeriod = 0x301E,

        /// <summary>
        /// Specifies the GUID of an archive tag.
        /// </summary>
        PidTagArchiveTag = 0x3018,

        /// <summary>
        /// Contains the name of the mail user's administrative assistant.
        /// </summary>
        PidTagAssistant = 0x3A30,

        /// <summary>
        /// Contains the telephone number of the mail user's administrative assistant.
        /// </summary>
        PidTagAssistantTelephoneNumber = 0x3A2E,

        /// <summary>
        /// Specifies whether the message being synchronized is an FAI message.
        /// </summary>
        PidTagAssociated = 0x67AA,

        /// <summary>
        /// Contains attachment encoding information.
        /// </summary>
        PidTagAttachAdditionalInformation = 0x370F,

        /// <summary>
        /// Contains the base of a relative URI.
        /// </summary>
        PidTagAttachContentBase = 0x3711,

        /// <summary>
        /// Contains a content identifier unique to the Message object that matches a corresponding "cid:" URI schema reference in the HTML body of the Message object.
        /// </summary>
        PidTagAttachContentId = 0x3712,

        /// <summary>
        /// Contains a relative or full URI that matches a corresponding reference in the HTML body of a Message object.
        /// </summary>
        PidTagAttachContentLocation = 0x3713,

        /// <summary>
        /// Contains the contents of the file to be attached.
        /// </summary>
        PidTagAttachDataBinary = 0x3701,

        /// <summary>
        /// Contains the binary representation of the Attachment object in an application-specific format.
        /// </summary>
        PidTagAttachDataObject = 0x3701,

        /// <summary>
        /// Contains encoding information about the Attachment object.
        /// </summary>
        PidTagAttachEncoding = 0x3702,

        /// <summary>
        /// Contains a file name extension that indicates the document type of an attachment.
        /// </summary>
        PidTagAttachExtension = 0x3703,

        /// <summary>
        /// Contains the 8.3 name of the PidTagAttachLongFilename property (section 2.586).
        /// </summary>
        PidTagAttachFilename = 0x3704,

        /// <summary>
        /// Indicates which body formats might reference this attachment when rendering data.
        /// </summary>
        PidTagAttachFlags = 0x3714,

        /// <summary>
        /// Contains the full filename and extension of the Attachment object.
        /// </summary>
        PidTagAttachLongFilename = 0x3707,

        /// <summary>
        /// Contains the fully-qualified path and file name with extension.
        /// </summary>
        PidTagAttachLongPathname = 0x370D,

        /// <summary>
        /// Indicates that a contact photo attachment is attached to a Contact object.
        /// </summary>
        PidTagAttachmentContactPhoto = 0x7FFF,

        /// <summary>
        /// Indicates special handling for an Attachment object.
        /// </summary>
        PidTagAttachmentFlags = 0x7FFD,

        /// <summary>
        /// Indicates whether an Attachment object is hidden from the end user.
        /// </summary>
        PidTagAttachmentHidden = 0x7FFE,

        /// <summary>
        /// Contains the type of Message object to which an attachment is linked.
        /// </summary>
        PidTagAttachmentLinkId = 0x7FFA,

        /// <summary>
        /// Represents the way the contents of an attachment are accessed.
        /// </summary>
        PidTagAttachMethod = 0x3705,

        /// <summary>
        /// Contains a content-type MIME header.
        /// </summary>
        PidTagAttachMimeTag = 0x370E,

        /// <summary>
        /// Identifies the Attachment object within its Message object.
        /// </summary>
        PidTagAttachNumber = 0x0E21,

        /// <summary>
        /// Contains the 8.3 name of the PidTagAttachLongPathname property (section 2.587).
        /// </summary>
        PidTagAttachPathname = 0x3708,

        /// <summary>
        /// Contains the class name of an object that can display the contents of the message.
        /// </summary>
        PidTagAttachPayloadClass = 0x371A,

        /// <summary>
        /// Contains the GUID of the software component that can display the contents of the message.
        /// </summary>
        PidTagAttachPayloadProviderGuidString = 0x3719,

        /// <summary>
        /// Contains a Windows Metafile, as specified in [MS-WMF], for the Attachment object.
        /// </summary>
        PidTagAttachRendering = 0x3709,

        /// <summary>
        /// Contains the size, in bytes, consumed by the Attachment object on the server.
        /// </summary>
        PidTagAttachSize = 0x0E20,

        /// <summary>
        /// Contains the identifier information for the application that supplied the Attachment object data.
        /// </summary>
        PidTagAttachTag = 0x370A,

        /// <summary>
        /// Contains the name of an attachment file, modified so that it can be correlated with TNEF messages.
        /// </summary>
        PidTagAttachTransportName = 0x370C,

        /// <summary>
        /// Specifies the hide or show status of a folder.
        /// </summary>
        PidTagAttributeHidden = 0x10F4,

        /// <summary>
        /// Indicates whether an item can be modified or deleted.
        /// </summary>
        PidTagAttributeReadOnly = 0x10F6,

        /// <summary>
        /// Contains text included in an automatically-generated message.
        /// </summary>
        PidTagAutoForwardComment = 0x0004,

        /// <summary>
        /// Indicates that a Message object has been automatically generated or automatically forwarded.
        /// </summary>
        PidTagAutoForwarded = 0x0005,

        /// <summary>
        /// Specifies whether a client or server application should forego sending automated replies in response to this message.
        /// </summary>
        PidTagAutoResponseSuppress = 0x3FDF,

        /// <summary>
        /// Contains the date of the mail user's birthday at midnight.
        /// </summary>
        PidTagBirthday = 0x3A42,

        /// <summary>
        /// Indicates the user's preference for viewing external content (such as links to images on an HTTP server) in the message body.
        /// </summary>
        PidTagBlockStatus = 0x1096,

        /// <summary>
        /// Contains message body text in plain text format.
        /// </summary>
        PidTagBody = 0x1000,

        /// <summary>
        /// Contains a GUID that corresponds to the current message body.
        /// </summary>
        PidTagBodyContentId = 0x1015,

        /// <summary>
        /// Contains a globally unique Uniform Resource Identifier (URI) that serves as a label for the current message body.
        /// </summary>
        PidTagBodyContentLocation = 0x1014,

        /// <summary>
        /// Contains the HTML body of the Message object.
        /// </summary>
        PidTagBodyHtml = 0x1013,

        /// <summary>
        /// Contains a secondary telephone number at the mail user's place of business.
        /// </summary>
        PidTagBusiness2TelephoneNumber = 0x3A1B,

        /// <summary>
        /// Contains secondary telephone numbers at the mail user's place of business.
        /// </summary>
        PidTagBusiness2TelephoneNumbers = 0x3A1B,

        /// <summary>
        /// Contains the telephone number of the mail user's business fax machine.
        /// </summary>
        PidTagBusinessFaxNumber = 0x3A24,

        /// <summary>
        /// Contains the URL of the mail user's business home page.
        /// </summary>
        PidTagBusinessHomePage = 0x3A51,

        /// <summary>
        /// Contains the primary telephone number of the mail user's place of business.
        /// </summary>
        PidTagBusinessTelephoneNumber = 0x3A08,

        /// <summary>
        /// Contains a telephone number to reach the mail user.
        /// </summary>
        PidTagCallbackTelephoneNumber = 0x3A02,

        /// <summary>
        /// Contains a unique identifier associated with the phone call.
        /// </summary>
        PidTagCallId = 0x6806,

        /// <summary>
        /// Contains the mail user's car telephone number.
        /// </summary>
        PidTagCarTelephoneNumber = 0x3A1E,

        /// <summary>
        /// Identifies a specific instance of a recurring appointment.
        /// </summary>
        PidTagCdoRecurrenceid = 0x10C5,

        /// <summary>
        /// Contains a structure that identifies the last change to the object.
        /// </summary>
        PidTagChangeKey = 0x65E2,

        /// <summary>
        /// Contains a structure that identifies the last change to the message or folder that is currently being synchronized.
        /// </summary>
        PidTagChangeNumber = 0x67A4,

        /// <summary>
        /// Specifies the names of the children of the contact.
        /// </summary>
        PidTagChildrensNames = 0x3A58,

        /// <summary>
        /// Specifies the actions the client is required to take on the message.
        /// </summary>
        PidTagClientActions = 0x6645,

        /// <summary>
        /// Contains the current time, in UTC, when the email message is submitted.
        /// </summary>
        PidTagClientSubmitTime = 0x0039,

        /// <summary>
        /// Contains the identifier for the client code page used for Unicode to double-byte character set (DBCS) string conversion.
        /// </summary>
        PidTagCodePageId = 0x66C3,

        /// <summary>
        /// Contains a comment about the purpose or content of the Address Book object.
        /// </summary>
        PidTagComment = 0x3004,

        /// <summary>
        /// Contains the main telephone number of the mail user's company.
        /// </summary>
        PidTagCompanyMainTelephoneNumber = 0x3A57,

        /// <summary>
        /// Contains the mail user's company name.
        /// </summary>
        PidTagCompanyName = 0x3A16,

        /// <summary>
        /// Contains the name of the mail user's computer network.
        /// </summary>
        PidTagComputerNetworkName = 0x3A49,

        /// <summary>
        /// Contains the EntryID of the conflict resolve message.
        /// </summary>
        PidTagConflictEntryId = 0x3FF0,

        /// <summary>
        /// Contains a string value that describes the type of Message object that a folder contains.
        /// </summary>
        PidTagContainerClass = 0x3613,

        /// <summary>
        /// Always empty. An NSPI server defines this value for distribution lists and it is not present for other objects.
        /// </summary>
        PidTagContainerContents = 0x360F,

        /// <summary>
        /// Contains a bitmask of flags that describe capabilities of an address book container.
        /// </summary>
        PidTagContainerFlags = 0x3600,

        /// <summary>
        /// Identifies all of the subfolders of the current folder.
        /// </summary>
        PidTagContainerHierarchy = 0x360E,

        /// <summary>
        /// Specifies the number of rows under the header row.
        /// </summary>
        PidTagContentCount = 0x3602,

        /// <summary>
        /// Indicates a confidence level that the message is spam.
        /// </summary>
        PidTagContentFilterSpamConfidenceLevel = 0x4076,

        /// <summary>
        /// Specifies the number of rows under the header row that have the PidTagRead property (section 2.869) set to FALSE.
        /// </summary>
        PidTagContentUnreadCount = 0x3603,

        /// <summary>
        /// Contains a computed value derived from other conversation-related properties. 
        /// </summary>
        PidTagConversationId = 0x3013,

        /// <summary>
        /// Indicates the relative position of this message within a conversation thread.
        /// </summary>
        PidTagConversationIndex = 0x0071,

        /// <summary>
        /// Indicates whether the GUID portion of the PidTagConversationIndex property (section 2.641) is to be used to compute the PidTagConversationId property (section 2.640).
        /// </summary>
        PidTagConversationIndexTracking = 0x3016,

        /// <summary>
        /// Contains an unchanging copy of the original subject.
        /// </summary>
        PidTagConversationTopic = 0x0070,

        /// <summary>
        /// Contains the name of the mail user's country/region.
        /// </summary>
        PidTagCountry = 0x3A26,

        /// <summary>
        /// Contains the time, in UTC, that the object was created.
        /// </summary>
        PidTagCreationTime = 0x3007,

        /// <summary>
        /// Specifies the original author of the message according to their Address Book EntryID.
        /// </summary>
        PidTagCreatorEntryId = 0x3FF9,

        /// <summary>
        /// Contains the name of the creator of a Message object.
        /// </summary>
        PidTagCreatorName = 0x3FF8,

        /// <summary>
        /// Contains the mail user's customer identification number.
        /// </summary>
        PidTagCustomerId = 0x3A4A,

        /// <summary>
        /// Indicates whether the Deferred Action Message (DAM) was updated by the server.
        /// </summary>
        PidTagDamBackPatched = 0x6647,

        /// <summary>
        /// Contains the EntryID of the delivered message that the client has to process.
        /// </summary>
        PidTagDamOriginalEntryId = 0x6646,

        /// <summary>
        /// Contains the message class of the object.
        /// </summary>
        PidTagDefaultPostMessageClass = 0x36E5,

        /// <summary>
        /// Contains the server EntryID for the DAM.
        /// </summary>
        PidTagDeferredActionMessageOriginalEntryId = 0x6741,

        /// <summary>
        /// Contains the date and time, in UTC, at which the sender prefers that the message be delivered.
        /// </summary>
        PidTagDeferredDeliveryTime = 0x000F,

        /// <summary>
        /// Contains a number used in the calculation of how long to defer sending a message.
        /// </summary>
        PidTagDeferredSendNumber = 0x3FEB,

        /// <summary>
        /// Contains the amount of time after which a client would like to defer sending the message.
        /// </summary>
        PidTagDeferredSendTime = 0x3FEF,

        /// <summary>
        /// Specifies the unit of time used as a multiplier with the PidTagDeferredSendNumber property (section 2.654) value.
        /// </summary>
        PidTagDeferredSendUnits = 0x3FEC,

        /// <summary>
        /// Specifies whether the message was forwarded due to the triggering of a delegate forward rule.
        /// </summary>
        PidTagDelegatedByRule = 0x3FE3,

        /// <summary>
        /// Indicates whether delegates can view Message objects that are marked as private.
        /// </summary>
        PidTagDelegateFlags = 0x686B,

        /// <summary>
        /// Indicates that the original message is to be deleted after it is sent.
        /// </summary>
        PidTagDeleteAfterSubmit = 0x0E01,

        /// <summary>
        /// Contains the total count of messages that have been deleted from a folder, excluding messages deleted within subfolders.
        /// </summary>
        PidTagDeletedCountTotal = 0x670B,

        /// <summary>
        /// Specifies the time, in UTC, when the item or folder was soft deleted.
        /// </summary>
        PidTagDeletedOn = 0x668F,

        /// <summary>
        /// Contains the delivery time for a delivery status notification, as specified [RFC3464], or a message disposition notification, as specified in [RFC3798].
        /// </summary>
        PidTagDeliverTime = 0x0010,

        /// <summary>
        /// Contains a name for the department in which the mail user works.
        /// </summary>
        PidTagDepartmentName = 0x3A18,

        /// <summary>
        /// Specifies the number of nested categories in which a given row is contained.
        /// </summary>
        PidTagDepth = 0x3005,

        /// <summary>
        /// Contains a list of blind carbon copy (Bcc) recipient display names.
        /// </summary>
        PidTagDisplayBcc = 0x0E02,

        /// <summary>
        /// Contains a list of carbon copy (Cc) recipient display names.
        /// </summary>
        PidTagDisplayCc = 0x0E03,

        /// <summary>
        /// Contains the display name of the folder.
        /// </summary>
        PidTagDisplayName = 0x3001,

        /// <summary>
        /// Contains the mail user's honorific title.
        /// </summary>
        PidTagDisplayNamePrefix = 0x3A45,

        /// <summary>
        /// Contains a list of the primary recipient display names, separated by semicolons, when an email message has primary recipients .
        /// </summary>
        PidTagDisplayTo = 0x0E04,

        /// <summary>
        /// Contains an integer value that indicates how to display an Address Book object in a table or as an addressee on a message.
        /// </summary>
        PidTagDisplayType = 0x3900,

        /// <summary>
        /// Contains an integer value that indicates how to display an Address Book object in a table or as a recipient on a message.
        /// </summary>
        PidTagDisplayTypeEx = 0x3905,

        /// <summary>
        /// Contains the email address of a Message object.
        /// </summary>
        PidTagEmailAddress = 0x3003,

        /// <summary>
        /// Contains the value of the PidLidAppointmentEndWhole property (section 2.14).
        /// </summary>
        PidTagEndDate = 0x0061,

        /// <summary>
        /// Contains the information to identify many different types of messaging objects.
        /// </summary>
        PidTagEntryId = 0x0FFF,

        /// <summary>
        /// Contains the end date and time of the exception in the local time zone of the computer when the exception is created.
        /// </summary>
        PidTagExceptionEndTime = 0x7FFC,

        /// <summary>
        /// Indicates the original date and time, in UTC, at which the instance in the recurrence pattern would have occurred if it were not an exception.
        /// </summary>
        PidTagExceptionReplaceTime = 0x7FF9,

        /// <summary>
        /// Contains the start date and time of the exception in the local time zone of the computer when the exception is created.
        /// </summary>
        PidTagExceptionStartTime = 0x7FFB,

        /// <summary>
        /// Contains the calculated security descriptor for the item.
        /// </summary>
        PidTagExchangeNTSecurityDescriptor = 0x0E84,

        /// <summary>
        /// Contains an integer value that is used along with the PidTagExpiryUnits property (section 2.681) to define the expiry send time.
        /// </summary>
        PidTagExpiryNumber = 0x3FED,

        /// <summary>
        /// Contains the time, in UTC, after which a client wants to receive an expiry event if the message arrives late.
        /// </summary>
        PidTagExpiryTime = 0x0015,

        /// <summary>
        /// Contains the unit of time that the value of the PidTagExpiryNumber property (section 2.679) multiplies.
        /// </summary>
        PidTagExpiryUnits = 0x3FEE,

        /// <summary>
        /// Contains encoded sub-properties for a folder.
        /// </summary>
        PidTagExtendedFolderFlags = 0x36DA,

        /// <summary>
        /// Contains action information about named properties used in the rule.
        /// </summary>
        PidTagExtendedRuleMessageActions = 0x0E99,

        /// <summary>
        /// Contains condition information about named properties used in the rule.
        /// </summary>
        PidTagExtendedRuleMessageCondition = 0x0E9A,

        /// <summary>
        /// Contains the maximum size, in bytes, that the user is allowed to accumulate for a single extended rule.
        /// </summary>
        PidTagExtendedRuleSizeLimit = 0x0E9B,

        /// <summary>
        /// Contains the number of pages in a Fax object.
        /// </summary>
        PidTagFaxNumberOfPages = 0x6804,

        /// <summary>
        /// Specifies the date and time, in UTC, that the Message object was flagged as complete.
        /// </summary>
        PidTagFlagCompleteTime = 0x1091,

        /// <summary>
        /// Specifies the flag state of the Message object.
        /// </summary>
        PidTagFlagStatus = 0x1090,

        /// <summary>
        /// Contains a unique identifier for an item across the message store.
        /// </summary>
        PidTagFlatUrlName = 0x670E,

        /// <summary>
        /// Identifies all FAI messages in the current folder.
        /// </summary>
        PidTagFolderAssociatedContents = 0x3610,

        /// <summary>
        /// Contains the Folder ID (FID) ([MS-OXCDATA] section 2.2.1.1) of the folder.
        /// </summary>
        PidTagFolderId = 0x6748,

        /// <summary>
        /// Specifies the type of a folder that includes the Root folder, Generic folder, and Search folder.
        /// </summary>
        PidTagFolderType = 0x3601,

        /// <summary>
        /// Specifies the flag color of the Message object.
        /// </summary>
        PidTagFollowupIcon = 0x1095,

        /// <summary>
        /// Contains an integer value used to calculate the start and end dates of the range of free/busy data to be published to the public folders.
        /// </summary>
        PidTagFreeBusyCountMonths = 0x6869,

        /// <summary>
        /// Contains EntryIDs of the Delegate Information object, the free/busy message of the logged on user, and the folder with the PidTagDisplayName property (section 2.667) value of "Freebusy Data".
        /// </summary>
        PidTagFreeBusyEntryIds = 0x36E4,

        /// <summary>
        /// Specifies the email address of the user or resource to whom this free/busy message applies.
        /// </summary>
        PidTagFreeBusyMessageEmailAddress = 0x6849,

        /// <summary>
        /// Specifies the end time, in UTC, of the publishing range.
        /// </summary>
        PidTagFreeBusyPublishEnd = 0x6848,

        /// <summary>
        /// Specifies the start time, in UTC, of the publishing range.
        /// </summary>
        PidTagFreeBusyPublishStart = 0x6847,

        /// <summary>
        /// Specifies the time, in UTC, that the data was published.
        /// </summary>
        PidTagFreeBusyRangeTimestamp = 0x6868,

        /// <summary>
        /// Contains the File Transfer Protocol (FTP) site address of the mail user.
        /// </summary>
        PidTagFtpSite = 0x3A4C,

        /// <summary>
        /// This property is deprecated and SHOULD NOT be used.
        /// </summary>
        PidTagGatewayNeedsToRefresh = 0x6846,

        /// <summary>
        /// Contains a value that represents the mail user's gender.
        /// </summary>
        PidTagGender = 0x3A4D,

        /// <summary>
        /// Contains a generational abbreviation that follows the full name of the mail user.
        /// </summary>
        PidTagGeneration = 0x3A05,

        /// <summary>
        /// Contains the mail user's given name.
        /// </summary>
        PidTagGivenName = 0x3A06,

        /// <summary>
        /// Contains a government identifier for the mail user.
        /// </summary>
        PidTagGovernmentIdNumber = 0x3A07,

        /// <summary>
        /// Indicates whether the Message object contains at least one attachment.
        /// </summary>
        PidTagHasAttachments = 0x0E1B,

        /// <summary>
        /// Indicates whether a Message object has a deferred action message associated with it.
        /// </summary>
        PidTagHasDeferredActionMessages = 0x3FEA,

        /// <summary>
        /// Indicates whether the Message object has a named property.
        /// </summary>
        PidTagHasNamedProperties = 0x664A,

        /// <summary>
        /// Indicates whether a Folder object has rules.
        /// </summary>
        PidTagHasRules = 0x663A,

        /// <summary>
        /// Contains a number that monotonically increases every time a subfolder is added to, or deleted from, this folder.
        /// </summary>
        PidTagHierarchyChangeNumber = 0x663E,

        /// <summary>
        /// Contains the names of the mail user's hobbies.
        /// </summary>
        PidTagHobbies = 0x3A43,

        /// <summary>
        /// Contains a secondary telephone number at the mail user's home.
        /// </summary>
        PidTagHome2TelephoneNumber = 0x3A2F,

        /// <summary>
        /// Contains secondary telephone numbers at the mail user's home.
        /// </summary>
        PidTagHome2TelephoneNumbers = 0x3A2F,

        /// <summary>
        /// Contains the name of the mail user's home locality, such as the town or city.
        /// </summary>
        PidTagHomeAddressCity = 0x3A59,

        /// <summary>
        /// Contains the name of the mail user's home country/region.
        /// </summary>
        PidTagHomeAddressCountry = 0x3A5A,

        /// <summary>
        /// Contains the postal code for the mail user's home postal address.
        /// </summary>
        PidTagHomeAddressPostalCode = 0x3A5B,

        /// <summary>
        /// Contains the number or identifier of the mail user's home post office box.
        /// </summary>
        PidTagHomeAddressPostOfficeBox = 0x3A5E,

        /// <summary>
        /// Contains the name of the mail user's home state or province.
        /// </summary>
        PidTagHomeAddressStateOrProvince = 0x3A5C,

        /// <summary>
        /// Contains the mail user's home street address.
        /// </summary>
        PidTagHomeAddressStreet = 0x3A5D,

        /// <summary>
        /// Contains the telephone number of the mail user's home fax machine.
        /// </summary>
        PidTagHomeFaxNumber = 0x3A25,

        /// <summary>
        /// Contains the primary telephone number of the mail user's home.
        /// </summary>
        PidTagHomeTelephoneNumber = 0x3A09,

        /// <summary>
        /// Contains message body text in HTML format.
        /// </summary>
        PidTagHtml = 0x1013,

        /// <summary>
        /// Contains the date and time, in UTC, when an appointment or meeting ends.
        /// </summary>
        PidTagICalendarEndTime = 0x10C4,

        /// <summary>
        /// Contains the date and time, in UTC, for the activation of the next reminder.
        /// </summary>
        PidTagICalendarReminderNextTime = 0x10CA,

        /// <summary>
        /// Contains the date and time, in UTC, when the appointment or meeting starts.
        /// </summary>
        PidTagICalendarStartTime = 0x10C3,

        /// <summary>
        /// Specifies which icon is to be used by a user interface when displaying a group of Message objects.
        /// </summary>
        PidTagIconIndex = 0x1080,

        /// <summary>
        /// Indicates the level of importance assigned by the end user to the Message object.
        /// </summary>
        PidTagImportance = 0x0017,

        /// <summary>
        /// Specifies whether the attachment represents an alternate replica.
        /// </summary>
        PidTagInConflict = 0x666C,

        /// <summary>
        /// Indicates which page of a display template to display first.
        /// </summary>
        PidTagInitialDetailsPane = 0x3F08,

        /// <summary>
        /// Contains the initials for parts of the full name of the mail user.
        /// </summary>
        PidTagInitials = 0x3A0A,

        /// <summary>
        /// Contains the value of the original message's PidTagInternetMessageId property (section 2.739) value.
        /// </summary>
        PidTagInReplyToId = 0x1042,

        /// <summary>
        /// Contains an object on an NSPI server.
        /// </summary>
        PidTagInstanceKey = 0x0FF6,

        /// <summary>
        /// Contains an identifier for a single instance of a row in the table.
        /// </summary>
        PidTagInstanceNum = 0x674E,

        /// <summary>
        /// Contains an identifier for all instances of a row in the table.
        /// </summary>
        PidTagInstID = 0x674D,

        /// <summary>
        /// Indicates the code page used for the PidTagBody property (section 2.609) or the PidTagBodyHtml property (section 2.612).
        /// </summary>
        PidTagInternetCodepage = 0x3FDE,

        /// <summary>
        /// Indicates the encoding method and HTML inclusion for attachments.
        /// </summary>
        PidTagInternetMailOverrideFormat = 0x5902,

        /// <summary>
        /// Corresponds to the message-id field.
        /// </summary>
        PidTagInternetMessageId = 0x1035,

        /// <summary>
        /// Contains a list of message IDs that specify the messages to which this reply is related.
        /// </summary>
        PidTagInternetReferences = 0x1039,

        /// <summary>
        /// Contains the EntryID of the Calendar folder.
        /// </summary>
        PidTagIpmAppointmentEntryId = 0x36D0,

        /// <summary>
        /// Contains the EntryID of the Contacts folder.
        /// </summary>
        PidTagIpmContactEntryId = 0x36D1,

        /// <summary>
        /// Contains the EntryID of the Drafts folder.
        /// </summary>
        PidTagIpmDraftsEntryId = 0x36D7,

        /// <summary>
        /// Contains the EntryID of the Journal folder.
        /// </summary>
        PidTagIpmJournalEntryId = 0x36D2,

        /// <summary>
        /// Contains the EntryID of the Notes folder.
        /// </summary>
        PidTagIpmNoteEntryId = 0x36D3,

        /// <summary>
        /// Contains the EntryID of the Tasks folder.
        /// </summary>
        PidTagIpmTaskEntryId = 0x36D4,

        /// <summary>
        /// Contains the Integrated Services Digital Network (ISDN) telephone number of the mail user.
        /// </summary>
        PidTagIsdnNumber = 0x3A2D,

        /// <summary>
        /// Indicates whether email recipients are to be added to the safe senders list.
        /// </summary>
        PidTagJunkAddRecipientsToSafeSendersList = 0x6103,

        /// <summary>
        /// Indicates whether email addresses of the contacts in the Contacts folder are treated in a special way with respect to the spam filter.
        /// </summary>
        PidTagJunkIncludeContacts = 0x6100,

        /// <summary>
        /// Indicates whether messages identified as spam can be permanently deleted.
        /// </summary>
        PidTagJunkPermanentlyDelete = 0x6102,

        /// <summary>
        /// Indicated whether the phishing stamp on a message is to be ignored.
        /// </summary>
        PidTagJunkPhishingEnableLinks = 0x6107,

        /// <summary>
        /// Indicates how aggressively incoming email is to be sent to the Junk Email folder.
        /// </summary>
        PidTagJunkThreshold = 0x6101,

        /// <summary>
        /// Contains a keyword that identifies the mail user to the mail user's system administrator.
        /// </summary>
        PidTagKeyword = 0x3A0B,

        /// <summary>
        /// Contains a value that indicates the language in which the messaging user is writing messages.
        /// </summary>
        PidTagLanguage = 0x3A0C,

        /// <summary>
        /// Contains the time, in UTC, of the last modification to the object.
        /// </summary>
        PidTagLastModificationTime = 0x3008,

        /// <summary>
        /// Specifies the Address Book EntryID of the last user to modify the contents of the message.
        /// </summary>
        PidTagLastModifierEntryId = 0x3FFB,

        /// <summary>
        /// Contains the name of the last mail user to change the Message object.
        /// </summary>
        PidTagLastModifierName = 0x3FFA,

        /// <summary>
        /// Specifies the last verb executed for the message item to which it is related.
        /// </summary>
        PidTagLastVerbExecuted = 0x1081,

        /// <summary>
        /// Contains the date and time, in UTC, during which the operation represented in the PidTagLastVerbExecuted property (section 2.758) took place.
        /// </summary>
        PidTagLastVerbExecutionTime = 0x1082,

        /// <summary>
        /// Contains a URI that provides detailed help information for the mailing list from which an email message was sent.
        /// </summary>
        PidTagListHelp = 0x1043,

        /// <summary>
        /// Contains the URI that subscribes a recipient to a  message’s associated mailing list.
        /// </summary>
        PidTagListSubscribe = 0x1044,

        /// <summary>
        /// Contains the URI that unsubscribes a recipient from a message’s associated mailing list.
        /// </summary>
        PidTagListUnsubscribe = 0x1045,

        /// <summary>
        /// Specifies the time, in UTC, that a Message object or Folder object was last changed.
        /// </summary>
        PidTagLocalCommitTime = 0x6709,

        /// <summary>
        /// Contains the time of the most recent message change within the folder container, excluding messages changed within subfolders.
        /// </summary>
        PidTagLocalCommitTimeMax = 0x670A,

        /// <summary>
        /// Contains the Logon object LocaleID.
        /// </summary>
        PidTagLocaleId = 0x66A1,

        /// <summary>
        /// Contains the name of the mail user's locality, such as the town or city.
        /// </summary>
        PidTagLocality = 0x3A27,

        /// <summary>
        /// Contains the location of the mail user.
        /// </summary>
        PidTagLocation = 0x3A0D,

        /// <summary>
        /// Contains the EntryID in the Global Address List (GAL) of the owner of the mailbox.
        /// </summary>
        PidTagMailboxOwnerEntryId = 0x661B,

        /// <summary>
        /// Contains the display name of the owner of the mailbox.
        /// </summary>
        PidTagMailboxOwnerName = 0x661C,

        /// <summary>
        /// Contains the name of the mail user's manager.
        /// </summary>
        PidTagManagerName = 0x3A4E,

        /// <summary>
        /// A 16-byte constant that is present on all Address Book objects, but is not present on objects in an offline address book.
        /// </summary>
        PidTagMappingSignature = 0x0FF8,

        /// <summary>
        /// Maximum size, in kilobytes, of a message that a user is allowed to submit for transmission to another user.
        /// </summary>
        PidTagMaximumSubmitMessageSize = 0x666D,

        /// <summary>
        /// Contains a unique identifier that the messaging server generates for each user.
        /// </summary>
        PidTagMemberId = 0x6671,

        /// <summary>
        /// Contains the user-readable name of the user.
        /// </summary>
        PidTagMemberName = 0x6672,

        /// <summary>
        /// Contains the permissions for the specified user.
        /// </summary>
        PidTagMemberRights = 0x6673,

        /// <summary>
        /// Identifies all attachments to the current message.
        /// </summary>
        PidTagMessageAttachments = 0x0E13,

        /// <summary>
        /// Indicates that the receiving mailbox owner is a carbon copy (Cc) recipient of this email message.
        /// </summary>
        PidTagMessageCcMe = 0x0058,

        /// <summary>
        /// Denotes the specific type of the Message object.
        /// </summary>
        PidTagMessageClass = 0x001A,

        /// <summary>
        /// Specifies the code page used to encode the non-Unicode string properties on this Message object.
        /// </summary>
        PidTagMessageCodepage = 0x3FFD,

        /// <summary>
        /// Specifies the time (in UTC) when the server received the message.
        /// </summary>
        PidTagMessageDeliveryTime = 0x0E06,

        /// <summary>
        /// Specifies the format that an email editor can use for editing the message body.
        /// </summary>
        PidTagMessageEditorFormat = 0x5909,

        /// <summary>
        /// Specifies the status of the Message object.
        /// </summary>
        PidTagMessageFlags = 0x0E07,

        /// <summary>
        /// Contains the common name of a messaging user for use in a message header.
        /// </summary>
        PidTagMessageHandlingSystemCommonName = 0x3A0F,

        /// <summary>
        /// Contains the Windows Locale ID of the end-user who created this message.
        /// </summary>
        PidTagMessageLocaleId = 0x3FF1,

        /// <summary>
        /// Indicates that the receiving mailbox owner is a primary or a carbon copy (Cc) recipient of this email message.
        /// </summary>
        PidTagMessageRecipientMe = 0x0059,

        /// <summary>
        /// Identifies all of the recipients of the current message.
        /// </summary>
        PidTagMessageRecipients = 0x0E12,

        /// <summary>
        /// Contains the size, in bytes, consumed by the Message object on the server.
        /// </summary>
        PidTagMessageSize = 0x0E08,

        /// <summary>
        /// Specifies the 64-bit version of the PidTagMessageSize property (section 2.787).
        /// </summary>
        PidTagMessageSizeExtended = 0x0E08,

        /// <summary>
        /// Specifies the status of a message in a contents table.
        /// </summary>
        PidTagMessageStatus = 0x0E17,

        /// <summary>
        /// Contains a message identifier assigned by a message transfer agent.
        /// </summary>
        PidTagMessageSubmissionId = 0x0047,

        /// <summary>
        /// Indicates that the receiving mailbox owner is one of the primary recipients of this email message.
        /// </summary>
        PidTagMessageToMe = 0x0057,

        /// <summary>
        /// Contains a value that contains the MID of the message currently being synchronized.
        /// </summary>
        PidTagMid = 0x674A,

        /// <summary>
        /// Specifies the middle name(s) of the contact.
        /// </summary>
        PidTagMiddleName = 0x3A44,

        /// <summary>
        /// Contains the top-level MIME message headers, all MIME message body part headers, and body part content that is not already converted to Message object properties, including attachments.
        /// </summary>
        PidTagMimeSkeleton = 0x64F0,

        /// <summary>
        /// Contains the mail user's cellular telephone number.
        /// </summary>
        PidTagMobileTelephoneNumber = 0x3A1C,

        /// <summary>
        /// Indicates the best available format for storing the message body.
        /// </summary>
        PidTagNativeBody = 0x1016,

        /// <summary>
        /// Specifies the server that a client is currently attempting to use to send email.
        /// </summary>
        PidTagNextSendAcct = 0x0E29,

        /// <summary>
        /// Contains the mail user's nickname.
        /// </summary>
        PidTagNickname = 0x3A4F,

        /// <summary>
        /// Contains the diagnostic code for a delivery status notification, as specified in [RFC3464].
        /// </summary>
        PidTagNonDeliveryReportDiagCode = 0x0C05,

        /// <summary>
        /// Contains an integer value that indicates a reason for delivery failure.
        /// </summary>
        PidTagNonDeliveryReportReasonCode = 0x0C04,

        /// <summary>
        /// Contains the value of the Status field for a delivery status notification, as specified in [RFC3464].
        /// </summary>
        PidTagNonDeliveryReportStatusCode = 0x0C20,

        /// <summary>
        /// Specifies whether the client sends a non-read receipt.
        /// </summary>
        PidTagNonReceiptNotificationRequested = 0x0C06,

        /// <summary>
        /// Contains the normalized subject of the message.
        /// </summary>
        PidTagNormalizedSubject = 0x0E1D,

        /// <summary>
        /// Indicates the type of Server object.
        /// </summary>
        PidTagObjectType = 0x0FFE,

        /// <summary>
        /// Contains the mail user's office location.
        /// </summary>
        PidTagOfficeLocation = 0x3A19,

        /// <summary>
        /// A string-formatted GUID that represents the address list container object.
        /// </summary>
        PidTagOfflineAddressBookContainerGuid = 0x6802,

        /// <summary>
        /// Contains the DN of the address list that is contained in the OAB message.
        /// </summary>
        PidTagOfflineAddressBookDistinguishedName = 0x6804,

        /// <summary>
        /// Contains the message class for full OAB messages.
        /// </summary>
        PidTagOfflineAddressBookMessageClass = 0x6803,

        /// <summary>
        /// Contains the display name of the address list.
        /// </summary>
        PidTagOfflineAddressBookName = 0x6800,

        /// <summary>
        /// Contains the sequence number of the OAB.
        /// </summary>
        PidTagOfflineAddressBookSequence = 0x6801,

        /// <summary>
        /// Contains a list of the property tags that have been truncated or limited by the server.
        /// </summary>
        PidTagOfflineAddressBookTruncatedProperties = 0x6805,

        /// <summary>
        /// Contains a positive number whose negative is less than or equal to the value of the PidLidTaskOrdinal property (section 2.327) of all of the Task objects in the folder.
        /// </summary>
        PidTagOrdinalMost = 0x36E2,

        /// <summary>
        /// Contains an identifier for the mail user used within the mail user's organization.
        /// </summary>
        PidTagOrganizationalIdNumber = 0x3A10,

        /// <summary>
        /// Contains an address book EntryID structure ([MS-OXCDATA] section 2.2.5.2) and is defined in report messages to identify the user who sent the original message.
        /// </summary>
        PidTagOriginalAuthorEntryId = 0x004C,

        /// <summary>
        /// Contains the display name of the sender of the original message referenced by a report message.
        /// </summary>
        PidTagOriginalAuthorName = 0x004D,

        /// <summary>
        /// Contains the delivery time, in UTC, from the original message.
        /// </summary>
        PidTagOriginalDeliveryTime = 0x0055,

        /// <summary>
        /// Contains the value of the PidTagDisplayBcc property (section 2.665) from the original message.
        /// </summary>
        PidTagOriginalDisplayBcc = 0x0072,

        /// <summary>
        /// Contains the value of the PidTagDisplayCc property(section 2.666) from the original message.
        /// </summary>
        PidTagOriginalDisplayCc = 0x0073,

        /// <summary>
        /// Contains the value of the PidTagDisplayTo property (section 2.669) from the original message.
        /// </summary>
        PidTagOriginalDisplayTo = 0x0074,

        /// <summary>
        /// Contains the original EntryID of an object.
        /// </summary>
        PidTagOriginalEntryId = 0x3A12,

        /// <summary>
        /// Designates the PidTagMessageClass property ([MS-OXCMSG] section 2.2.1.3) from the original message.
        /// </summary>
        PidTagOriginalMessageClass = 0x004B,

        /// <summary>
        /// Contains the message ID of the original message included in replies or resent messages.
        /// </summary>
        PidTagOriginalMessageId = 0x1046,

        /// <summary>
        /// Contains the value of the original message sender's PidTagSenderAddressType property (section 2.991).
        /// </summary>
        PidTagOriginalSenderAddressType = 0x0066,

        /// <summary>
        /// Contains the value of the original message sender's PidTagSenderEmailAddress property (section 2.992).
        /// </summary>
        PidTagOriginalSenderEmailAddress = 0x0067,

        /// <summary>
        /// Contains an address book EntryID that is set on delivery report messages.
        /// </summary>
        PidTagOriginalSenderEntryId = 0x005B,

        /// <summary>
        /// Contains the value of the original message sender's PidTagSenderName property (section 2.995), and is set on delivery report messages.
        /// </summary>
        PidTagOriginalSenderName = 0x005A,

        /// <summary>
        /// Contains an address book search key set on the original email message.
        /// </summary>
        PidTagOriginalSenderSearchKey = 0x005C,

        /// <summary>
        /// Contains the sensitivity value of the original email message.
        /// </summary>
        PidTagOriginalSensitivity = 0x002E,

        /// <summary>
        /// Contains the address type of the end user who is represented by the original email message sender.
        /// </summary>
        PidTagOriginalSentRepresentingAddressType = 0x0068,

        /// <summary>
        /// Contains the email address of the end user who is represented by the original email message sender.
        /// </summary>
        PidTagOriginalSentRepresentingEmailAddress = 0x0069,

        /// <summary>
        /// Identifies an address book EntryID that contains the entry identifier of the end user who is represented by the original message sender.
        /// </summary>
        PidTagOriginalSentRepresentingEntryId = 0x005E,

        /// <summary>
        /// Contains the display name of the end user who is represented by the original email message sender.
        /// </summary>
        PidTagOriginalSentRepresentingName = 0x005D,

        /// <summary>
        /// Identifies an address book search key that contains the SearchKey of the end user who is represented by the original message sender.
        /// </summary>
        PidTagOriginalSentRepresentingSearchKey = 0x005F,

        /// <summary>
        /// Specifies the subject of the original message.
        /// </summary>
        PidTagOriginalSubject = 0x0049,

        /// <summary>
        /// Specifies the original email message's submission date and time, in UTC.
        /// </summary>
        PidTagOriginalSubmitTime = 0x004E,

        /// <summary>
        /// Indicates whether an email sender requests an email delivery receipt from the messaging system.
        /// </summary>
        PidTagOriginatorDeliveryReportRequested = 0x0023,

        /// <summary>
        /// Specifies whether an email sender requests suppression of nondelivery receipts.
        /// </summary>
        PidTagOriginatorNonDeliveryReportRequested = 0x0C08,

        /// <summary>
        /// Specifies whether contact synchronization with an external source is handled by the server.
        /// </summary>
        PidTagOscSyncEnabled = 0x7C24,

        /// <summary>
        /// Contains the name of the mail user's other locality, such as the town or city.
        /// </summary>
        PidTagOtherAddressCity = 0x3A5F,

        /// <summary>
        /// Contains the name of the mail user's other country/region.
        /// </summary>
        PidTagOtherAddressCountry = 0x3A60,

        /// <summary>
        /// Contains the postal code for the mail user's other postal address.
        /// </summary>
        PidTagOtherAddressPostalCode = 0x3A61,

        /// <summary>
        /// Contains the number or identifier of the mail user's other post office box.
        /// </summary>
        PidTagOtherAddressPostOfficeBox = 0x3A64,

        /// <summary>
        /// Contains the name of the mail user's other state or province.
        /// </summary>
        PidTagOtherAddressStateOrProvince = 0x3A62,

        /// <summary>
        /// Contains the mail user's other street address.
        /// </summary>
        PidTagOtherAddressStreet = 0x3A63,

        /// <summary>
        /// Contains an alternate telephone number for the mail user.
        /// </summary>
        PidTagOtherTelephoneNumber = 0x3A1F,

        /// <summary>
        /// Indicates whether the user is OOF.
        /// </summary>
        PidTagOutOfOfficeState = 0x661D,

        /// <summary>
        /// Specifies a quasi-unique value among all of the Calendar objects in a user's mailbox.
        /// </summary>
        PidTagOwnerAppointmentId = 0x0062,

        /// <summary>
        /// Contains the mail user's pager telephone number.
        /// </summary>
        PidTagPagerTelephoneNumber = 0x3A21,

        /// <summary>
        /// Contains the EntryID of the folder where messages or subfolders reside.
        /// </summary>
        PidTagParentEntryId = 0x0E09,

        /// <summary>
        /// Contains a value that contains the Folder ID (FID), as specified in [MS-OXCDATA] section 2.2.1.1, that identifies the parent folder of the messaging object being synchronized.
        /// </summary>
        PidTagParentFolderId = 0x6749,

        /// <summary>
        /// Contains the search key that is used to correlate the original message and the reports about the original message.
        /// </summary>
        PidTagParentKey = 0x0025,

        /// <summary>
        /// Contains a value on a folder that contains the PidTagSourceKey property (section 2.1012) of the parent folder.
        /// </summary>
        PidTagParentSourceKey = 0x65E1,

        /// <summary>
        /// Contains the URL of the mail user's personal home page.
        /// </summary>
        PidTagPersonalHomePage = 0x3A50,

        /// <summary>
        /// Specifies the GUID of a retention tag.
        /// </summary>
        PidTagPolicyTag = 0x3019,

        /// <summary>
        /// Contains the mail user's postal address.
        /// </summary>
        PidTagPostalAddress = 0x3A15,

        /// <summary>
        /// Contains the postal code for the mail user's postal address.
        /// </summary>
        PidTagPostalCode = 0x3A2A,

        /// <summary>
        /// Contains the number or identifier of the mail user's post office box.
        /// </summary>
        PidTagPostOfficeBox = 0x3A2B,

        /// <summary>
        /// Contains a value that contains a serialized representation of a PredecessorChangeList structure.
        /// </summary>
        PidTagPredecessorChangeList = 0x65E3,

        /// <summary>
        /// Contains the telephone number of the mail user's primary fax machine.
        /// </summary>
        PidTagPrimaryFaxNumber = 0x3A23,

        /// <summary>
        /// Specifies the first server that a client is to use to send the email with.
        /// </summary>
        PidTagPrimarySendAccount = 0x0E28,

        /// <summary>
        /// Contains the mail user's primary telephone number.
        /// </summary>
        PidTagPrimaryTelephoneNumber = 0x3A1A,

        /// <summary>
        /// Indicates the client's request for the priority with which the message is to be sent by the messaging system.
        /// </summary>
        PidTagPriority = 0x0026,

        /// <summary>
        /// Indicates whether a client has already processed a received task communication.
        /// </summary>
        PidTagProcessed = 0x7D01,

        /// <summary>
        /// Contains the name of the mail user's line of business.
        /// </summary>
        PidTagProfession = 0x3A46,

        /// <summary>
        /// Maximum size, in kilobytes, that a user is allowed to accumulate in their mailbox before no further email will be delivered to their mailbox.
        /// </summary>
        PidTagProhibitReceiveQuota = 0x666A,

        /// <summary>
        /// Maximum size, in kilobytes, that a user is allowed to accumulate in their mailbox before the user can no longer send any more email.
        /// </summary>
        PidTagProhibitSendQuota = 0x666E,

        /// <summary>
        /// Contains the domain responsible for transmitting the current message.
        /// </summary>
        PidTagPurportedSenderDomain = 0x4083,

        /// <summary>
        /// Contains the mail user's radio telephone number.
        /// </summary>
        PidTagRadioTelephoneNumber = 0x3A1D,

        /// <summary>
        /// Indicates whether a message has been read.
        /// </summary>
        PidTagRead = 0x0E69,

        /// <summary>
        /// Contains the address type of the end user to whom a read receipt is directed.
        /// </summary>
        PidTagReadReceiptAddressType = 0x4029,

        /// <summary>
        /// Contains the email address of the end user to whom a read receipt is directed.
        /// </summary>
        PidTagReadReceiptEmailAddress = 0x402A,

        /// <summary>
        /// Contains an address book EntryID.
        /// </summary>
        PidTagReadReceiptEntryId = 0x0046,

        /// <summary>
        /// Contains the display name for the end user to whom a read receipt is directed.
        /// </summary>
        PidTagReadReceiptName = 0x402B,

        /// <summary>
        /// Specifies whether the email sender requests a read receipt from all recipients when this email message is read or opened.
        /// </summary>
        PidTagReadReceiptRequested = 0x0029,

        /// <summary>
        /// Contains an address book search key.
        /// </summary>
        PidTagReadReceiptSearchKey = 0x0053,

        /// <summary>
        /// Contains the SMTP email address of the user to whom a read receipt is directed.
        /// </summary>
        PidTagReadReceiptSmtpAddress = 0x5D05,

        /// <summary>
        /// Contains the sent time for a message disposition notification, as specified in [RFC3798].
        /// </summary>
        PidTagReceiptTime = 0x002A,

        /// <summary>
        /// Contains the email message receiver's email address type.
        /// </summary>
        PidTagReceivedByAddressType = 0x0075,

        /// <summary>
        /// Contains the email message receiver's email address.
        /// </summary>
        PidTagReceivedByEmailAddress = 0x0076,

        /// <summary>
        /// Contains the address book EntryID of the mailbox receiving the Email object.
        /// </summary>
        PidTagReceivedByEntryId = 0x003F,

        /// <summary>
        /// Contains the email message receiver's display name.
        /// </summary>
        PidTagReceivedByName = 0x0040,

        /// <summary>
        /// Identifies an address book search key that contains a binary-comparable key that is used to identify correlated objects for a search.
        /// </summary>
        PidTagReceivedBySearchKey = 0x0051,

        /// <summary>
        /// Contains the email message receiver's SMTP email address.
        /// </summary>
        PidTagReceivedBySmtpAddress = 0x5D07,

        /// <summary>
        /// Contains the email address type for the end user represented by the receiving mailbox owner.
        /// </summary>
        PidTagReceivedRepresentingAddressType = 0x0077,

        /// <summary>
        /// Contains the email address for the end user represented by the receiving mailbox owner.
        /// </summary>
        PidTagReceivedRepresentingEmailAddress = 0x0078,

        /// <summary>
        /// Contains an address book EntryID that identifies the end user represented by the receiving mailbox owner.
        /// </summary>
        PidTagReceivedRepresentingEntryId = 0x0043,

        /// <summary>
        /// Contains the display name for the end user represented by the receiving mailbox owner.
        /// </summary>
        PidTagReceivedRepresentingName = 0x0044,

        /// <summary>
        /// Identifies an address book search key that contains a binary-comparable key of the end user represented by the receiving mailbox owner.
        /// </summary>
        PidTagReceivedRepresentingSearchKey = 0x0052,

        /// <summary>
        /// Contains the SMTP email address of the user represented by the receiving mailbox owner.
        /// </summary>
        PidTagReceivedRepresentingSmtpAddress = 0x5D08,

        /// <summary>
        /// Specifies the display name of the recipient.
        /// </summary>
        PidTagRecipientDisplayName = 0x5FF6,

        /// <summary>
        /// Identifies an Address Book object that specifies the recipient.
        /// </summary>
        PidTagRecipientEntryId = 0x5FF7,

        /// <summary>
        /// Specifies a bit field that describes the recipient status.
        /// </summary>
        PidTagRecipientFlags = 0x5FFD,

        /// <summary>
        /// Specifies the location of the current recipient in the recipient table.
        /// </summary>
        PidTagRecipientOrder = 0x5FDF,

        /// <summary>
        /// Indicates that the attendee proposed a new date and/or time.
        /// </summary>
        PidTagRecipientProposed = 0x5FE1,

        /// <summary>
        /// Indicates the meeting end time requested by the attendee in a counter proposal.
        /// </summary>
        PidTagRecipientProposedEndTime = 0x5FE4,

        /// <summary>
        /// Indicates the meeting start time requested by the attendee in a counter proposal.
        /// </summary>
        PidTagRecipientProposedStartTime = 0x5FE3,

        /// <summary>
        /// Specifies whether adding additional or different recipients is prohibited for the email message when forwarding the email message.
        /// </summary>
        PidTagRecipientReassignmentProhibited = 0x002B,

        /// <summary>
        /// Indicates the response status that is returned by the attendee.
        /// </summary>
        PidTagRecipientTrackStatus = 0x5FFF,

        /// <summary>
        /// Indicates the date and time at which the attendee responded.
        /// </summary>
        PidTagRecipientTrackStatusTime = 0x5FFB,

        /// <summary>
        /// Represents the recipient type of a recipient on the message.
        /// </summary>
        PidTagRecipientType = 0x0C15,

        /// <summary>
        /// Contains a unique binary-comparable identifier for a specific object.
        /// </summary>
        PidTagRecordKey = 0x0FF9,

        /// <summary>
        /// Contains the name of the mail user's referral.
        /// </summary>
        PidTagReferredByName = 0x3A47,

        /// <summary>
        /// Contains an EntryID for the Reminders folder.
        /// </summary>
        PidTagRemindersOnlineEntryId = 0x36D5,

        /// <summary>
        /// Contains the value of the Remote-MTA field for a delivery status notification, as specified in [RFC3464].
        /// </summary>
        PidTagRemoteMessageTransferAgent = 0x0C21,

        /// <summary>
        /// Represents an offset, in rendered characters, to use when rendering an attachment  within the main message text.
        /// </summary>
        PidTagRenderingPosition = 0x370B,

        /// <summary>
        /// Identifies a FlatEntryList structure ([MS-OXCDATA] section 2.3.3) of address book EntryIDs for recipients that are to receive a reply.
        /// </summary>
        PidTagReplyRecipientEntries = 0x004F,

        /// <summary>
        /// Contains a list of display names for recipients that are to receive a reply.
        /// </summary>
        PidTagReplyRecipientNames = 0x0050,

        /// <summary>
        /// Indicates whether a reply is requested to a Message object.
        /// </summary>
        PidTagReplyRequested = 0x0C17,

        /// <summary>
        /// Contains the value of the GUID that points to a Reply template.
        /// </summary>
        PidTagReplyTemplateId = 0x65C2,

        /// <summary>
        /// Specifies the time, in UTC, that the sender has designated for an associated work item to be due.
        /// </summary>
        PidTagReplyTime = 0x0030,

        /// <summary>
        /// Contains a string indicating whether the original message was displayed to the user or deleted (report messages only).
        /// </summary>
        PidTagReportDisposition = 0x0080,

        /// <summary>
        /// Contains a description of the action that a client has performed on behalf of a user (report messages only).
        /// </summary>
        PidTagReportDispositionMode = 0x0081,

        /// <summary>
        /// Specifies an entry ID that identifies the application that generated a report message.
        /// </summary>
        PidTagReportEntryId = 0x0045,

        /// <summary>
        /// Contains the value of the Reporting-MTA field for a delivery status notification, as specified in [RFC3464].
        /// </summary>
        PidTagReportingMessageTransferAgent = 0x6820,

        /// <summary>
        /// Contains the display name for the entity (usually a server agent) that generated the report message.
        /// </summary>
        PidTagReportName = 0x003A,

        /// <summary>
        /// Contains an address book search key representing the entity (usually a server agent) that generated the report message.
        /// </summary>
        PidTagReportSearchKey = 0x0054,

        /// <summary>
        /// Contains the data that is used to correlate the report and the original message.
        /// </summary>
        PidTagReportTag = 0x0031,

        /// <summary>
        /// Contains the optional text for a report message.
        /// </summary>
        PidTagReportText = 0x1001,

        /// <summary>
        /// Indicates the last time that the contact list that is controlled by the PidTagJunkIncludeContacts property (section 2.749) was updated.
        /// </summary>
        PidTagReportTime = 0x0032,

        /// <summary>
        /// Specifies how to resolve any conflicts with the message.
        /// </summary>
        PidTagResolveMethod = 0x3FE7,

        /// <summary>
        /// Indicates whether a response is requested to a Message object.
        /// </summary>
        PidTagResponseRequested = 0x0063,

        /// <summary>
        /// Specifies whether another mail agent has ensured that the message will be delivered.
        /// </summary>
        PidTagResponsibility = 0x0E0F,

        /// <summary>
        /// Specifies the date, in UTC, after which a Message object is expired by the server.
        /// </summary>
        PidTagRetentionDate = 0x301C,

        /// <summary>
        /// Contains flags that specify the status or nature of an item's retention tag or archive tag.
        /// </summary>
        PidTagRetentionFlags = 0x301D,

        /// <summary>
        /// Specifies the number of days that a Message object can remain unarchived.
        /// </summary>
        PidTagRetentionPeriod = 0x301A,

        /// <summary>
        /// Specifies a user's folder permissions.
        /// </summary>
        PidTagRights = 0x6639,

        /// <summary>
        /// Contains a bitmask that indicates which stream properties exist on the message.
        /// </summary>
        PidTagRoamingDatatypes = 0x7C06,

        /// <summary>
        /// Contains a dictionary stream, as specified in [MS-OXOCFG] section 2.2.5.1.
        /// </summary>
        PidTagRoamingDictionary = 0x7C07,

        /// <summary>
        /// Contains an XML stream, as specified in [MS-OXOCFG] section 2.2.5.2.
        /// </summary>
        PidTagRoamingXmlStream = 0x7C08,

        /// <summary>
        /// Contains a unique identifier for a recipient in a message's recipient table.
        /// </summary>
        PidTagRowid = 0x3000,

        /// <summary>
        /// Identifies the type of the row.
        /// </summary>
        PidTagRowType = 0x0FF5,

        /// <summary>
        /// Contains message body text in compressed RTF format.
        /// </summary>
        PidTagRtfCompressed = 0x1009,

        /// <summary>
        /// Indicates whether the PidTagBody property (section 2.609) and the PidTagRtfCompressed property (section 2.932) contain the same text (ignoring formatting).
        /// </summary>
        PidTagRtfInSync = 0x0E1F,

        /// <summary>
        /// Contains the index of a rule action that failed.
        /// </summary>
        PidTagRuleActionNumber = 0x6650,

        /// <summary>
        /// Contains the set of actions associated with the rule.
        /// </summary>
        PidTagRuleActions = 0x6680,

        /// <summary>
        /// Contains the ActionType field ([MS-OXORULE] section 2.2.5.1) of a rule that failed.
        /// </summary>
        PidTagRuleActionType = 0x6649,

        /// <summary>
        /// Defines the conditions under which a rule action is to be executed.
        /// </summary>
        PidTagRuleCondition = 0x6679,

        /// <summary>
        /// Contains the error code that indicates the cause of an error encountered during the execution of the rule.
        /// </summary>
        PidTagRuleError = 0x6648,

        /// <summary>
        /// Contains the EntryID of the folder where the rule that triggered the generation of a DAM is stored.
        /// </summary>
        PidTagRuleFolderEntryId = 0x6651,

        /// <summary>
        /// Specifies a unique identifier that is generated by the messaging server for each rule when the rule is first created.
        /// </summary>
        PidTagRuleId = 0x6674,

        /// <summary>
        /// Contains a buffer that is obtained by concatenating the PidTagRuleId property (section 2.940) values from all of the rules contributing actions that are contained in the PidTagClientActions property (section 2.625).
        /// </summary>
        PidTagRuleIds = 0x6675,

        /// <summary>
        /// Contains 0x00000000. This property is not used.
        /// </summary>
        PidTagRuleLevel = 0x6683,

        /// <summary>
        /// Contains 0x00000000. Set on the FAI message.
        /// </summary>
        PidTagRuleMessageLevel = 0x65ED,

        /// <summary>
        /// Specifies the name of the rule. Set on the FAI message.
        /// </summary>
        PidTagRuleMessageName = 0x65EC,

        /// <summary>
        /// Identifies the client application that owns the rule. Set on the FAI message.
        /// </summary>
        PidTagRuleMessageProvider = 0x65EB,

        /// <summary>
        /// Contains opaque data set by the client for the exclusive use of the client. Set on the FAI message.
        /// </summary>
        PidTagRuleMessageProviderData = 0x65EE,

        /// <summary>
        /// Contains a value used to determine the order in which rules are evaluated and executed. Set on the FAI message.
        /// </summary>
        PidTagRuleMessageSequence = 0x65F3,

        /// <summary>
        /// Contains flags that specify the state of the rule. Set on the FAI message.
        /// </summary>
        PidTagRuleMessageState = 0x65E9,

        /// <summary>
        /// Contains an opaque property that the client sets for the exclusive use of the client. Set on the FAI message.
        /// </summary>
        PidTagRuleMessageUserFlags = 0x65EA,

        /// <summary>
        /// Specifies the name of the rule.
        /// </summary>
        PidTagRuleName = 0x6682,

        /// <summary>
        /// A string identifying the client application that owns a rule.
        /// </summary>
        PidTagRuleProvider = 0x6681,

        /// <summary>
        /// Contains opaque data set by the client for the exclusive use of the client.
        /// </summary>
        PidTagRuleProviderData = 0x6684,

        /// <summary>
        /// Contains a value used to determine the order in which rules are evaluated and executed.
        /// </summary>
        PidTagRuleSequence = 0x6676,

        /// <summary>
        /// Contains flags that specify the state of the rule.
        /// </summary>
        PidTagRuleState = 0x6677,

        /// <summary>
        /// Contains an opaque property that the client sets for the exclusive use of the client.
        /// </summary>
        PidTagRuleUserFlags = 0x6678,

        /// <summary>
        /// Contains additional rule data about the Rule FAI message.
        /// </summary>
        PidTagRwRulesStream = 0x6802,

        /// <summary>
        /// Contains a list of tombstones, where each tombstone represents a Meeting object that has been declined.
        /// </summary>
        PidTagScheduleInfoAppointmentTombstone = 0x686A,

        /// <summary>
        /// Indicates whether a client or server is to automatically respond to all meeting requests for the attendee or resource.
        /// </summary>
        PidTagScheduleInfoAutoAcceptAppointments = 0x686D,

        /// <summary>
        /// Specifies the EntryIDs of the delegates.
        /// </summary>
        PidTagScheduleInfoDelegateEntryIds = 0x6845,

        /// <summary>
        /// Specifies the names of the delegates.
        /// </summary>
        PidTagScheduleInfoDelegateNames = 0x6844,

        /// <summary>
        /// Specifies the names of the delegates in Unicode.
        /// </summary>
        PidTagScheduleInfoDelegateNamesW = 0x684A,

        /// <summary>
        /// Indicates whether the delegator wants to receive copies of the meeting-related objects that are sent to the delegate.
        /// </summary>
        PidTagScheduleInfoDelegatorWantsCopy = 0x6842,

        /// <summary>
        /// Indicates whether the delegator wants to receive informational updates.
        /// </summary>
        PidTagScheduleInfoDelegatorWantsInfo = 0x684B,

        /// <summary>
        /// Indicates whether a client or server, when automatically responding to meeting requests, is to decline Meeting Request objects that overlap with previously scheduled events.
        /// </summary>
        PidTagScheduleInfoDisallowOverlappingAppts = 0x686F,

        /// <summary>
        /// Indicates whether a client or server, when automatically responding to meeting requests, is to decline Meeting Request objects that represent a recurring series.
        /// </summary>
        PidTagScheduleInfoDisallowRecurringAppts = 0x686E,

        /// <summary>
        /// Contains a value set to TRUE by the client, regardless of user input.
        /// </summary>
        PidTagScheduleInfoDontMailDelegates = 0x6843,

        /// <summary>
        /// This property is deprecated and is not to be used.
        /// </summary>
        PidTagScheduleInfoFreeBusy = 0x686C,

        /// <summary>
        /// Specifies the times for which the free/busy status is set a value of OOF.
        /// </summary>
        PidTagScheduleInfoFreeBusyAway = 0x6856,

        /// <summary>
        /// Specifies the blocks of time for which the free/busy status is set to a value of busy.
        /// </summary>
        PidTagScheduleInfoFreeBusyBusy = 0x6854,

        /// <summary>
        /// Specifies the blocks for which free/busy data of type busy or OOF is present in the free/busy message.
        /// </summary>
        PidTagScheduleInfoFreeBusyMerged = 0x6850,

        /// <summary>
        /// Specifies the blocks of times for which the free/busy status is set to a value of  tentative.
        /// </summary>
        PidTagScheduleInfoFreeBusyTentative = 0x6852,

        /// <summary>
        /// Specifies the months for which free/busy data of type OOF is present in the free/busy message.
        /// </summary>
        PidTagScheduleInfoMonthsAway = 0x6855,

        /// <summary>
        /// Specifies the months for which free/busy data of type busy is present in the free/busy message.
        /// </summary>
        PidTagScheduleInfoMonthsBusy = 0x6853,

        /// <summary>
        /// Specifies the months for which free/busy data of type busy or OOF is present in the free/busy message.
        /// </summary>
        PidTagScheduleInfoMonthsMerged = 0x684F,

        /// <summary>
        /// Specifies the months for which free/busy data of type tentative is present in the free/busy message.
        /// </summary>
        PidTagScheduleInfoMonthsTentative = 0x6851,

        /// <summary>
        /// Set to 0x00000000 when sending and is ignored on receipt.
        /// </summary>
        PidTagScheduleInfoResourceType = 0x6841,

        /// <summary>
        /// Contains the EntryID of the folder named "SCHEDULE+ FREE BUSY" under the non-IPM subtree of the public folder message store.
        /// </summary>
        PidTagSchedulePlusFreeBusyEntryId = 0x6622,

        /// <summary>
        /// Contains a series of instructions that can be executed to format an address and the data that is needed to execute those instructions.
        /// </summary>
        PidTagScriptData = 0x0004,

        /// <summary>
        /// Specifies the search criteria and search options.
        /// </summary>
        PidTagSearchFolderDefinition = 0x6845,

        /// <summary>
        /// Contains the time, in UTC, at which the search folder container will be stale and has to be updated or recreated.
        /// </summary>
        PidTagSearchFolderExpiration = 0x683A,

        /// <summary>
        /// Contains a GUID that identifies the search folder.
        /// </summary>
        PidTagSearchFolderId = 0x6842,

        /// <summary>
        /// Contains the last time, in UTC, that the folder was accessed.
        /// </summary>
        PidTagSearchFolderLastUsed = 0x6834,

        /// <summary>
        /// This property is not to be used.
        /// </summary>
        PidTagSearchFolderRecreateInfo = 0x6844,

        /// <summary>
        /// Contains flags that specify the binary large object (BLOB) data that appears in the PidTagSearchFolderDefinition (section 2.979) property.
        /// </summary>
        PidTagSearchFolderStorageType = 0x6846,

        /// <summary>
        /// Contains a unique binary-comparable key that identifies an object for a search.
        /// </summary>
        PidTagSearchKey = 0x300B,

        /// <summary>
        /// Contains security attributes in XML.
        /// </summary>
        PidTagSecurityDescriptorAsXml = 0x0E6A,

        /// <summary>
        /// This property is not set and, if set, is ignored.
        /// </summary>
        PidTagSelectable = 0x3609,

        /// <summary>
        /// Contains the email address type of the sending mailbox owner.
        /// </summary>
        PidTagSenderAddressType = 0x0C1E,

        /// <summary>
        /// Contains the email address of the sending mailbox owner.
        /// </summary>
        PidTagSenderEmailAddress = 0x0C1F,

        /// <summary>
        /// Identifies an address book EntryID that contains the address book EntryID of the sending mailbox owner.
        /// </summary>
        PidTagSenderEntryId = 0x0C19,

        /// <summary>
        /// Reports the results of a Sender-ID check.
        /// </summary>
        PidTagSenderIdStatus = 0x4079,

        /// <summary>
        /// Contains the display name of the sending mailbox owner.
        /// </summary>
        PidTagSenderName = 0x0C1A,

        /// <summary>
        /// Identifies an address book search key.
        /// </summary>
        PidTagSenderSearchKey = 0x0C1D,

        /// <summary>
        /// Contains the SMTP email address format of the e–mail address of the sending mailbox owner.
        /// </summary>
        PidTagSenderSmtpAddress = 0x5D01,

        /// <summary>
        /// Contains the telephone number of the caller associated with a voice mail message.
        /// </summary>
        PidTagSenderTelephoneNumber = 0x6802,

        /// <summary>
        /// Contains a bitmask of message encoding preferences for email sent to an email-enabled entity that is represented by this Address Book object.
        /// </summary>
        PidTagSendInternetEncoding = 0x3A71,

        /// <summary>
        /// Indicates whether the email-enabled entity represented by the Address Book object can receive all message content, including Rich Text Format (RTF) and other embedded objects.
        /// </summary>
        PidTagSendRichInfo = 0x3A40,

        /// <summary>
        /// Indicates the sender's assessment of the sensitivity of the Message object.
        /// </summary>
        PidTagSensitivity = 0x0036,

        /// <summary>
        /// Contains an EntryID that represents the Sent Items folder for the message.
        /// </summary>
        PidTagSentMailSvrEID = 0x6740,

        /// <summary>
        /// Contains an email address type.
        /// </summary>
        PidTagSentRepresentingAddressType = 0x0064,

        /// <summary>
        /// Contains an email address for the end user who is represented by the sending mailbox owner.
        /// </summary>
        PidTagSentRepresentingEmailAddress = 0x0065,

        /// <summary>
        /// Contains the identifier of the end user who is represented by the sending mailbox owner.
        /// </summary>
        PidTagSentRepresentingEntryId = 0x0041,

        /// <summary>
        /// The PidTagSentRepresentingFlags flag
        /// </summary>
        PidTagSentRepresentingFlags = 0x401A,

        /// <summary>
        /// Contains the display name for the end user who is represented by the sending mailbox owner.
        /// </summary>
        PidTagSentRepresentingName = 0x0042,

        /// <summary>
        /// Contains a binary-comparable key that represents the end user who is represented by the sending mailbox owner.
        /// </summary>
        PidTagSentRepresentingSearchKey = 0x003B,

        /// <summary>
        /// Contains the SMTP email address of the end user who is represented by the sending mailbox owner.
        /// </summary>
        PidTagSentRepresentingSmtpAddress = 0x5D02,

        /// <summary>
        /// Contains the SMTP address of the Message object.
        /// </summary>
        PidTagSmtpAddress = 0x39FE,

        /// <summary>
        /// Contains the locale identifier.
        /// </summary>
        PidTagSortLocaleId = 0x6705,

        /// <summary>
        /// Contains a value that contains an internal global identifier (GID) for this folder or message.
        /// </summary>
        PidTagSourceKey = 0x65E0,

        /// <summary>
        /// Contains a recording of the mail user's name pronunciation.
        /// </summary>
        PidTagSpokenName = 0x8CC2,

        /// <summary>
        /// Contains the name of the mail user's spouse/partner.
        /// </summary>
        PidTagSpouseName = 0x3A48,

        /// <summary>
        /// Contains the value of the PidLidAppointmentStartWhole property (section 2.29).
        /// </summary>
        PidTagStartDate = 0x0060,

        /// <summary>
        /// Contains the default retention period, and the start date from which the age of a Message object is calculated.
        /// </summary>
        PidTagStartDateEtc = 0x301B,

        /// <summary>
        /// Contains the name of the mail user's state or province.
        /// </summary>
        PidTagStateOrProvince = 0x3A28,

        /// <summary>
        /// Contains the unique EntryID of the message store where an object resides.
        /// </summary>
        PidTagStoreEntryId = 0x0FFB,

        /// <summary>
        /// Indicates whether a mailbox has any active Search folders.
        /// </summary>
        PidTagStoreState = 0x340E,

        /// <summary>
        /// Indicates whether string properties within the .msg file are Unicode-encoded.
        /// </summary>
        PidTagStoreSupportMask = 0x340D,

        /// <summary>
        /// Contains the mail user's street address.
        /// </summary>
        PidTagStreetAddress = 0x3A29,

        /// <summary>
        /// Specifies whether a folder has subfolders.
        /// </summary>
        PidTagSubfolders = 0x360A,

        /// <summary>
        /// Contains the subject of the email message.
        /// </summary>
        PidTagSubject = 0x0037,

        /// <summary>
        /// Contains the prefix for the subject of the message.
        /// </summary>
        PidTagSubjectPrefix = 0x003D,

        /// <summary>
        /// Contains supplementary information about a delivery status notification, as specified in [RFC3464].
        /// </summary>
        PidTagSupplementaryInfo = 0x0C1B,

        /// <summary>
        /// Contains the mail user's family name.
        /// </summary>
        PidTagSurname = 0x3A11,

        /// <summary>
        /// Contains a secondary storage location for flags when sender flags or sender reminders are supported.
        /// </summary>
        PidTagSwappedToDoData = 0x0E2D,

        /// <summary>
        /// Contains the value of the PidTagStoreEntryId property (section 2.1018) of the message when the value of the PidTagSwappedToDoData property (section 2.1027) is set.
        /// </summary>
        PidTagSwappedToDoStore = 0x0E2C,

        /// <summary>
        /// Contains the message ID of a Message object being submitted for optimization ([MS-OXOMSG] section 3.2.4.4).
        /// </summary>
        PidTagTargetEntryId = 0x3010,

        /// <summary>
        /// Contains the mail user's telecommunication device for the deaf (TTY/TDD) telephone number.
        /// </summary>
        PidTagTelecommunicationsDeviceForDeafTelephoneNumber = 0x3A4B,

        /// <summary>
        /// Contains the mail user's telex number. This property is returned from an NSPI server as a PtypMultipleBinary. Otherwise, the data type is PtypString.
        /// </summary>
        PidTagTelexNumber = 0x3A2C,

        /// <summary>
        /// Describes the controls used in the template that is used to retrieve address book information.
        /// </summary>
        PidTagTemplateData = 0x0001,

        /// <summary>
        /// Contains the value of the PidTagEntryId property (section 2.674), expressed as a Permanent Entry ID format.
        /// </summary>
        PidTagTemplateid = 0x3902,

        /// <summary>
        /// Specifies the character set of an attachment received via MIME with the content-type of text.
        /// </summary>
        PidTagTextAttachmentCharset = 0x371B,

        /// <summary>
        /// Contains the mail user's photo in .jpg format.
        /// </summary>
        PidTagThumbnailPhoto = 0x8C9E,

        /// <summary>
        /// Contains the mail user's job title.
        /// </summary>
        PidTagTitle = 0x3A17,

        /// <summary>
        /// Contains a value that correlates a Transport Neutral Encapsulation Format (TNEF) attachment with a message.
        /// </summary>
        PidTagTnefCorrelationKey = 0x007F,

        /// <summary>
        /// Contains flags associated with objects.
        /// </summary>
        PidTagToDoItemFlags = 0x0E2B,

        /// <summary>
        /// Contains an Address Book object's display name that is transmitted with the message.
        /// </summary>
        PidTagTransmittableDisplayName = 0x3A20,

        /// <summary>
        /// Contains transport-specific message envelope information for email.
        /// </summary>
        PidTagTransportMessageHeaders = 0x007D,

        /// <summary>
        /// Specifies whether the associated message was delivered through a trusted transport channel.
        /// </summary>
        PidTagTrustSender = 0x0E79,

        /// <summary>
        /// Contains an ASN.1 authentication certificate for a messaging user.
        /// </summary>
        PidTagUserCertificate = 0x3A22,

        /// <summary>
        /// Address book EntryID of the user logged on to the public folders.
        /// </summary>
        PidTagUserEntryId = 0x6619,

        /// <summary>
        /// Contains a list of certificates for the mail user.
        /// </summary>
        PidTagUserX509Certificate = 0x3A70,

        /// <summary>
        /// Contains view definitions.
        /// </summary>
        PidTagViewDescriptorBinary = 0x7001,

        /// <summary>
        /// The PidTagViewDescriptorName
        /// </summary>
        PidTagViewDescriptorName = 0x7006,

        /// <summary>
        /// Contains view definitions in string format.
        /// </summary>
        PidTagViewDescriptorStrings = 0x7002,

        /// <summary>
        /// Contains the View Descriptor version.
        /// </summary>
        PidTagViewDescriptorVersion = 0x7007,

        /// <summary>
        /// Contains a list of file names for the audio file attachments that are to be played as part of a message.
        /// </summary>
        PidTagVoiceMessageAttachmentOrder = 0x6805,

        /// <summary>
        /// Specifies the name of the caller who left the attached voice message, as provided by the voice network's caller ID system.
        /// </summary>
        PidTagVoiceMessageSenderName = 0x6803,

        /// <summary>
        /// Contains the date of the mail user's wedding anniversary.
        /// </summary>
        PidTagWeddingAnniversary = 0x3A41,

        /// <summary>
        /// Specifies the value of the PidTagEntryId property (section 2.674) of the user to whom the folder belongs.
        /// </summary>
        PidTagWlinkAddressBookEID = 0x6854,

        /// <summary>
        /// Specifies the value of the PidTagStoreEntryId property (section 2.1018) of the current user (not the owner of the folder).
        /// </summary>
        PidTagWlinkAddressBookStoreEID = 0x6891,

        /// <summary>
        /// Specifies the background color of the calendar.
        /// </summary>
        PidTagWlinkCalendarColor = 0x6853,

        /// <summary>
        /// Specifies the Client ID that allows the client to determine whether the shortcut was created on the current machine/user via an equality test.
        /// </summary>
        PidTagWlinkClientID = 0x6890,

        /// <summary>
        /// Specifies the EntryID of the folder pointed to by the shortcut.
        /// </summary>
        PidTagWlinkEntryId = 0x684C,

        /// <summary>
        /// Specifies conditions associated with the shortcut.
        /// </summary>
        PidTagWlinkFlags = 0x684A,

        /// <summary>
        /// Specifies the type of folder pointed to by the shortcut.
        /// </summary>
        PidTagWlinkFolderType = 0x684F,

        /// <summary>
        /// Specifies the value of the PidTagWlinkGroupHeaderID property (section 2.1060) of the group header associated with the shortcut.
        /// </summary>
        PidTagWlinkGroupClsid = 0x6850,

        /// <summary>
        /// Specifies the value of the PidTagNormalizedSubject (section 2.803) of the group header associated with the shortcut.
        /// </summary>
        PidTagWlinkGroupName = 0x6851,

        /// <summary>
        /// Specifies a variable-length binary property to be used to sort shortcuts lexicographically.
        /// </summary>
        PidTagWlinkOrdinal = 0x684B,

        /// <summary>
        /// Specifies the value of PidTagRecordKey property (section 2.901) of the folder pointed to by the shortcut.
        /// </summary>
        PidTagWlinkRecordKey = 0x684D,

        /// <summary>
        /// Specifies the type of group header.
        /// </summary>
        PidTagWlinkROGroupType = 0x6892,

        /// <summary>
        /// Specifies the section where the shortcut should be grouped.
        /// </summary>
        PidTagWlinkSection = 0x6852,

        /// <summary>
        /// Specifies the value of the PidTagStoreEntryId property (section 2.1018) of the folder pointed to by the shortcut.
        /// </summary>
        PidTagWlinkStoreEntryId = 0x684E,

        /// <summary>
        /// Specifies the type of navigation shortcut.
        /// </summary>
        PidTagWlinkType = 0x6849,
    }

    /// <summary>
    /// The enum of Property long ID (LID) related to Canonical name.
    /// </summary>
    public enum PidLidPropertyEnum : ushort
    {
        /// <summary>
        /// Specifies the state of the electronic addresses of the contact and represents a set of bit flags.
        /// </summary>
        PidLidAddressBookProviderArrayType = 0x00008029,

        /// <summary>
        /// Specifies which electronic address properties are set on the Contact object.
        /// </summary>
        PidLidAddressBookProviderEmailList = 0x00008028,

        /// <summary>
        /// Specifies the country code portion of the mailing address of the contact.
        /// </summary>
        PidLidAddressCountryCode = 0x000080DD,

        /// <summary>
        /// Specifies whether to automatically archive the message.
        /// </summary>
        PidLidAgingDontAgeMe = 0x0000850E,

        /// <summary>
        /// Specifies a list of all the attendees except for the organizer, including resources and unsendable attendees.
        /// </summary>
        PidLidAllAttendeesString = 0x00008238,

        /// <summary>
        /// This property is set to TRUE.
        /// </summary>
        PidLidAllowExternalCheck = 0x00008246,

        /// <summary>
        /// Specifies the EntryID of the Appointment object that represents an anniversary of the contact.
        /// </summary>
        PidLidAnniversaryEventEntryId = 0x0000804E,

        /// <summary>
        /// Specifies a bit field that describes the auxiliary state of the object.
        /// </summary>
        PidLidAppointmentAuxiliaryFlags = 0x00008207,

        /// <summary>
        /// Specifies the color to be used when displaying the Calendar object.
        /// </summary>
        PidLidAppointmentColor = 0x00008214,

        /// <summary>
        /// Indicates whether a Meeting Response object is a counter proposal.
        /// </summary>
        PidLidAppointmentCounterProposal = 0x00008257,

        /// <summary>
        /// Specifies the length of the event, in minutes.
        /// </summary>
        PidLidAppointmentDuration = 0x00008213,

        /// <summary>
        /// Indicates the date that the appointment ends.
        /// </summary>
        PidLidAppointmentEndDate = 0x00008211,

        /// <summary>
        /// Indicates the time that the appointment ends.
        /// </summary>
        PidLidAppointmentEndTime = 0x00008210,

        /// <summary>
        /// Specifies the end date and time for the event.
        /// </summary>
        PidLidAppointmentEndWhole = 0x0000820E,

        /// <summary>
        /// Indicates to the organizer the last sequence number that was sent to any attendee.
        /// </summary>
        PidLidAppointmentLastSequence = 0x00008203,

        /// <summary>
        /// Indicates the message class of the Meeting object to be generated from the Meeting Request object.
        /// </summary>
        PidLidAppointmentMessageClass = 0x00000024,

        /// <summary>
        /// Indicates whether attendees are not allowed to propose a new date and/or time for the meeting.
        /// </summary>
        PidLidAppointmentNotAllowPropose = 0x0000825A,

        /// <summary>
        /// Specifies the number of attendees who have sent counter proposals that have not been accepted or rejected by the organizer.
        /// </summary>
        PidLidAppointmentProposalNumber = 0x00008259,

        /// <summary>
        /// Indicates the proposed value for the PidLidAppointmentDuration property (section 2.11) for a counter proposal.
        /// </summary>
        PidLidAppointmentProposedDuration = 0x00008256,

        /// <summary>
        /// Specifies the proposed value for the PidLidAppointmentEndWhole property (section 2.14) for a counter proposal.
        /// </summary>
        PidLidAppointmentProposedEndWhole = 0x00008251,

        /// <summary>
        /// Specifies the proposed value for the PidLidAppointmentStartWhole property (section 2.29) for a counter proposal.
        /// </summary>
        PidLidAppointmentProposedStartWhole = 0x00008250,

        /// <summary>
        /// Specifies the dates and times when a recurring series occurs.
        /// </summary>
        PidLidAppointmentRecur = 0x00008216,

        /// <summary>
        /// Specifies the user who last replied to the meeting request or meeting update.
        /// </summary>
        PidLidAppointmentReplyName = 0x00008230,

        /// <summary>
        /// Specifies the date and time at which the attendee responded to a received meeting request or Meeting Update object.
        /// </summary>
        PidLidAppointmentReplyTime = 0x00008220,

        /// <summary>
        /// Specifies the sequence number of a Meeting object.
        /// </summary>
        PidLidAppointmentSequence = 0x00008201,

        /// <summary>
        /// Indicates the date and time at which the PidLidAppointmentSequence property (section 2.25) was last modified.
        /// </summary>
        PidLidAppointmentSequenceTime = 0x00008202,

        /// <summary>
        /// Identifies the date that the appointment starts.
        /// </summary>
        PidLidAppointmentStartDate = 0x00008212,

        /// <summary>
        /// Identifies the time that the appointment starts.
        /// </summary>
        PidLidAppointmentStartTime = 0x0000820F,

        /// <summary>
        /// Specifies the start date and time of the appointment.
        /// </summary>
        PidLidAppointmentStartWhole = 0x0000820D,

        /// <summary>
        /// Specifies a bit field that describes the state of the object.
        /// </summary>
        PidLidAppointmentStateFlags = 0x00008217,

        /// <summary>
        /// Specifies whether the event is an all-day event.
        /// </summary>
        PidLidAppointmentSubType = 0x00008215,

        /// <summary>
        /// Specifies time zone information that indicates the time zone of the PidLidAppointmentEndWhole property (section 2.14).
        /// </summary>
        PidLidAppointmentTimeZoneDefinitionEndDisplay = 0x0000825F,

        /// <summary>
        /// Specifies time zone information that describes how to convert the meeting date and time on a recurring series to and from UTC.
        /// </summary>
        PidLidAppointmentTimeZoneDefinitionRecur = 0x00008260,

        /// <summary>
        /// Specifies time zone information that indicates the time zone of the PidLidAppointmentStartWhole property (section 2.29).
        /// </summary>
        PidLidAppointmentTimeZoneDefinitionStartDisplay = 0x0000825E,

        /// <summary>
        /// Contains a list of unsendable attendees.
        /// </summary>
        PidLidAppointmentUnsendableRecipients = 0x0000825D,

        /// <summary>
        /// Indicates the time at which the appointment was last updated.
        /// </summary>
        PidLidAppointmentUpdateTime = 0x00008226,

        /// <summary>
        /// Specifies the date and time at which the meeting-related object was sent.
        /// </summary>
        PidLidAttendeeCriticalChange = 0x00000001,

        /// <summary>
        /// Indicates whether the value of the PidLidLocation property (section 2.159) is set to the PidTagDisplayName property (section 2.667).
        /// </summary>
        PidLidAutoFillLocation = 0x0000823A,

        /// <summary>
        /// Specifies to the application whether to create a Journal object for each action associated with this Contact object.
        /// </summary>
        PidLidAutoLog = 0x00008025,

        /// <summary>
        /// Specifies the options used in the automatic processing of email messages.
        /// </summary>
        PidLidAutoProcessState = 0x0000851A,

        /// <summary>
        /// Specifies whether to automatically start the conferencing application when a reminder for the start of a meeting is executed.
        /// </summary>
        PidLidAutoStartCheck = 0x00008244,

        /// <summary>
        /// Specifies billing information for the contact.
        /// </summary>
        PidLidBilling = 0x00008535,

        /// <summary>
        /// Specifies the EntryID of an optional Appointment object that represents the birthday of the contact.
        /// </summary>
        PidLidBirthdayEventEntryId = 0x0000804D,

        /// <summary>
        /// Specifies the birthday of a contact.
        /// </summary>
        PidLidBirthdayLocal = 0x000080DE,

        /// <summary>
        /// Contains the image to be used on a business card.
        /// </summary>
        PidLidBusinessCardCardPicture = 0x00008041,

        /// <summary>
        /// Contains user customization details for displaying a contact as a business card.
        /// </summary>
        PidLidBusinessCardDisplayDefinition = 0x00008040,

        /// <summary>
        /// Specifies the availability of a user for the event described by the object.
        /// </summary>
        PidLidBusyStatus = 0x00008205,

        /// <summary>
        /// Contains the value of the CalendarType field from the PidLidAppointmentRecur property (section 2.22).
        /// </summary>
        PidLidCalendarType = 0x0000001C,

        /// <summary>
        /// Contains the array of text labels assigned to this Message object.
        /// </summary>
        PidLidCategories = 0x00009000,

        /// <summary>
        /// Contains a list of all the sendable attendees who are also optional attendees.
        /// </summary>
        PidLidCcAttendeesString = 0x0000823C,

        /// <summary>
        /// Specifies a bit field that indicates how the Meeting object has changed.
        /// </summary>
        PidLidChangeHighlight = 0x00008204,

        /// <summary>
        /// Contains a list of the classification categories to which the associated Message object has been assigned.
        /// </summary>
        PidLidClassification = 0x000085B6,

        /// <summary>
        /// The PidLidClassificationDescription
        /// </summary>
        PidLidClassificationDescription = 0x000085B7,

        /// <summary>
        /// Contains the GUID that identifies the list of email classification categories used by a Message object.
        /// </summary>
        PidLidClassificationGuid = 0x000085B8,

        /// <summary>
        /// Indicates whether the message uses any classification categories.
        /// </summary>
        PidLidClassificationKeep = 0x000085BA,

        /// <summary>
        /// Indicates whether the contents of this message are regarded as classified information.
        /// </summary>
        PidLidClassified = 0x000085B5,

        /// <summary>
        /// Contains the value of the PidLidGlobalObjectId property (section 2.142) for an object that represents an Exception object to a recurring series, where the Year, Month, and Day fields are all zero.
        /// </summary>
        PidLidCleanGlobalObjectId = 0x00000023,

        /// <summary>
        ///  Indicates what actions the user has taken on this Meeting object.
        /// </summary>
        PidLidClientIntent = 0x00000015,

        /// <summary>
        /// Specifies the end date and time of the event in UTC.
        /// </summary>
        PidLidClipEnd = 0x00008236,

        /// <summary>
        /// Specifies the start date and time of the event in UTC.
        /// </summary>
        PidLidClipStart = 0x00008235,

        /// <summary>
        /// Specifies the document to be launched when the user joins the meeting.
        /// </summary>
        PidLidCollaborateDoc = 0x00008247,

        /// <summary>
        /// Indicates the end time for the Message object.
        /// </summary>
        PidLidCommonEnd = 0x00008517,

        /// <summary>
        /// Indicates the start time for the Message object.
        /// </summary>
        PidLidCommonStart = 0x00008516,

        /// <summary>
        /// Contains a list of company names, each of which is associated with a contact that is specified in the PidLidContacts property ([MS-OXCMSG] section 2.2.1.57.2).
        /// </summary>
        PidLidCompanies = 0x00008539,

        /// <summary>
        /// The PidLidConferencingCheck
        /// </summary>
        PidLidConferencingCheck = 0x00008240,

        /// <summary>
        /// Specifies the type of the meeting.
        /// </summary>
        PidLidConferencingType = 0x00008241,

        /// <summary>
        /// Specifies the character set used for a Contact object.
        /// </summary>
        PidLidContactCharacterSet = 0x00008023,

        /// <summary>
        /// Specifies the visible fields in the application's user interface that are used to help display the contact information.
        /// </summary>
        PidLidContactItemData = 0x00008007,

        /// <summary>
        /// Specifies the EntryID of the GAL contact to which the duplicate contact is linked.
        /// </summary>
        PidLidContactLinkedGlobalAddressListEntryId = 0x000080E2,

        /// <summary>
        /// Contains the elements of the PidLidContacts property (section 2.77).
        /// </summary>
        PidLidContactLinkEntry = 0x00008585,

        /// <summary>
        /// Specifies the GUID of the GAL contact to which the duplicate contact is linked.
        /// </summary>
        PidLidContactLinkGlobalAddressListLinkId = 0x000080E8,

        /// <summary>
        /// Specifies the state of the linking between the GAL contact and the duplicate contact.
        /// </summary>
        PidLidContactLinkGlobalAddressListLinkState = 0x000080E6,

        /// <summary>
        /// Contains a list of GAL contacts that were previously rejected for linking with the duplicate contact.
        /// </summary>
        PidLidContactLinkLinkRejectHistory = 0x000080E5,

        /// <summary>
        /// The PidLidContactLinkName
        /// </summary>
        PidLidContactLinkName = 0x00008586,

        /// <summary>
        /// Contains the list of SearchKeys for a Contact object linked to by the Message object.
        /// </summary>
        PidLidContactLinkSearchKey = 0x00008584,

        /// <summary>
        /// Contains a list of the SMTP addresses that are used by the contact.
        /// </summary>
        PidLidContactLinkSMTPAddressCache = 0x000080E3,

        /// <summary>
        /// Contains the PidTagDisplayName property (section 2.667) of each Address Book EntryID referenced in the value of the PidLidContactLinkEntry property (section 2.70).
        /// </summary>
        PidLidContacts = 0x0000853A,

        /// <summary>
        /// Contains text used to add custom text to a business card representation of a Contact object.
        /// </summary>
        PidLidContactUserField1 = 0x0000804F,

        /// <summary>
        /// Contains text used to add custom text to a business card representation of a Contact object.
        /// </summary>
        PidLidContactUserField2 = 0x00008050,

        /// <summary>
        /// Contains text used to add custom text to a business card representation of a Contact object.
        /// </summary>
        PidLidContactUserField3 = 0x00008051,

        /// <summary>
        /// Contains text used to add custom text to a business card representation of a Contact object.
        /// </summary>
        PidLidContactUserField4 = 0x00008052,

        /// <summary>
        /// Contains the time, in UTC, that an Email object was last received in the conversation, or the last time that the user modified the conversation action, whichever occurs later.
        /// </summary>
        PidLidConversationActionLastAppliedTime = 0x000085CA,

        /// <summary>
        /// Contains the maximum value of the PidTagMessageDeliveryTime property (section 2.780) of all of the Email objects modified in response to the last time that the user changed a conversation action on the client.
        /// </summary>
        PidLidConversationActionMaxDeliveryTime = 0x000085C8,

        /// <summary>
        /// Contains the EntryID for the destination folder.
        /// </summary>
        PidLidConversationActionMoveFolderEid = 0x000085C6,

        /// <summary>
        /// Contains the EntryID for a move to a folder in a different message store.
        /// </summary>
        PidLidConversationActionMoveStoreEid = 0x000085C7,

        /// <summary>
        /// Contains the version of the conversation action FAI message.
        /// </summary>
        PidLidConversationActionVersion = 0x000085CB,

        /// <summary>
        /// Specifies a sequential number to be used in the processing of a conversation action.
        /// </summary>
        PidLidConversationProcessed = 0x000085C9,

        /// <summary>
        /// Specifies the build number of the client application that sent the message.
        /// </summary>
        PidLidCurrentVersion = 0x00008552,

        /// <summary>
        /// Specifies the name of the client application that sent the message.
        /// </summary>
        PidLidCurrentVersionName = 0x00008554,

        /// <summary>
        /// Identifies the day interval for the recurrence pattern.
        /// </summary>
        PidLidDayInterval = 0x00000011,

        /// <summary>
        /// Identifies the day of the month for the appointment or meeting.
        /// </summary>
        PidLidDayOfMonth = 0x00001000,

        /// <summary>
        /// Indicates whether a delegate responded to the meeting request.
        /// </summary>
        PidLidDelegateMail = 0x00000009,

        /// <summary>
        /// This property is ignored by the server and is set to an empty string by the client.
        /// </summary>
        PidLidDepartment = 0x00008010,

        /// <summary>
        /// Specifies the directory server to be used.
        /// </summary>
        PidLidDirectory = 0x00008242,

        /// <summary>
        /// Specifies the 32-bit cyclic redundancy check (CRC) polynomial checksum, as specified in [ISO/IEC8802-3], calculated on the value of the PidLidDistributionListMembers property (section 2.96).
        /// </summary>
        PidLidDistributionListChecksum = 0x0000804C,

        /// <summary>
        /// Specifies the list of EntryIDs of the objects corresponding to the members of the personal distribution list.
        /// </summary>
        PidLidDistributionListMembers = 0x00008055,

        /// <summary>
        /// Specifies the name of the personal distribution list.
        /// </summary>
        PidLidDistributionListName = 0x00008053,

        /// <summary>
        /// Specifies the list of one-off EntryIDs corresponding to the members of the personal distribution list.
        /// </summary>
        PidLidDistributionListOneOffMembers = 0x00008054,

        /// <summary>
        /// Specifies the list of EntryIDs and one-off EntryIDs corresponding to the members of the personal distribution list.
        /// </summary>
        PidLidDistributionListStream = 0x00008064,

        /// <summary>
        /// Specifies the address type of an electronic address.
        /// </summary>
        PidLidEmail1AddressType = 0x00008082,

        /// <summary>
        /// Specifies the user-readable display name for the email address.
        /// </summary>
        PidLidEmail1DisplayName = 0x00008080,

        /// <summary>
        /// Specifies the email address of the contact.
        /// </summary>
        PidLidEmail1EmailAddress = 0x00008083,

        /// <summary>
        /// Specifies the SMTP email address that corresponds to the email address for the Contact object.
        /// </summary>
        PidLidEmail1OriginalDisplayName = 0x00008084,

        /// <summary>
        /// Specifies the EntryID of the object corresponding to this electronic address.
        /// </summary>
        PidLidEmail1OriginalEntryId = 0x00008085,

        /// <summary>
        /// Specifies the address type of the electronic address.
        /// </summary>
        PidLidEmail2AddressType = 0x00008092,

        /// <summary>
        /// Specifies the user-readable display name for the email address.
        /// </summary>
        PidLidEmail2DisplayName = 0x00008090,

        /// <summary>
        /// Specifies the email address of the contact.
        /// </summary>
        PidLidEmail2EmailAddress = 0x00008093,

        /// <summary>
        /// Specifies the SMTP email address that corresponds to the email address for the Contact object.
        /// </summary>
        PidLidEmail2OriginalDisplayName = 0x00008094,

        /// <summary>
        /// Specifies the EntryID of the object that corresponds to this electronic address.
        /// </summary>
        PidLidEmail2OriginalEntryId = 0x00008095,

        /// <summary>
        /// Specifies the address type of the electronic address.
        /// </summary>
        PidLidEmail3AddressType = 0x000080A2,

        /// <summary>
        /// Specifies the user-readable display name for the email address.
        /// </summary>
        PidLidEmail3DisplayName = 0x000080A0,

        /// <summary>
        /// Specifies the email address of the contact.
        /// </summary>
        PidLidEmail3EmailAddress = 0x000080A3,

        /// <summary>
        /// Specifies the SMTP email address that corresponds to the email address for the Contact object.
        /// </summary>
        PidLidEmail3OriginalDisplayName = 0x000080A4,

        /// <summary>
        /// Specifies the EntryID of the object that corresponds to this electronic address.
        /// </summary>
        PidLidEmail3OriginalEntryId = 0x000080A5,

        /// <summary>
        /// Identifies the end date of the recurrence range.
        /// </summary>
        PidLidEndRecurrenceDate = 0x0000000F,

        /// <summary>
        /// Identifies the end time of the recurrence range.
        /// </summary>
        PidLidEndRecurrenceTime = 0x00000010,

        /// <summary>
        /// Specifies the date and time, in UTC, within a recurrence pattern that an exception will replace.
        /// </summary>
        PidLidExceptionReplaceTime = 0x00008228,

        /// <summary>
        /// Contains the string value "FAX".
        /// </summary>
        PidLidFax1AddressType = 0x000080B2,

        /// <summary>
        /// Contains a user-readable display name, followed by the "@" character, followed by a fax number.
        /// </summary>
        PidLidFax1EmailAddress = 0x000080B3,

        /// <summary>
        /// Contains the same value as the PidTagNormalizedSubject property (section 2.803).
        /// </summary>
        PidLidFax1OriginalDisplayName = 0x000080B4,

        /// <summary>
        /// Specifies a one-off EntryID that corresponds to this fax address.
        /// </summary>
        PidLidFax1OriginalEntryId = 0x000080B5,

        /// <summary>
        /// Contains the string value "FAX".
        /// </summary>
        PidLidFax2AddressType = 0x000080C2,

        /// <summary>
        /// Contains a user-readable display name, followed by the "@" character, followed by a fax number.
        /// </summary>
        PidLidFax2EmailAddress = 0x000080C3,

        /// <summary>
        /// Contains the same value as the PidTagNormalizedSubject property (section 2.803).
        /// </summary>
        PidLidFax2OriginalDisplayName = 0x000080C4,

        /// <summary>
        /// Specifies a one-off EntryID corresponding to this fax address.
        /// </summary>
        PidLidFax2OriginalEntryId = 0x000080C5,

        /// <summary>
        /// Contains the string value "FAX".
        /// </summary>
        PidLidFax3AddressType = 0x000080D2,

        /// <summary>
        /// Contains a user-readable display name, followed by the "@" character, followed by a fax number.
        /// </summary>
        PidLidFax3EmailAddress = 0x000080D3,

        /// <summary>
        /// Contains the same value as the PidTagNormalizedSubject property (section 2.803).
        /// </summary>
        PidLidFax3OriginalDisplayName = 0x000080D4,

        /// <summary>
        /// Specifies a one-off EntryID that corresponds to this fax address.
        /// </summary>
        PidLidFax3OriginalEntryId = 0x000080D5,

        /// <summary>
        /// Indicates that the object is a Recurring Calendar object with one or more exceptions, and that at least one of the Exception Embedded Message objects has at least one RecipientRow structure, as described in [MS-OXCDATA] section 2.8.3.
        /// </summary>
        PidLidFExceptionalAttendees = 0x0000822B,

        /// <summary>
        /// Indicates that the Exception Embedded Message object has a body that differs from the Recurring Calendar object.
        /// </summary>
        PidLidFExceptionalBody = 0x00008206,

        /// <summary>
        /// Specifies the name under which to file a contact when displaying a list of contacts.
        /// </summary>
        PidLidFileUnder = 0x00008005,

        /// <summary>
        /// Specifies how to generate and recompute the value of the PidLidFileUnder property (section 2.132) when other contact name properties change.
        /// </summary>
        PidLidFileUnderId = 0x00008006,

        /// <summary>
        /// Specifies a list of possible values for the PidLidFileUnderId property (section 2.133).
        /// </summary>
        PidLidFileUnderList = 0x00008026,

        /// <summary>
        /// Indicates whether invitations have been sent for the meeting that this Meeting object represents.
        /// </summary>
        PidLidFInvited = 0x00008229,

        /// <summary>
        /// Contains user-specifiable text to be associated with the flag.
        /// </summary>
        PidLidFlagRequest = 0x00008530,

        /// <summary>
        /// Contains an index identifying one of a set of pre-defined text strings to be associated with the flag.
        /// </summary>
        PidLidFlagString = 0x000085C0,

        /// <summary>
        /// Indicates whether the Meeting Request object represents an exception to a recurring series, and whether it was forwarded (even when forwarded by the organizer) rather than being an invitation sent by the organizer.
        /// </summary>
        PidLidForwardInstance = 0x0000820A,

        /// <summary>
        /// Contains a list of RecipientRow structures, as described in [MS-OXCDATA] section 2.8.3, that indicate the recipients of a meeting forward.
        /// </summary>
        PidLidForwardNotificationRecipients = 0x00008261,

        /// <summary>
        /// Indicates whether the Calendar folder from which the meeting was opened is another user's calendar.
        /// </summary>
        PidLidFOthersAppointment = 0x0000822F,

        /// <summary>
        /// Specifies a URL path from which a client can retrieve free/busy status information for the contact.
        /// </summary>
        PidLidFreeBusyLocation = 0x000080D8,

        /// <summary>
        /// Contains an ID for an object that represents an exception to a recurring series.
        /// </summary>
        PidLidGlobalObjectId = 0x00000003,

        /// <summary>
        /// Specifies whether the attachment has a picture.
        /// </summary>
        PidLidHasPicture = 0x00008015,

        /// <summary>
        /// Specifies the complete address of the home address of the contact.
        /// </summary>
        PidLidHomeAddress = 0x0000801A,

        /// <summary>
        /// Specifies the country code portion of the home address of the contact.
        /// </summary>
        PidLidHomeAddressCountryCode = 0x000080DA,

        /// <summary>
        /// Specifies the business webpage URL of the contact.
        /// </summary>
        PidLidHtml = 0x0000802B,

        /// <summary>
        /// Identifies the day of the week for the appointment or meeting.
        /// </summary>
        PidLidICalendarDayOfWeekMask = 0x00001001,

        /// <summary>
        /// Contains the contents of the iCalendar MIME part of the original MIME message.
        /// </summary>
        PidLidInboundICalStream = 0x0000827A,

        /// <summary>
        /// Contains the name of the form associated with this message.
        /// </summary>
        PidLidInfoPathFormName = 0x000085B1,

        /// <summary>
        /// Specifies the instant messaging address of the contact.
        /// </summary>
        PidLidInstantMessagingAddress = 0x00008062,

        /// <summary>
        /// Contains the value of the PidLidBusyStatus property (section 2.47) on the Meeting object in the organizer's calendar at the time that the Meeting Request object or Meeting Update object was sent.
        /// </summary>
        PidLidIntendedBusyStatus = 0x00008224,

        /// <summary>
        /// Specifies the user-visible email account name through which the email message is sent.
        /// </summary>
        PidLidInternetAccountName = 0x00008580,

        /// <summary>
        /// Specifies the email account ID through which the email message is sent.
        /// </summary>
        PidLidInternetAccountStamp = 0x00008581,

        /// <summary>
        /// Specifies whether the contact is linked to other contacts.
        /// </summary>
        PidLidIsContactLinked = 0x000080E0,

        /// <summary>
        /// Indicates whether the object represents an exception (including an orphan instance).
        /// </summary>
        PidLidIsException = 0x0000000A,

        /// <summary>
        /// Specifies whether the object is associated with a recurring series.
        /// </summary>
        PidLidIsRecurring = 0x00000005,

        /// <summary>
        /// Indicates whether the user did not include any text in the body of the Meeting Response object.
        /// </summary>
        PidLidIsSilent = 0x00000004,

        /// <summary>
        /// Indicates whether the user did not include any text in the body of the Meeting Response object.
        /// </summary>
        PidLidLinkedTaskItems = 0x0000820C,

        /// <summary>
        /// Specifies the location of the event.
        /// </summary>
        PidLidLocation = 0x00008208,

        /// <summary>
        /// Indicates whether the document was sent by email or posted to a server folder during journaling.
        /// </summary>
        PidLidLogDocumentPosted = 0x00008711,

        /// <summary>
        /// Indicates whether the document was printed during journaling.
        /// </summary>
        PidLidLogDocumentPrinted = 0x0000870E,

        /// <summary>
        /// Indicates whether the document was sent to a routing recipient during journaling.
        /// </summary>
        PidLidLogDocumentRouted = 0x00008710,

        /// <summary>
        /// Indicates whether the document was saved during journaling.
        /// </summary>
        PidLidLogDocumentSaved = 0x0000870F,

        /// <summary>
        /// Contains the duration, in minutes, of the activity.
        /// </summary>
        PidLidLogDuration = 0x00008707,

        /// <summary>
        /// Contains the time, in UTC, at which the activity ended.
        /// </summary>
        PidLidLogEnd = 0x00008708,

        /// <summary>
        /// Contains metadata about the Journal object.
        /// </summary>
        PidLidLogFlags = 0x0000870C,

        /// <summary>
        /// Contains the time, in UTC, at which the activity began.
        /// </summary>
        PidLidLogStart = 0x00008706,

        /// <summary>
        /// Briefly describes the journal activity that is being recorded.
        /// </summary>
        PidLidLogType = 0x00008700,

        /// <summary>
        /// Contains an expanded description of the journal activity that is being recorded.
        /// </summary>
        PidLidLogTypeDesc = 0x00008712,

        /// <summary>
        /// Indicates the type of Meeting Request object or Meeting Update object.
        /// </summary>
        PidLidMeetingType = 0x00000026,

        /// <summary>
        /// Specifies the URL of the Meeting Workspace that is associated with a Calendar object.
        /// </summary>
        PidLidMeetingWorkspaceUrl = 0x00008209,

        /// <summary>
        /// Indicates the monthly interval of the appointment or meeting.
        /// </summary>
        PidLidMonthInterval = 0x00000013,

        /// <summary>
        /// Indicates the month of the year in which the appointment or meeting occurs.
        /// </summary>
        PidLidMonthOfYear = 0x00001006,

        /// <summary>
        /// Indicates the calculated month of the year in which the appointment or meeting occurs.
        /// </summary>
        PidLidMonthOfYearMask = 0x00000017,

        /// <summary>
        /// Specifies the URL to be launched when the user joins the meeting.
        /// </summary>
        PidLidNetShowUrl = 0x00008248,

        /// <summary>
        /// Indicates whether the recurrence pattern has an end date.
        /// </summary>
        PidLidNoEndDateFlag = 0x0000100B,

        /// <summary>
        /// Contains a list of all of the unsendable attendees who are also resources.
        /// </summary>
        PidLidNonSendableBcc = 0x00008538,

        /// <summary>
        /// Contains a list of all of the unsendable attendees who are also optional attendees.
        /// </summary>
        PidLidNonSendableCc = 0x00008537,

        /// <summary>
        /// Contains a list of all of the unsendable attendees who are also required attendees.
        /// </summary>
        PidLidNonSendableTo = 0x00008536,

        /// <summary>
        /// Contains the value from the response table.
        /// </summary>
        PidLidNonSendBccTrackStatus = 0x00008545,

        /// <summary>
        /// Contains the value from the response table.
        /// </summary>
        PidLidNonSendCcTrackStatus = 0x00008544,

        /// <summary>
        /// Contains the value from the response table.
        /// </summary>
        PidLidNonSendToTrackStatus = 0x00008543,

        /// <summary>
        /// Specifies the suggested background color of the Note object.
        /// </summary>
        PidLidNoteColor = 0x00008B00,

        /// <summary>
        /// Specifies the height of the visible message window in pixels.
        /// </summary>
        PidLidNoteHeight = 0x00008B03,

        /// <summary>
        /// Specifies the width of the visible message window in pixels.
        /// </summary>
        PidLidNoteWidth = 0x00008B02,

        /// <summary>
        /// Specifies the distance, in pixels, from the left edge of the screen that a user interface displays a Note object.
        /// </summary>
        PidLidNoteX = 0x00008B04,

        /// <summary>
        /// Specifies the distance, in pixels, from the top edge of the screen that a user interface displays a Note object.
        /// </summary>
        PidLidNoteY = 0x00008B05,

        /// <summary>
        /// Indicates the number of occurrences in the recurring appointment or meeting.
        /// </summary>
        PidLidOccurrences = 0x00001005,

        /// <summary>
        /// Indicates the original value of the PidLidLocation property (section 2.159) before a meeting update.
        /// </summary>
        PidLidOldLocation = 0x00000028,

        /// <summary>
        /// Indicates the recurrence pattern for the appointment or meeting.
        /// </summary>
        PidLidOldRecurrenceType = 0x00000018,

        /// <summary>
        /// Indicates the original value of the PidLidAppointmentEndWhole property (section 2.14) before a meeting update.
        /// </summary>
        PidLidOldWhenEndWhole = 0x0000002A,

        /// <summary>
        /// Indicates the original value of the PidLidAppointmentStartWhole property (section 2.29) before a meeting update.
        /// </summary>
        PidLidOldWhenStartWhole = 0x00000029,

        /// <summary>
        /// Specifies the password for a meeting on which the PidLidConferencingType property (section 2.66) has the value 0x00000002.
        /// </summary>
        PidLidOnlinePassword = 0x00008249,

        /// <summary>
        /// Specifies optional attendees.
        /// </summary>
        PidLidOptionalAttendees = 0x00000007,

        /// <summary>
        /// Specifies the email address of the organizer.
        /// </summary>
        PidLidOrganizerAlias = 0x00008243,

        /// <summary>
        /// Specifies the EntryID of the delegator’s message store.
        /// </summary>
        PidLidOriginalStoreEntryId = 0x00008237,

        /// <summary>
        /// Specifies the complete address of the other address of the contact.
        /// </summary>
        PidLidOtherAddress = 0x0000801C,

        /// <summary>
        /// Specifies the country code portion of the other address of the contact.
        /// </summary>
        PidLidOtherAddressCountryCode = 0x000080DC,

        /// <summary>
        /// Specifies the date and time at which a Meeting Request object was sent by the organizer.
        /// </summary>
        PidLidOwnerCriticalChange = 0x0000001A,

        /// <summary>
        /// Indicates the name of the owner of the mailbox.
        /// </summary>
        PidLidOwnerName = 0x0000822E,

        /// <summary>
        /// Specifies the synchronization state of the Document object that is in the Document Libraries folder of the site mailbox.
        /// </summary>
        PidLidPendingStateForSiteMailboxDocument = 0x000085E0,

        /// <summary>
        /// Indicates whether a time-flagged Message object is complete.
        /// </summary>
        PidLidPercentComplete = 0x00008102,

        /// <summary>
        /// Specifies which physical address is the mailing address for this contact.
        /// </summary>
        PidLidPostalAddressId = 0x00008022,

        /// <summary>
        /// Contains the contents of the title field from the XML of the Atom feed or RSS channel.
        /// </summary>
        PidLidPostRssChannel = 0x00008904,

        /// <summary>
        /// Contains the URL of the RSS or Atom feed from which the XML file came.
        /// </summary>
        PidLidPostRssChannelLink = 0x00008900,

        /// <summary>
        /// Contains a unique identifier for the RSS object.
        /// </summary>
        PidLidPostRssItemGuid = 0x00008903,

        /// <summary>
        /// Contains a hash of the feed XML computed by using an implementation-dependent algorithm.
        /// </summary>
        PidLidPostRssItemHash = 0x00008902,

        /// <summary>
        /// Contains the URL of the link from an RSS or Atom item.
        /// </summary>
        PidLidPostRssItemLink = 0x00008901,

        /// <summary>
        /// Contains the item element and all of its sub-elements from an RSS feed, or the entry element and all of its sub-elements from an Atom feed.
        /// </summary>
        PidLidPostRssItemXml = 0x00008905,

        /// <summary>
        /// Contains the user's preferred name for the RSS or Atom subscription.
        /// </summary>
        PidLidPostRssSubscription = 0x00008906,

        /// <summary>
        /// Indicates whether the end user wishes for this Message object to be hidden from other users who have access to the Message object.
        /// </summary>
        PidLidPrivate = 0x00008506,

        /// <summary>
        /// Indicates that the Meeting Response object was out-of-date when it was received.
        /// </summary>
        PidLidPromptSendUpdate = 0x00008045,

        /// <summary>
        /// Identifies the length, in minutes, of the appointment or meeting.
        /// </summary>
        PidLidRecurrenceDuration = 0x0000100D,

        /// <summary>
        /// Specifies a description of the recurrence pattern of the Calendar object.
        /// </summary>
        PidLidRecurrencePattern = 0x00008232,

        /// <summary>
        /// Specifies the recurrence type of the recurring series.
        /// </summary>
        PidLidRecurrenceType = 0x00008231,

        /// <summary>
        /// Specifies whether the object represents a recurring series.
        /// </summary>
        PidLidRecurring = 0x00008223,

        /// <summary>
        /// Specifies the value of the EntryID of the Contact object unless the Contact object is a copy of an earlier original.
        /// </summary>
        PidLidReferenceEntryId = 0x000085BD,

        /// <summary>
        /// Specifies the interval, in minutes, between the time at which the reminder first becomes overdue and the start time of the Calendar object.
        /// </summary>
        PidLidReminderDelta = 0x00008501,

        /// <summary>
        /// Specifies the filename of the sound that a client is to play when the reminder for that object becomes overdue.
        /// </summary>
        PidLidReminderFileParameter = 0x0000851F,

        /// <summary>
        /// Specifies whether the client is to respect the current values of the  PidLidReminderPlaySound property (section 2.221) and the PidLidReminderFileParameter property (section 2.219), or use the default values for those properties.
        /// </summary>
        PidLidReminderOverride = 0x0000851C,

        /// <summary>
        /// Specifies whether the client is to play a sound when the reminder becomes overdue.
        /// </summary>
        PidLidReminderPlaySound = 0x0000851E,

        /// <summary>
        /// Specifies whether a reminder is set on the object.
        /// </summary>
        PidLidReminderSet = 0x00008503,

        /// <summary>
        /// Specifies the point in time when a reminder transitions from pending to overdue.
        /// </summary>
        PidLidReminderSignalTime = 0x00008560,

        /// <summary>
        /// Specifies the initial signal time for objects that are not Calendar objects.
        /// </summary>
        PidLidReminderTime = 0x00008502,

        /// <summary>
        /// Indicates the time and date of the reminder for the appointment or meeting.
        /// </summary>
        PidLidReminderTimeDate = 0x00008505,

        /// <summary>
        /// Indicates the time of the reminder for the appointment or meeting.
        /// </summary>
        PidLidReminderTimeTime = 0x00008504,

        /// <summary>
        /// This property is not set and, if set, is ignored.
        /// </summary>
        PidLidReminderType = 0x0000851D,

        /// <summary>
        /// Indicates the remote status of the calendar item.
        /// </summary>
        PidLidRemoteStatus = 0x00008511,

        /// <summary>
        /// Identifies required attendees for the appointment or meeting.
        /// </summary>
        PidLidRequiredAttendees = 0x00000006,

        /// <summary>
        /// Identifies resource attendees for the appointment or meeting.
        /// </summary>
        PidLidResourceAttendees = 0x00000008,

        /// <summary>
        /// Specifies the response status of an attendee.
        /// </summary>
        PidLidResponseStatus = 0x00008218,

        /// <summary>
        /// Indicates whether the Meeting Request object or Meeting Update object has been processed.
        /// </summary>
        PidLidServerProcessed = 0x000085CC,

        /// <summary>
        /// Indicates what processing actions have been taken on this Meeting Request object or Meeting Update object.
        /// </summary>
        PidLidServerProcessingActions = 0x000085CD,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingAnonymity = 0x00008A19,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingBindingEntryId = 0x00008A2D,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingBrowseUrl = 0x00008A51,

        /// <summary>
        /// Indicates that the Message object relates to a special folder.
        /// </summary>
        PidLidSharingCapabilities = 0x00008A17,

        /// <summary>
        /// Contains a zero-length string.
        /// </summary>
        PidLidSharingConfigurationUrl = 0x00008A24,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingDataRangeEnd = 0x00008A45,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingDataRangeStart = 0x00008A44,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingDetail = 0x00008A2B,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingExtensionXml = 0x00008A21,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingFilter = 0x00008A13,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingFlags = 0x00008A0A,

        /// <summary>
        /// Indicates the type of Sharing Message object.
        /// </summary>
        PidLidSharingFlavor = 0x00008A18,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingFolderEntryId = 0x00008A15,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingIndexEntryId = 0x00008A2E,

        /// <summary>
        /// Contains the value of the PidTagEntryId property (section 2.674) for the Address Book object of the currently logged-on user.
        /// </summary>
        PidLidSharingInitiatorEntryId = 0x00008A09,

        /// <summary>
        /// Contains the value of the PidTagDisplayName property (section 2.667) from the Address Book object identified by the PidLidSharingInitiatorEntryId property (section 2.248).
        /// </summary>
        PidLidSharingInitiatorName = 0x00008A07,

        /// <summary>
        /// Contains the value of the PidTagSmtpAddress property (section 2.1010) from the Address Book object identified by the PidLidSharingInitiatorEntryId property (section 2.248).
        /// </summary>
        PidLidSharingInitiatorSmtp = 0x00008A08,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingInstanceGuid = 0x00008A1C,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingLastAutoSyncTime = 0x00008A55,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingLastSyncTime = 0x00008A1F,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingLocalComment = 0x00008A4D,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingLocalLastModificationTime = 0x00008A23,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingLocalName = 0x00008A0F,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingLocalPath = 0x00008A0E,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingLocalStoreUid = 0x00008A49,

        /// <summary>
        /// Contains the value of the PidTagContainerClass property (section 2.633) of the folder being shared.
        /// </summary>
        PidLidSharingLocalType = 0x00008A14,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingLocalUid = 0x00008A10,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingOriginalMessageEntryId = 0x00008A29,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingParentBindingEntryId = 0x00008A5C,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingParticipants = 0x00008A1E,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingPermissions = 0x00008A1B,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingProviderExtension = 0x00008A0B,

        /// <summary>
        /// Contains the value "%xAE.F0.06.00.00.00.00.00.C0.00.00.00.00.00.00.46".
        /// </summary>
        PidLidSharingProviderGuid = 0x00008A01,

        /// <summary>
        /// Contains a user-displayable name of the sharing provider identified by the PidLidSharingProviderGuid property (section 2.266).
        /// </summary>
        PidLidSharingProviderName = 0x00008A02,

        /// <summary>
        /// Contains a URL related to the sharing provider identified by the PidLidSharingProviderGuid property (section 2.266).
        /// </summary>
        PidLidSharingProviderUrl = 0x00008A03,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingRangeEnd = 0x00008A47,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingRangeStart = 0x00008A46,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingReciprocation = 0x00008A1A,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingRemoteByteSize = 0x00008A4B,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingRemoteComment = 0x00008A2F,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingRemoteCrc = 0x00008A4C,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingRemoteLastModificationTime = 0x00008A22,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingRemoteMessageCount = 0x00008A4F,

        /// <summary>
        /// Contains the value of the PidTagDisplayName property (section 2.667) on the folder being shared.
        /// </summary>
        PidLidSharingRemoteName = 0x00008A05,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingRemotePass = 0x00008A0D,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingRemotePath = 0x00008A04,

        /// <summary>
        /// Contains a hexadecimal string representation of the value of the PidTagStoreEntryId property (section 2.1018) on the folder being shared.
        /// </summary>
        PidLidSharingRemoteStoreUid = 0x00008A48,

        /// <summary>
        /// Contains the same value as the PidLidSharingLocalType property (section 2.259).
        /// </summary>
        PidLidSharingRemoteType = 0x00008A1D,

        /// <summary>
        /// Contains the EntryID of the folder being shared.
        /// </summary>
        PidLidSharingRemoteUid = 0x00008A06,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingRemoteUser = 0x00008A0C,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingRemoteVersion = 0x00008A5B,

        /// <summary>
        /// Contains the time at which the recipient of the sharing request sent a sharing response.
        /// </summary>
        PidLidSharingResponseTime = 0x00008A28,

        /// <summary>
        /// Contains the type of response with which the recipient of the sharing request responded.
        /// </summary>
        PidLidSharingResponseType = 0x00008A27,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingRoamLog = 0x00008A4E,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingStart = 0x00008A25,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingStatus = 0x00008A00,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingStop = 0x00008A26,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingSyncFlags = 0x00008A60,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingSyncInterval = 0x00008A2A,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingTimeToLive = 0x00008A2C,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingTimeToLiveAuto = 0x00008A56,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingWorkingHoursDays = 0x00008A42,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingWorkingHoursEnd = 0x00008A41,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingWorkingHoursStart = 0x00008A40,

        /// <summary>
        /// Contains a value that is ignored by the server no matter what value is generated by the client.
        /// </summary>
        PidLidSharingWorkingHoursTimeZone = 0x00008A43,

        /// <summary>
        /// Specifies how a Message object is handled by the client in relation to certain user interface actions by the user, such as deleting a message.
        /// </summary>
        PidLidSideEffects = 0x00008510,

        /// <summary>
        /// Indicates that the original MIME message contained a single MIME part.
        /// </summary>
        PidLidSingleBodyICal = 0x0000827B,

        /// <summary>
        /// Indicates whether the Message object has no end-user visible attachments.
        /// </summary>
        PidLidSmartNoAttach = 0x00008514,

        /// <summary>
        /// Specifies which folder a message was in before it was filtered into the Junk Email folder.
        /// </summary>
        PidLidSpamOriginalFolder = 0x0000859C,

        /// <summary>
        /// Identifies the start date of the recurrence pattern.
        /// </summary>
        PidLidStartRecurrenceDate = 0x0000000D,

        /// <summary>
        /// Identifies the start time of the recurrence pattern.
        /// </summary>
        PidLidStartRecurrenceTime = 0x0000000E,

        /// <summary>
        /// Indicates the acceptance state of the task.
        /// </summary>
        PidLidTaskAcceptanceState = 0x0000812A,

        /// <summary>
        /// Indicates whether a task assignee has replied to a task request for this Task object.
        /// </summary>
        PidLidTaskAccepted = 0x00008108,

        /// <summary>
        /// Indicates the number of minutes that the user actually spent working on a task.
        /// </summary>
        PidLidTaskActualEffort = 0x00008110,

        /// <summary>
        /// Specifies the name of the user that last assigned the task.
        /// </summary>
        PidLidTaskAssigner = 0x00008121,

        /// <summary>
        /// Contains a stack of entries, each of which represents a task assigner.
        /// </summary>
        PidLidTaskAssigners = 0x00008117,

        /// <summary>
        /// Indicates that the task is complete.
        /// </summary>
        PidLidTaskComplete = 0x0000811C,

        /// <summary>
        /// The client can set this property, but it has no impact on the Task-Related Objects Protocol and is ignored by the server.
        /// </summary>
        PidLidTaskCustomFlags = 0x00008139,

        /// <summary>
        /// Specifies the date when the user completed work on the task.
        /// </summary>
        PidLidTaskDateCompleted = 0x0000810F,

        /// <summary>
        /// Indicates whether new occurrences remain to be generated.
        /// </summary>
        PidLidTaskDeadOccurrence = 0x00008109,

        /// <summary>
        /// Specifies the date by which the user expects work on the task to be complete.
        /// </summary>
        PidLidTaskDueDate = 0x00008105,

        /// <summary>
        /// Indicates the number of minutes that the user expects to work on a task.
        /// </summary>
        PidLidTaskEstimatedEffort = 0x00008111,

        /// <summary>
        /// Indicates that the Task object was originally created by the action of the current user or user agent instead of by the processing of a task request.
        /// </summary>
        PidLidTaskFCreator = 0x0000811E,

        /// <summary>
        /// Indicates the accuracy of the PidLidTaskOwner property (section 2.328).
        /// </summary>
        PidLidTaskFFixOffline = 0x0000812C,

        /// <summary>
        /// Indicates whether the task includes a recurrence pattern.
        /// </summary>
        PidLidTaskFRecurring = 0x00008126,

        /// <summary>
        /// Contains a unique GUID for this task, which is used to locate an existing task upon receipt of a task response or task update.
        /// </summary>
        PidLidTaskGlobalId = 0x00008519,

        /// <summary>
        /// Indicates the type of change that was last made to the Task object.
        /// </summary>
        PidLidTaskHistory = 0x0000811A,

        /// <summary>
        /// Contains the name of the user who most recently assigned the task, or the user to whom it was most recently assigned.
        /// </summary>
        PidLidTaskLastDelegate = 0x00008125,

        /// <summary>
        /// Contains the date and time of the most recent change made to the Task object.
        /// </summary>
        PidLidTaskLastUpdate = 0x00008115,

        /// <summary>
        /// Contains the name of the most recent user to have been the owner of the task.
        /// </summary>
        PidLidTaskLastUser = 0x00008122,

        /// <summary>
        /// Specifies the assignment status of the embedded Task object.
        /// </summary>
        PidLidTaskMode = 0x00008518,

        /// <summary>
        /// Provides optimization hints about the recipients of a Task object.
        /// </summary>
        PidLidTaskMultipleRecipients = 0x00008120,

        /// <summary>
        /// Not used. The client can set this property, but it has no impact on the Task-Related Objects Protocol and is ignored by the server.
        /// </summary>
        PidLidTaskNoCompute = 0x00008124,

        /// <summary>
        /// Provides an aid to custom sorting of Task objects.
        /// </summary>
        PidLidTaskOrdinal = 0x00008123,

        /// <summary>
        /// Contains the name of the owner of the task.
        /// </summary>
        PidLidTaskOwner = 0x0000811F,

        /// <summary>
        /// Indicates the role of the current user relative to the Task object.
        /// </summary>
        PidLidTaskOwnership = 0x00008129,

        /// <summary>
        /// Contains a RecurrencePattern structure that provides information about recurring tasks.
        /// </summary>
        PidLidTaskRecurrence = 0x00008116,

        /// <summary>
        /// Indicates whether future instances of recurring tasks need reminders, even though the value of the PidLidReminderSet property (section 2.222) is 0x00.
        /// </summary>
        PidLidTaskResetReminder = 0x00008107,

        /// <summary>
        /// Not used. The client can set this property, but it has no impact on the Task-Related Objects Protocol and is ignored by the server.
        /// </summary>
        PidLidTaskRole = 0x00008127,

        /// <summary>
        /// Specifies the date on which the user expects work on the task to begin.
        /// </summary>
        PidLidTaskStartDate = 0x00008104,

        /// <summary>
        /// Indicates the current assignment state of the Task object.
        /// </summary>
        PidLidTaskState = 0x00008113,

        /// <summary>
        /// Specifies the status of a task.
        /// </summary>
        PidLidTaskStatus = 0x00008101,

        /// <summary>
        /// Indicates whether the task assignee has been requested to send an email message update upon completion of the assigned task.
        /// </summary>
        PidLidTaskStatusOnComplete = 0x00008119,

        /// <summary>
        /// Indicates whether the task assignee has been requested to send a task update when the assigned Task object changes.
        /// </summary>
        PidLidTaskUpdates = 0x0000811B,

        /// <summary>
        /// Indicates which copy is the latest update of a Task object.
        /// </summary>
        PidLidTaskVersion = 0x00008112,

        /// <summary>
        /// This property is set by the client but is ignored by the server.
        /// </summary>
        PidLidTeamTask = 0x00008103,

        /// <summary>
        /// Specifies information about the time zone of a recurring meeting.
        /// </summary>
        PidLidTimeZone = 0x0000000C,

        /// <summary>
        /// The PidLidTimeZoneDescription
        /// </summary>
        PidLidTimeZoneDescription = 0x00008234,

        /// <summary>
        /// Specifies a human-readable description of the time zone that is represented by the data in the PidLidTimeZoneStruct property (section 2.342).
        /// </summary>
        PidLidTimeZoneStruct = 0x00008233,

        /// <summary>
        /// Contains a list of all of the sendable attendees who are also required attendees.
        /// </summary>
        PidLidToAttendeesString = 0x0000823B,

        /// <summary>
        /// Contains the current time, in UTC, which is used to determine the sort order of objects in a consolidated to-do list.
        /// </summary>
        PidLidToDoOrdinalDate = 0x000085A0,

        /// <summary>
        /// Contains the numerals 0 through 9 that are used to break a tie when the PidLidToDoOrdinalDate property (section 2.344) is used to perform a sort of objects.
        /// </summary>
        PidLidToDoSubOrdinal = 0x000085A1,

        /// <summary>
        /// Contains user-specifiable text to identify this Message object in a consolidated to-do list.
        /// </summary>
        PidLidToDoTitle = 0x000085A4,

        /// <summary>
        /// Specifies whether Transport Neutral Encapsulation Format (TNEF) is to be included on a message when the message is converted from TNEF to MIME or SMTP format.
        /// </summary>
        PidLidUseTnef = 0x00008582,

        /// <summary>
        /// Contains the value of the PidTagMessageDeliveryTime  property (section 2.780) when modifying the PidLidFlagRequest property (section 2.136).
        /// </summary>
        PidLidValidFlagStringProof = 0x000085BF,

        /// <summary>
        /// Specifies the voting option that a respondent has selected.
        /// </summary>
        PidLidVerbResponse = 0x00008524,

        /// <summary>
        /// Specifies what voting responses the user can make in response to the message.
        /// </summary>
        PidLidVerbStream = 0x00008520,

        /// <summary>
        /// Specifies the wedding anniversary of the contact, at midnight in the client's local time zone, and is saved without any time zone conversions.
        /// </summary>
        PidLidWeddingAnniversaryLocal = 0x000080DF,

        /// <summary>
        /// Identifies the number of weeks that occur between each meeting.
        /// </summary>
        PidLidWeekInterval = 0x00000012,

        /// <summary>
        /// Contains the value of the PidLidLocation property (section 2.159) from the associated Meeting object.
        /// </summary>
        PidLidWhere = 0x00000002,

        /// <summary>
        /// Specifies the complete address of the work address of the contact.
        /// </summary>
        PidLidWorkAddress = 0x0000801B,

        /// <summary>
        /// Specifies the city or locality portion of the work address of the contact.
        /// </summary>
        PidLidWorkAddressCity = 0x00008046,

        /// <summary>
        /// Specifies the country or region portion of the work address of the contact.
        /// </summary>
        PidLidWorkAddressCountry = 0x00008049,

        /// <summary>
        /// Specifies the country code portion of the work address of the contact.
        /// </summary>
        PidLidWorkAddressCountryCode = 0x000080DB,

        /// <summary>
        /// Specifies the postal code (ZIP code) portion of the work address of the contact.
        /// </summary>
        PidLidWorkAddressPostalCode = 0x00008048,

        /// <summary>
        /// Specifies the post office box portion of the work address of the contact.
        /// </summary>
        PidLidWorkAddressPostOfficeBox = 0x0000804A,

        /// <summary>
        /// Specifies the state or province portion of the work address of the contact.
        /// </summary>
        PidLidWorkAddressState = 0x00008047,

        /// <summary>
        /// Specifies the street portion of the work address of the contact.
        /// </summary>
        PidLidWorkAddressStreet = 0x00008045,

        /// <summary>
        /// Indicates the yearly interval of the appointment or meeting.
        /// </summary>
        PidLidYearInterval = 0x00000014,

        /// <summary>
        /// Specifies the phonetic pronunciation of the company name of the contact.
        /// </summary>
        PidLidYomiCompanyName = 0x0000802E,

        /// <summary>
        /// Specifies the phonetic pronunciation of the given name of the contact.
        /// </summary>
        PidLidYomiFirstName = 0x0000802C,

        /// <summary>
        /// Specifies the phonetic pronunciation of the surname of the contact.
        /// </summary>
        PidLidYomiLastName = 0x0000802D
    }

    /// <summary>
    /// The dictionary and method about property name.
    /// </summary>
    public class PropertyNameMap
    {
        #region PidNamePropertyDic
        /// <summary>
        /// The dictionary of PidName and property.
        /// </summary>
        public Dictionary<string, string> PidNamePropertyDic = new Dictionary<string, string>
        {
            { "{PS_INTERNET_HEADERS}::Accept-Language", "PidNameAcceptLanguage" },
            { "{PS_PUBLIC_STRINGS}::AppName", "PidNameApplicationName" },
            { "{PSETID_Attachment}::AttachmentMacContentType", "PidNameAttachmentMacContentType" },
            { "{PSETID_Attachment}::AttachmentMacInfo", "PidNameAttachmentMacInfo" },
            { "{PSETID_UnifiedMessaging}::UMAudioNotes", "PidNameAudioNotes" },
            { "{PS_PUBLIC_STRINGS}::Author", "PidNameAuthor" },
            { "{PSETID_UnifiedMessaging}::AsrData", "PidNameAutomaticSpeechRecognitionData" },
            { "{PS_PUBLIC_STRINGS}::ByteCount", "PidNameByteCount" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:calendar:attendeerole", "PidNameCalendarAttendeeRole" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:calendar:busystatus", "PidNameCalendarBusystatus" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:calendar:contact", "PidNameCalendarContact" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:calendar:contacturl", "PidNameCalendarContactUrl" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:calendar:created", "PidNameCalendarCreated" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:calendar:descriptionurl", "PidNameCalendarDescriptionUrl" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:calendar:duration", "PidNameCalendarDuration" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:calendar:exdate", "PidNameCalendarExceptionDate" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:calendar:exrule", "PidNameCalendarExceptionRule" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:calendar:geolatitude", "PidNameCalendarGeoLatitude" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:calendar:geolongitude", "PidNameCalendarGeoLongitude" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:calendar:instancetype", "PidNameCalendarInstanceType" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:calendar:isorganizer", "PidNameCalendarIsOrganizer" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:calendar:lastmodified", "PidNameCalendarLastModified" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:calendar:locationurl", "PidNameCalendarLocationUrl},PidNameLocationUrl" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:calendar:meetingstatus", "PidNameCalendarMeetingStatus" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:calendar:method", "PidNameCalendarMethod" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:calendar:prodid", "PidNameCalendarProductId" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:calendar:recurrenceidrange", "PidNameCalendarRecurrenceIdRange" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:calendar:reminderoffset", "PidNameCalendarReminderOffset" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:calendar:resources", "PidNameCalendarResources" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:calendar:rsvp", "PidNameCalendarRsvp" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:calendar:sequence", "PidNameCalendarSequence" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:calendar:timezone", "PidNameCalendarTimeZone" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:calendar:timezoneid", "PidNameCalendarTimeZoneId" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:calendar:transparent", "PidNameCalendarTransparent" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:calendar:uid", "PidNameCalendarUid" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:calendar:version", "PidNameCalendarVersion" },
            { "{PS_PUBLIC_STRINGS}::Category", "PidNameCategory" },
            { "{PS_PUBLIC_STRINGS}::CharCount", "PidNameCharacterCount" },
            { "{PS_PUBLIC_STRINGS}::Comments", "PidNameComments" },
            { "{PS_PUBLIC_STRINGS}::Company", "PidNameCompany" },
            { "{PS_INTERNET_HEADERS}::Content-Base", "PidNameContentBase" },
            { "{PS_INTERNET_HEADERS}::Content-Class", "PidNameContentClass" },
            { "{PS_INTERNET_HEADERS}::Content-Type", "PidNameContentType" },
            { "{PS_PUBLIC_STRINGS}::CreateDtmRo", "PidNameCreateDateTimeReadOnly" },
            { "{PS_INTERNET_HEADERS}::Xref", "PidNameCrossReference" },
            { "{PS_PUBLIC_STRINGS}::DAV:id", "PidNameDavId" },
            { "{PS_PUBLIC_STRINGS}::DAV:iscollection", "PidNameDavIsCollection" },
            { "{PS_PUBLIC_STRINGS}::DAV:isstructureddocument", "PidNameDavIsStructuredDocument" },
            { "{PS_PUBLIC_STRINGS}::DAV:parentname", "PidNameDavParentName" },
            { "{PS_PUBLIC_STRINGS}::DAV:uid", "PidNameDavUid" },
            { "{PS_PUBLIC_STRINGS}::DocParts", "PidNameDocumentParts" },
            { "{PS_PUBLIC_STRINGS}::EditTime", "PidNameEditTime" },
            { "{PS_PUBLIC_STRINGS}::http://schemas.microsoft.com/exchange/intendedbusystatus", "PidNameExchangeIntendedBusyStatus" },
            { "{PS_PUBLIC_STRINGS}::http://schemas.microsoft.com/exchange/junkemailmovestamp", "PidNameExchangeJunkEmailMoveStamp" },
            { "{PS_PUBLIC_STRINGS}::http://schemas.microsoft.com/exchange/modifyexceptionstruct", "PidNameExchangeModifyExceptionStructure" },
            { "{PS_PUBLIC_STRINGS}::http://schemas.microsoft.com/exchange/nomodifyexceptions", "PidNameExchangeNoModifyExceptions" },
            { "{PS_PUBLIC_STRINGS}::http://schemas.microsoft.com/exchange/patternend", "PidNameExchangePatternEnd" },
            { "{PS_PUBLIC_STRINGS}::http://schemas.microsoft.com/exchange/patternstart", "PidNameExchangePatternStart" },
            { "{PS_PUBLIC_STRINGS}::http://schemas.microsoft.com/exchange/reminderinterval", "PidNameExchangeReminderInterval" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas-microsoft-com:exch-data:baseschema", "PidNameExchDatabaseSchema" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas-microsoft-com:exch-data:expected-content-class", "PidNameExchDataExpectedContentClass" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas-microsoft-com:exch-data:schema-collection-ref", "PidNameExchDataSchemaCollectionReference" },
            { "{PSETID_XmlExtractedEntities}::XmlExtractedAddresses", "PidNameExtractedAddresses" },
            { "{PSETID_XmlExtractedEntities}::XmlExtractedContacts", "PidNameExtractedContacts" },
            { "{PSETID_XmlExtractedEntities}::XmlExtractedEmails", "PidNameExtractedEmails" },
            { "{PSETID_XmlExtractedEntities}::XmlExtractedMeetings", "PidNameExtractedMeetings" },
            { "{PSETID_XmlExtractedEntities}::XmlExtractedPhones", "PidNameExtractedPhones" },
            { "{PSETID_XmlExtractedEntities}::XmlExtractedTasks", "PidNameExtractedTasks" },
            { "{PSETID_XmlExtractedEntities}::XmlExtractedUrls", "PidNameExtractedUrls" },
            { "{PS_INTERNET_HEADERS}::From", "PidNameFrom" },
            { "{PS_PUBLIC_STRINGS}::HeadingPairs", "PidNameHeadingPairs" },
            { "{PS_PUBLIC_STRINGS}::HiddenCount", "PidNameHiddenCount" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:httpmail:calendar", "PidNameHttpmailCalendar" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:httpmail:htmldescription", "PidNameHttpmailHtmlDescription" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:httpmail:sendmsg", "PidNameHttpmailSendMessage" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:calendar:rdate", "PidNameICalendarRecurrenceDate" },
            { "{PS_PUBLIC_STRINGS}::urn:schemas:calendar:rrule", "PidNameICalendarRecurrenceRule" },
            { "{PS_INTERNET_HEADERS}::Subject", "PidNameInternetSubject" },
            { "{PS_PUBLIC_STRINGS}::Keywords", "PidNameKeywords" },
            { "{PS_PUBLIC_STRINGS}::LastAuthor", "PidNameLastAuthor" },
            { "{PS_PUBLIC_STRINGS}::LastPrinted", "PidNameLastPrinted" },
            { "{PS_PUBLIC_STRINGS}::LastSaveDtm", "PidNameLastSaveDateTime" },
            { "{PS_PUBLIC_STRINGS}::LineCount", "PidNameLineCount" },
            { "{PS_PUBLIC_STRINGS}::LinksDirty", "PidNameLinksDirty" },
            { "{PS_PUBLIC_STRINGS}::Manager", "PidNameManager" },
            { "{PS_PUBLIC_STRINGS}::DoNotForward", "PidNameMeetingDoNotForward" },
            { "{PS_INTERNET_HEADERS}::msip_labels", "PidNameMSIPLabels" },
            { "{PS_PUBLIC_STRINGS}::MMClipCount", "PidNameMultimediaClipCount" },
            { "{PS_PUBLIC_STRINGS}::NoteCount", "PidNameNoteCount" },
            { "{PS_PUBLIC_STRINGS}::OMSAccountGuid", "PidNameOMSAccountGuid" },
            { "{PS_PUBLIC_STRINGS}::OMSMobileModel", "PidNameOMSMobileModel" },
            { "{PS_PUBLIC_STRINGS}::OMSScheduleTime", "PidNameOMSScheduleTime" },
            { "{PS_PUBLIC_STRINGS}::OMSServiceType", "PidNameOMSServiceType" },
            { "{PS_PUBLIC_STRINGS}::OMSSourceType", "PidNameOMSSourceType" },
            { "{PS_PUBLIC_STRINGS}::PageCount", "PidNamePageCount" },
            { "{PS_PUBLIC_STRINGS}::ParCount", "PidNameParagraphCount" },
            { "{PS_PUBLIC_STRINGS}::http://schemas.microsoft.com/outlook/phishingstamp", "PidNamePhishingStamp" },
            { "{PS_PUBLIC_STRINGS}::PresFormat", "PidNamePresentationFormat" },
            { "{PS_PUBLIC_STRINGS}::quarantine-original-sender", "PidNameQuarantineOriginalSender" },
            { "{PS_PUBLIC_STRINGS}::RevNumber", "PidNameRevisionNumber" },
            { "{PS_PUBLIC_STRINGS}::DRMLicense", "PidNameRightsManagementLicense" },
            { "{PS_PUBLIC_STRINGS}::Scale", "PidNameScale" },
            { "{PS_PUBLIC_STRINGS}::Security", "PidNameSecurity" },
            { "{PS_PUBLIC_STRINGS}::SlideCount", "PidNameSlideCount" },
            { "{PS_PUBLIC_STRINGS}::Subject", "PidNameSubject" },
            { "{PS_PUBLIC_STRINGS}::Template", "PidNameTemplate" },
            { "{PS_PUBLIC_STRINGS}::Thumbnail", "PidNameThumbnail" },
            { "{PS_PUBLIC_STRINGS}::Title", "PidNameTitle" },
            { "{PS_PUBLIC_STRINGS}::WordCount", "PidNameWordCount" },
            { "{PS_INTERNET_HEADERS}::X-CallID", "PidNameXCallId" },
            { "{PS_INTERNET_HEADERS}::X-FaxNumberOfPages", "PidNameXFaxNumberOfPages" },
            { "{PS_INTERNET_HEADERS}::X-RequireProtectedPlayOnPhone", "PidNameXRequireProtectedPlayOnPhone" },
            { "{PS_INTERNET_HEADERS}::X-CallingTelephoneNumber", "PidNameXSenderTelephoneNumber" },
            { "{PS_INTERNET_HEADERS}::X-Sharing-Browse-Url", "PidNameXSharingBrowseUrl" },
            { "{PS_INTERNET_HEADERS}::X-Sharing-Capabilities", "PidNameXSharingCapabilities" },
            { "{PS_INTERNET_HEADERS}::X-Sharing-Config-Url", "PidNameXSharingConfigUrl" },
            { "{PS_INTERNET_HEADERS}::X-Sharing-Exended-Caps", "PidNameXSharingExendedCaps" },
            { "{PS_INTERNET_HEADERS}::X-Sharing-Flavor", "PidNameXSharingFlavor" },
            { "{PS_INTERNET_HEADERS}::X-Sharing-Instance-Guid", "PidNameXSharingInstanceGuid" },
            { "{PS_INTERNET_HEADERS}::X-Sharing-Local-Type", "PidNameXSharingLocalType" },
            { "{PS_INTERNET_HEADERS}::X-Sharing-Provider-Guid", "PidNameXSharingProviderGuid" },
            { "{PS_INTERNET_HEADERS}::X-Sharing-Provider-Name", "PidNameXSharingProviderName" },
            { "{PS_INTERNET_HEADERS}::X-Sharing-Provider-Url", "PidNameXSharingProviderUrl" },
            { "{PS_INTERNET_HEADERS}::X-Sharing-Remote-Name", "PidNameXSharingRemoteName" },
            { "{PS_INTERNET_HEADERS}::X-Sharing-Remote-Path", "PidNameXSharingRemotePath" },
            { "{PS_INTERNET_HEADERS}::X-Sharing-Remote-Store-Uid", "PidNameXSharingRemoteStoreUid" },
            { "{PS_INTERNET_HEADERS}::X-Sharing-Remote-Type", "PidNameXSharingRemoteType" },
            { "{PS_INTERNET_HEADERS}::X-Sharing-Remote-Uid", "PidNameXSharingRemoteUid" },
            { "{PS_INTERNET_HEADERS}::X-AttachmentOrder", "PidNameXVoiceMessageAttachmentOrder" },
            { "{PS_INTERNET_HEADERS}::X-VoiceMessageDuration", "PidNameXVoiceMessageDuration" },
            { "{PS_INTERNET_HEADERS}::X-VoiceMessageSenderName", "PidNameXVoiceMessageSenderName" }
        };
        #endregion

        /// <summary>
        /// Get the property name by GUID.
        /// </summary>
        /// <param name="guidValue">The GUID value.</param>
        /// <returns>The the property name</returns>
        public string GetPropSetNameByGuid(Guid guidValue)
        {
            switch (guidValue.ToString())
            {
                case "00020329-0000-0000-C000-000000000046":
                    {
                        return "PS_PUBLIC_STRINGS";
                    }

                case "00062008-0000-0000-C000-000000000046":
                    {
                        return "PSETID_Common";
                    }

                case "00062004-0000-0000-C000-000000000046":
                    {
                        return "PSETID_Address";
                    }

                case "00020386-0000-0000-C000-000000000046":
                    {
                        return "PS_INTERNET_HEADERS";
                    }

                case "00062002-0000-0000-C000-000000000046":
                    {
                        return "PSETID_Appointment";
                    }

                case "6ED8DA90-450B-101B-98DA-00AA003F1305":
                    {
                        return "PSETID_Meeting";
                    }

                case "0006200A-0000-0000-C000-000000000046":
                    {
                        return "PSETID_Log";
                    }

                case "41F28F13-83F4-4114-A584-EEDB5A6B0BFF":
                    {
                        return "PSETID_Messaging";
                    }

                case "0006200E-0000-0000-C000-000000000046":
                    {
                        return "PSETID_Note";
                    }

                case "00062041-0000-0000-C000-000000000046":
                    {
                        return "PSETID_PostRss";
                    }

                case "00062003-0000-0000-C000-000000000046":
                    {
                        return "PSETID_Task";
                    }

                case "4442858E-A9E3-4E80-B900-317A210CC15B":
                    {
                        return "PSETID_UnifiedMessaging";
                    }

                case "00020328-0000-0000-C000-000000000046":
                    {
                        return "PS_MAPI";
                    }

                case "71035549-0739-4DCB-9163-00F0580DBBDF":
                    {
                        return "PSETID_AirSync";
                    }

                case "00062040-0000-0000-C000-000000000046":
                    {
                        return "PSETID_Sharing";
                    }

                case "23239608-685D-4732-9C55-4C95CB4E8E33":
                    {
                        return "PSETID_XmlExtractedEntities";
                    }

                default:
                    {
                        return "unknown";
                    }
            }
        }

        /// <summary>
        /// Get the property name by GUID and property name.
        /// </summary>
        /// <param name="propName">The property name.</param>
        /// <param name="guidValue">The GUID value.</param>
        /// <returns>The property identity</returns>
        public string GetPropIdentity(string propName, Guid guidValue)
        {
            string propSet = this.GetPropSetNameByGuid(guidValue);
            string key = "{ " + propSet + " }::" + propName;

            return string.Format("{0} ({1}), PropertySet: {2}", this.PidNamePropertyDic[key], propName, propSet) + ", " + guidValue.ToString();
        }
    }
}
