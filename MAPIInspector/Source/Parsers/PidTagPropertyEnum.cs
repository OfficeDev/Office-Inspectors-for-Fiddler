namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The enum of Tag Property ID related to Canonical name.
    /// </summary>
    public enum PidTagPropertyEnum : ushort
    {
        /// <summary>
        /// Describes the controls used in the template that is used to retrieve address book information.
        /// </summary>
        PidTagTemplateData = 0x0001,

        /// <summary>
        /// Specifies whether the sender permits the message to be auto-forwarded.
        /// </summary>
        PidTagAlternateRecipientAllowed = 0x0002,

        /// <summary>
        /// Contains a series of instructions that can be executed to format an address and the data that is needed to execute those instructions.
        /// </summary>
        PidTagScriptData = 0x0004,

        /// <summary>
        /// Contains text included in an automatically-generated message.
        /// </summary>
        PidTagAutoForwardComment = 0x0004,

        /// <summary>
        /// Indicates that a Message object has been automatically generated or automatically forwarded.
        /// </summary>
        PidTagAutoForwarded = 0x0005,

        /// <summary>
        /// Contains the date and time, in UTC, at which the sender prefers that the message be delivered.
        /// </summary>
        PidTagDeferredDeliveryTime = 0x000F,

        /// <summary>
        /// Contains the delivery time for a delivery status notification, as specified [RFC3464], or a message disposition notification, as specified in [RFC3798].
        /// </summary>
        PidTagDeliverTime = 0x0010,

        /// <summary>
        /// Contains the time, in UTC, after which a client wants to receive an expiry event if the message arrives late.
        /// </summary>
        PidTagExpiryTime = 0x0015,

        /// <summary>
        /// Indicates the level of importance assigned by the end user to the Message object.
        /// </summary>
        PidTagImportance = 0x0017,

        /// <summary>
        /// Denotes the specific type of the Message object.
        /// </summary>
        PidTagMessageClass = 0x001A,

        /// <summary>
        /// Indicates whether an email sender requests an email delivery receipt from the messaging system.
        /// </summary>
        PidTagOriginatorDeliveryReportRequested = 0x0023,

        /// <summary>
        /// Contains the search key that is used to correlate the original message and the reports about the original message.
        /// </summary>
        PidTagParentKey = 0x0025,

        /// <summary>
        /// Indicates the client's request for the priority with which the message is to be sent by the messaging system.
        /// </summary>
        PidTagPriority = 0x0026,

        /// <summary>
        /// Specifies whether the email sender requests a read receipt from all recipients when this email message is read or opened.
        /// </summary>
        PidTagReadReceiptRequested = 0x0029,

        /// <summary>
        /// Contains the sent time for a message disposition notification, as specified in [RFC3798].
        /// </summary>
        PidTagReceiptTime = 0x002A,

        /// <summary>
        /// Specifies whether adding additional or different recipients is prohibited for the email message when forwarding the email message.
        /// </summary>
        PidTagRecipientReassignmentProhibited = 0x002B,

        /// <summary>
        /// Contains the sensitivity value of the original email message.
        /// </summary>
        PidTagOriginalSensitivity = 0x002E,

        /// <summary>
        /// Specifies the time, in UTC, that the sender has designated for an associated work item to be due.
        /// </summary>
        PidTagReplyTime = 0x0030,

        /// <summary>
        /// Contains the data that is used to correlate the report and the original message.
        /// </summary>
        PidTagReportTag = 0x0031,

        /// <summary>
        /// Indicates the last time that the contact list that is controlled by the PidTagJunkIncludeContacts property (section 2.749) was updated.
        /// </summary>
        PidTagReportTime = 0x0032,

        /// <summary>
        /// Indicates the sender's assessment of the sensitivity of the Message object.
        /// </summary>
        PidTagSensitivity = 0x0036,

        /// <summary>
        /// Contains the subject of the email message.
        /// </summary>
        PidTagSubject = 0x0037,

        /// <summary>
        /// Contains the current time, in UTC, when the email message is submitted.
        /// </summary>
        PidTagClientSubmitTime = 0x0039,

        /// <summary>
        /// Contains the display name for the entity (usually a server agent) that generated the report message.
        /// </summary>
        PidTagReportName = 0x003A,

        /// <summary>
        /// Contains a binary-comparable key that represents the end user who is represented by the sending mailbox owner.
        /// </summary>
        PidTagSentRepresentingSearchKey = 0x003B,

        /// <summary>
        /// Contains the prefix for the subject of the message.
        /// </summary>
        PidTagSubjectPrefix = 0x003D,

        /// <summary>
        /// Contains the address book EntryID of the mailbox receiving the Email object.
        /// </summary>
        PidTagReceivedByEntryId = 0x003F,

        /// <summary>
        /// Contains the email message receiver's display name.
        /// </summary>
        PidTagReceivedByName = 0x0040,

        /// <summary>
        /// Contains the identifier of the end user who is represented by the sending mailbox owner.
        /// </summary>
        PidTagSentRepresentingEntryId = 0x0041,

        /// <summary>
        /// Contains the display name for the end user who is represented by the sending mailbox owner.
        /// </summary>
        PidTagSentRepresentingName = 0x0042,

        /// <summary>
        /// Contains an address book EntryID that identifies the end user represented by the receiving mailbox owner.
        /// </summary>
        PidTagReceivedRepresentingEntryId = 0x0043,

        /// <summary>
        /// Contains the display name for the end user represented by the receiving mailbox owner.
        /// </summary>
        PidTagReceivedRepresentingName = 0x0044,

        /// <summary>
        /// Specifies an entry ID that identifies the application that generated a report message.
        /// </summary>
        PidTagReportEntryId = 0x0045,

        /// <summary>
        /// Contains an address book EntryID.
        /// </summary>
        PidTagReadReceiptEntryId = 0x0046,

        /// <summary>
        /// Contains a message identifier assigned by a message transfer agent.
        /// </summary>
        PidTagMessageSubmissionId = 0x0047,

        /// <summary>
        /// Specifies the subject of the original message.
        /// </summary>
        PidTagOriginalSubject = 0x0049,

        /// <summary>
        /// Designates the PidTagMessageClass property ([MS-OXCMSG] section 2.2.1.3) from the original message.
        /// </summary>
        PidTagOriginalMessageClass = 0x004B,

        /// <summary>
        /// Contains an address book EntryID structure ([MS-OXCDATA] section 2.2.5.2) and is defined in report messages to identify the user who sent the original message.
        /// </summary>
        PidTagOriginalAuthorEntryId = 0x004C,

        /// <summary>
        /// Contains the display name of the sender of the original message referenced by a report message.
        /// </summary>
        PidTagOriginalAuthorName = 0x004D,

        /// <summary>
        /// Specifies the original email message's submission date and time, in UTC.
        /// </summary>
        PidTagOriginalSubmitTime = 0x004E,

        /// <summary>
        /// Identifies a FlatEntryList structure ([MS-OXCDATA] section 2.3.3) of address book EntryIDs for recipients that are to receive a reply.
        /// </summary>
        PidTagReplyRecipientEntries = 0x004F,

        /// <summary>
        /// Contains a list of display names for recipients that are to receive a reply.
        /// </summary>
        PidTagReplyRecipientNames = 0x0050,

        /// <summary>
        /// Identifies an address book search key that contains a binary-comparable key that is used to identify correlated objects for a search.
        /// </summary>
        PidTagReceivedBySearchKey = 0x0051,

        /// <summary>
        /// Identifies an address book search key that contains a binary-comparable key of the end user represented by the receiving mailbox owner.
        /// </summary>
        PidTagReceivedRepresentingSearchKey = 0x0052,

        /// <summary>
        /// Contains an address book search key.
        /// </summary>
        PidTagReadReceiptSearchKey = 0x0053,

        /// <summary>
        /// Contains an address book search key representing the entity (usually a server agent) that generated the report message.
        /// </summary>
        PidTagReportSearchKey = 0x0054,

        /// <summary>
        /// Contains the delivery time, in UTC, from the original message.
        /// </summary>
        PidTagOriginalDeliveryTime = 0x0055,

        /// <summary>
        /// Indicates that the receiving mailbox owner is one of the primary recipients of this email message.
        /// </summary>
        PidTagMessageToMe = 0x0057,

        /// <summary>
        /// Indicates that the receiving mailbox owner is a carbon copy (Cc) recipient of this email message.
        /// </summary>
        PidTagMessageCcMe = 0x0058,

        /// <summary>
        /// Indicates that the receiving mailbox owner is a primary or a carbon copy (Cc) recipient of this email message.
        /// </summary>
        PidTagMessageRecipientMe = 0x0059,

        /// <summary>
        /// Contains the value of the original message sender's PidTagSenderName property (section 2.995), and is set on delivery report messages.
        /// </summary>
        PidTagOriginalSenderName = 0x005A,

        /// <summary>
        /// Contains an address book EntryID that is set on delivery report messages.
        /// </summary>
        PidTagOriginalSenderEntryId = 0x005B,

        /// <summary>
        /// Contains an address book search key set on the original email message.
        /// </summary>
        PidTagOriginalSenderSearchKey = 0x005C,

        /// <summary>
        /// Contains the display name of the end user who is represented by the original email message sender.
        /// </summary>
        PidTagOriginalSentRepresentingName = 0x005D,

        /// <summary>
        /// Identifies an address book EntryID that contains the entry identifier of the end user who is represented by the original message sender.
        /// </summary>
        PidTagOriginalSentRepresentingEntryId = 0x005E,

        /// <summary>
        /// Identifies an address book search key that contains the SearchKey of the end user who is represented by the original message sender.
        /// </summary>
        PidTagOriginalSentRepresentingSearchKey = 0x005F,

        /// <summary>
        /// Contains the value of the PidLidAppointmentStartWhole property (section 2.29).
        /// </summary>
        PidTagStartDate = 0x0060,

        /// <summary>
        /// Contains the value of the PidLidAppointmentEndWhole property (section 2.14).
        /// </summary>
        PidTagEndDate = 0x0061,

        /// <summary>
        /// Specifies a quasi-unique value among all of the Calendar objects in a user's mailbox.
        /// </summary>
        PidTagOwnerAppointmentId = 0x0062,

        /// <summary>
        /// Indicates whether a response is requested to a Message object.
        /// </summary>
        PidTagResponseRequested = 0x0063,

        /// <summary>
        /// Contains an email address type.
        /// </summary>
        PidTagSentRepresentingAddressType = 0x0064,

        /// <summary>
        /// Contains an email address for the end user who is represented by the sending mailbox owner.
        /// </summary>
        PidTagSentRepresentingEmailAddress = 0x0065,

        /// <summary>
        /// Contains the value of the original message sender's PidTagSenderAddressType property (section 2.991).
        /// </summary>
        PidTagOriginalSenderAddressType = 0x0066,

        /// <summary>
        /// Contains the value of the original message sender's PidTagSenderEmailAddress property (section 2.992).
        /// </summary>
        PidTagOriginalSenderEmailAddress = 0x0067,

        /// <summary>
        /// Contains the address type of the end user who is represented by the original email message sender.
        /// </summary>
        PidTagOriginalSentRepresentingAddressType = 0x0068,

        /// <summary>
        /// Contains the email address of the end user who is represented by the original email message sender.
        /// </summary>
        PidTagOriginalSentRepresentingEmailAddress = 0x0069,

        /// <summary>
        /// Contains an unchanging copy of the original subject.
        /// </summary>
        PidTagConversationTopic = 0x0070,

        /// <summary>
        /// Indicates the relative position of this message within a conversation thread.
        /// </summary>
        PidTagConversationIndex = 0x0071,

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
        /// Contains the email message receiver's email address type.
        /// </summary>
        PidTagReceivedByAddressType = 0x0075,

        /// <summary>
        /// Contains the email message receiver's email address.
        /// </summary>
        PidTagReceivedByEmailAddress = 0x0076,

        /// <summary>
        /// Contains the email address type for the end user represented by the receiving mailbox owner.
        /// </summary>
        PidTagReceivedRepresentingAddressType = 0x0077,

        /// <summary>
        /// Contains the email address for the end user represented by the receiving mailbox owner.
        /// </summary>
        PidTagReceivedRepresentingEmailAddress = 0x0078,

        /// <summary>
        /// Contains transport-specific message envelope information for email.
        /// </summary>
        PidTagTransportMessageHeaders = 0x007D,

        /// <summary>
        /// Contains a value that correlates a Transport Neutral Encapsulation Format (TNEF) attachment with a message.
        /// </summary>
        PidTagTnefCorrelationKey = 0x007F,

        /// <summary>
        /// Contains a string indicating whether the original message was displayed to the user or deleted (report messages only).
        /// </summary>
        PidTagReportDisposition = 0x0080,

        /// <summary>
        /// Contains a description of the action that a client has performed on behalf of a user (report messages only).
        /// </summary>
        PidTagReportDispositionMode = 0x0081,

        /// <summary>
        /// Contains the maximum occupancy of the room.
        /// </summary>
        PidTagAddressBookRoomCapacity = 0x0807,

        /// <summary>
        /// Contains a description of the Resource object.
        /// </summary>
        PidTagAddressBookRoomDescription = 0x0809,

        /// <summary>
        /// Contains an integer value that indicates a reason for delivery failure.
        /// </summary>
        PidTagNonDeliveryReportReasonCode = 0x0C04,

        /// <summary>
        /// Contains the diagnostic code for a delivery status notification, as specified in [RFC3464].
        /// </summary>
        PidTagNonDeliveryReportDiagCode = 0x0C05,

        /// <summary>
        /// Specifies whether the client sends a non-read receipt.
        /// </summary>
        PidTagNonReceiptNotificationRequested = 0x0C06,

        /// <summary>
        /// Specifies whether an email sender requests suppression of nondelivery receipts.
        /// </summary>
        PidTagOriginatorNonDeliveryReportRequested = 0x0C08,

        /// <summary>
        /// Represents the recipient type of a recipient on the message.
        /// </summary>
        PidTagRecipientType = 0x0C15,

        /// <summary>
        /// Indicates whether a reply is requested to a Message object.
        /// </summary>
        PidTagReplyRequested = 0x0C17,

        /// <summary>
        /// Identifies an address book EntryID that contains the address book EntryID of the sending mailbox owner.
        /// </summary>
        PidTagSenderEntryId = 0x0C19,

        /// <summary>
        /// Contains the display name of the sending mailbox owner.
        /// </summary>
        PidTagSenderName = 0x0C1A,

        /// <summary>
        /// Contains supplementary information about a delivery status notification, as specified in [RFC3464].
        /// </summary>
        PidTagSupplementaryInfo = 0x0C1B,

        /// <summary>
        /// Identifies an address book search key.
        /// </summary>
        PidTagSenderSearchKey = 0x0C1D,

        /// <summary>
        /// Contains the email address type of the sending mailbox owner.
        /// </summary>
        PidTagSenderAddressType = 0x0C1E,

        /// <summary>
        /// Contains the email address of the sending mailbox owner.
        /// </summary>
        PidTagSenderEmailAddress = 0x0C1F,

        /// <summary>
        /// Contains the value of the Status field for a delivery status notification, as specified in [RFC3464].
        /// </summary>
        PidTagNonDeliveryReportStatusCode = 0x0C20,

        /// <summary>
        /// Contains the value of the Remote-MTA field for a delivery status notification, as specified in [RFC3464].
        /// </summary>
        PidTagRemoteMessageTransferAgent = 0x0C21,

        /// <summary>
        /// Indicates that the original message is to be deleted after it is sent.
        /// </summary>
        PidTagDeleteAfterSubmit = 0x0E01,

        /// <summary>
        /// Contains a list of blind carbon copy (Bcc) recipient display names.
        /// </summary>
        PidTagDisplayBcc = 0x0E02,

        /// <summary>
        /// Contains a list of carbon copy (Cc) recipient display names.
        /// </summary>
        PidTagDisplayCc = 0x0E03,

        /// <summary>
        /// Contains a list of the primary recipient display names, separated by semicolons, when an email message has primary recipients .
        /// </summary>
        PidTagDisplayTo = 0x0E04,

        /// <summary>
        /// Specifies the time (in UTC) when the server received the message.
        /// </summary>
        PidTagMessageDeliveryTime = 0x0E06,

        /// <summary>
        /// Specifies the status of the Message object.
        /// </summary>
        PidTagMessageFlags = 0x0E07,

        /// <summary>
        /// Contains the size, in bytes, consumed by the Message object on the server.
        /// </summary>
        PidTagMessageSize = 0x0E08,

        /// <summary>
        /// Specifies the 64-bit version of the PidTagMessageSize property (section 2.787).
        /// </summary>
        PidTagMessageSizeExtended = 0x0E08,

        /// <summary>
        /// Contains the EntryID of the folder where messages or subfolders reside.
        /// </summary>
        PidTagParentEntryId = 0x0E09,

        /// <summary>
        /// Specifies whether another mail agent has ensured that the message will be delivered.
        /// </summary>
        PidTagResponsibility = 0x0E0F,

        /// <summary>
        /// Identifies all of the recipients of the current message.
        /// </summary>
        PidTagMessageRecipients = 0x0E12,

        /// <summary>
        /// Identifies all attachments to the current message.
        /// </summary>
        PidTagMessageAttachments = 0x0E13,

        /// <summary>
        /// Specifies the status of a message in a contents table.
        /// </summary>
        PidTagMessageStatus = 0x0E17,

        /// <summary>
        /// Indicates whether the Message object contains at least one attachment.
        /// </summary>
        PidTagHasAttachments = 0x0E1B,

        /// <summary>
        /// Contains the normalized subject of the message.
        /// </summary>
        PidTagNormalizedSubject = 0x0E1D,

        /// <summary>
        /// Indicates whether the PidTagBody property (section 2.609) and the PidTagRtfCompressed property (section 2.932) contain the same text (ignoring formatting).
        /// </summary>
        PidTagRtfInSync = 0x0E1F,

        /// <summary>
        /// Contains the size, in bytes, consumed by the Attachment object on the server.
        /// </summary>
        PidTagAttachSize = 0x0E20,

        /// <summary>
        /// Identifies the Attachment object within its Message object.
        /// </summary>
        PidTagAttachNumber = 0x0E21,

        /// <summary>
        /// Specifies the first server that a client is to use to send the email with.
        /// </summary>
        PidTagPrimarySendAccount = 0x0E28,

        /// <summary>
        /// Specifies the server that a client is currently attempting to use to send email.
        /// </summary>
        PidTagNextSendAcct = 0x0E29,

        /// <summary>
        /// Contains flags associated with objects.
        /// </summary>
        PidTagToDoItemFlags = 0x0E2B,

        /// <summary>
        /// Contains the value of the PidTagStoreEntryId property (section 2.1018) of the message when the value of the PidTagSwappedToDoData property (section 2.1027) is set.
        /// </summary>
        PidTagSwappedToDoStore = 0x0E2C,

        /// <summary>
        /// Contains a secondary storage location for flags when sender flags or sender reminders are supported.
        /// </summary>
        PidTagSwappedToDoData = 0x0E2D,

        /// <summary>
        /// Indicates whether a message has been read.
        /// </summary>
        PidTagRead = 0x0E69,

        /// <summary>
        /// Contains security attributes in XML.
        /// </summary>
        PidTagSecurityDescriptorAsXml = 0x0E6A,

        /// <summary>
        /// Specifies whether the associated message was delivered through a trusted transport channel.
        /// </summary>
        PidTagTrustSender = 0x0E79,

        /// <summary>
        /// Contains the calculated security descriptor for the item.
        /// </summary>
        PidTagExchangeNTSecurityDescriptor = 0x0E84,

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
        /// Indicates the operations available to the client for the object.
        /// </summary>
        PidTagAccess = 0x0FF4,

        /// <summary>
        /// Identifies the type of the row.
        /// </summary>
        PidTagRowType = 0x0FF5,

        /// <summary>
        /// Contains an object on an NSPI server.
        /// </summary>
        PidTagInstanceKey = 0x0FF6,

        /// <summary>
        /// Indicates the client's access level to the object.
        /// </summary>
        PidTagAccessLevel = 0x0FF7,

        /// <summary>
        /// A 16-byte constant that is present on all Address Book objects, but is not present on objects in an offline address book.
        /// </summary>
        PidTagMappingSignature = 0x0FF8,

        /// <summary>
        /// Contains a unique binary-comparable identifier for a specific object.
        /// </summary>
        PidTagRecordKey = 0x0FF9,

        /// <summary>
        /// Contains the unique EntryID of the message store where an object resides.
        /// </summary>
        PidTagStoreEntryId = 0x0FFB,

        /// <summary>
        /// Indicates the type of Server object.
        /// </summary>
        PidTagObjectType = 0x0FFE,

        /// <summary>
        /// Contains the information to identify many different types of messaging objects.
        /// </summary>
        PidTagEntryId = 0x0FFF,

        /// <summary>
        /// Contains message body text in plain text format.
        /// </summary>
        PidTagBody = 0x1000,

        /// <summary>
        /// Contains the optional text for a report message.
        /// </summary>
        PidTagReportText = 0x1001,

        /// <summary>
        /// Contains a count of the significant characters of the message text.
        /// </summary>
        PidTagRtfSyncBodyCount = 0x1007,

        /// <summary>
        /// Contains significant characters that appear at the beginning of the message text.
        /// </summary>
        PidTagRtfSyncBodyTag = 0x1008,

        /// <summary>
        /// Contains message body text in compressed RTF format.
        /// </summary>
        PidTagRtfCompressed = 0x1009,

        /// <summary>
        /// Contains a count of the ignorable characters that appear before the significant characters of the message.
        /// </summary>
        PidTagRtfSyncPrefixCount = 0x1010,

        /// <summary>
        /// Contains a count of the ignorable characters that appear after the significant characters of the message.
        /// </summary>
        PidTagRtfSyncTrailingCount = 0x1011,

        /// <summary>
        /// Contains message body text in HTML format.
        /// </summary>
        PidTagHtml = 0x1013,

        /// <summary>
        /// Contains the HTML body of the Message object.
        /// </summary>
        PidTagBodyHtml = 0x1013,

        /// <summary>
        /// Contains a globally unique Uniform Resource Identifier (URI) that serves as a label for the current message body.
        /// </summary>
        PidTagBodyContentLocation = 0x1014,

        /// <summary>
        /// Contains a GUID that corresponds to the current message body.
        /// </summary>
        PidTagBodyContentId = 0x1015,

        /// <summary>
        /// Indicates the best available format for storing the message body.
        /// </summary>
        PidTagNativeBody = 0x1016,

        /// <summary>
        /// Corresponds to the message-id field.
        /// </summary>
        PidTagInternetMessageId = 0x1035,

        /// <summary>
        /// Contains a list of message IDs that specify the messages to which this reply is related.
        /// </summary>
        PidTagInternetReferences = 0x1039,

        /// <summary>
        /// Contains the value of the original message's PidTagInternetMessageId property (section 2.739) value.
        /// </summary>
        PidTagInReplyToId = 0x1042,

        /// <summary>
        /// Contains a URI that provides detailed help information for the mailing list from which an email message was sent.
        /// </summary>
        PidTagListHelp = 0x1043,

        /// <summary>
        /// Contains the URI that subscribes a recipient to a message’s associated mailing list.
        /// </summary>
        PidTagListSubscribe = 0x1044,

        /// <summary>
        /// Contains the URI that unsubscribes a recipient from a message’s associated mailing list.
        /// </summary>
        PidTagListUnsubscribe = 0x1045,

        /// <summary>
        /// Contains the message ID of the original message included in replies or resent messages.
        /// </summary>
        PidTagOriginalMessageId = 0x1046,

        /// <summary>
        /// Specifies which icon is to be used by a user interface when displaying a group of Message objects.
        /// </summary>
        PidTagIconIndex = 0x1080,

        /// <summary>
        /// Specifies the last verb executed for the message item to which it is related.
        /// </summary>
        PidTagLastVerbExecuted = 0x1081,

        /// <summary>
        /// Contains the date and time, in UTC, during which the operation represented in the PidTagLastVerbExecuted property (section 2.758) took place.
        /// </summary>
        PidTagLastVerbExecutionTime = 0x1082,

        /// <summary>
        /// Specifies the flag state of the Message object.
        /// </summary>
        PidTagFlagStatus = 0x1090,

        /// <summary>
        /// Specifies the date and time, in UTC, that the Message object was flagged as complete.
        /// </summary>
        PidTagFlagCompleteTime = 0x1091,

        /// <summary>
        /// Specifies the flag color of the Message object.
        /// </summary>
        PidTagFollowupIcon = 0x1095,

        /// <summary>
        /// Indicates the user's preference for viewing external content (such as links to images on an HTTP server) in the message body.
        /// </summary>
        PidTagBlockStatus = 0x1096,

        /// <summary>
        /// Contains the date and time, in UTC, when the appointment or meeting starts.
        /// </summary>
        PidTagICalendarStartTime = 0x10C3,

        /// <summary>
        /// Contains the date and time, in UTC, when an appointment or meeting ends.
        /// </summary>
        PidTagICalendarEndTime = 0x10C4,

        /// <summary>
        /// Identifies a specific instance of a recurring appointment.
        /// </summary>
        PidTagCdoRecurrenceid = 0x10C5,

        /// <summary>
        /// Contains the date and time, in UTC, for the activation of the next reminder.
        /// </summary>
        PidTagICalendarReminderNextTime = 0x10CA,

        /// <summary>
        /// Specifies the hide or show status of a folder.
        /// </summary>
        PidTagAttributeHidden = 0x10F4,

        /// <summary>
        /// Indicates whether an item can be modified or deleted.
        /// </summary>
        PidTagAttributeReadOnly = 0x10F6,

        /// <summary>
        /// Contains a unique identifier for a recipient in a message's recipient table.
        /// </summary>
        PidTagRowid = 0x3000,

        /// <summary>
        /// Contains the display name of the folder.
        /// </summary>
        PidTagDisplayName = 0x3001,

        /// <summary>
        /// Contains the email address type of a Message object.
        /// </summary>
        PidTagAddressType = 0x3002,

        /// <summary>
        /// Contains the email address of a Message object.
        /// </summary>
        PidTagEmailAddress = 0x3003,

        /// <summary>
        /// Contains a comment about the purpose or content of the Address Book object.
        /// </summary>
        PidTagComment = 0x3004,

        /// <summary>
        /// Specifies the number of nested categories in which a given row is contained.
        /// </summary>
        PidTagDepth = 0x3005,

        /// <summary>
        /// Contains the time, in UTC, that the object was created.
        /// </summary>
        PidTagCreationTime = 0x3007,

        /// <summary>
        /// Contains the time, in UTC, of the last modification to the object.
        /// </summary>
        PidTagLastModificationTime = 0x3008,

        /// <summary>
        /// Contains a unique binary-comparable key that identifies an object for a search.
        /// </summary>
        PidTagSearchKey = 0x300B,

        /// <summary>
        /// The OrigEntryId
        /// </summary>
        OrigEntryId = 0x300F,

        /// <summary>
        /// Contains the message ID of a Message object being submitted for optimization ([MS-OXOMSG] section 3.2.4.4).
        /// </summary>
        PidTagTargetEntryId = 0x3010,

        /// <summary>
        /// Contains a computed value derived from other conversation-related properties.
        /// </summary>
        PidTagConversationId = 0x3013,

        /// <summary>
        /// Indicates whether the GUID portion of the PidTagConversationIndex property (section 2.641) is to be used to compute the PidTagConversationId property (section 2.640).
        /// </summary>
        PidTagConversationIndexTracking = 0x3016,

        /// <summary>
        /// Specifies the GUID of an archive tag.
        /// </summary>
        PidTagArchiveTag = 0x3018,

        /// <summary>
        /// Specifies the GUID of a retention tag.
        /// </summary>
        PidTagPolicyTag = 0x3019,

        /// <summary>
        /// Specifies the number of days that a Message object can remain unarchived.
        /// </summary>
        PidTagRetentionPeriod = 0x301A,

        /// <summary>
        /// Contains the default retention period, and the start date from which the age of a Message object is calculated.
        /// </summary>
        PidTagStartDateEtc = 0x301B,

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
        PidTagArchivePeriod = 0x301E,

        /// <summary>
        /// Specifies the date, in UTC, after which a Message object is archived by the server.
        /// </summary>
        PidTagArchiveDate = 0x301F,

        /// <summary>
        /// Indicates whether string properties within the .msg file are Unicode-encoded.
        /// </summary>
        PidTagStoreSupportMask = 0x340D,

        /// <summary>
        /// Indicates whether a mailbox has any active Search folders.
        /// </summary>
        PidTagStoreState = 0x340E,

        /// <summary>
        /// Contains a bitmask of flags that describe capabilities of an address book container.
        /// </summary>
        PidTagContainerFlags = 0x3600,

        /// <summary>
        /// Specifies the type of a folder that includes the Root folder, Generic folder, and Search folder.
        /// </summary>
        PidTagFolderType = 0x3601,

        /// <summary>
        /// Specifies the number of rows under the header row.
        /// </summary>
        PidTagContentCount = 0x3602,

        /// <summary>
        /// Specifies the number of rows under the header row that have the PidTagRead property (section 2.869) set to FALSE.
        /// </summary>
        PidTagContentUnreadCount = 0x3603,

        /// <summary>
        /// This property is not set and, if set, is ignored.
        /// </summary>
        PidTagSelectable = 0x3609,

        /// <summary>
        /// Specifies whether a folder has subfolders.
        /// </summary>
        PidTagSubfolders = 0x360A,

        /// <summary>
        /// Contains a filter value used in ambiguous name resolution.
        /// </summary>
        PidTagAnr = 0x360C,

        /// <summary>
        /// Identifies all of the subfolders of the current folder.
        /// </summary>
        PidTagContainerHierarchy = 0x360E,

        /// <summary>
        /// Always empty. An NSPI server defines this value for distribution lists and it is not present for other objects.
        /// </summary>
        PidTagContainerContents = 0x360F,

        /// <summary>
        /// Identifies all FAI messages in the current folder.
        /// </summary>
        PidTagFolderAssociatedContents = 0x3610,

        /// <summary>
        /// Contains a string value that describes the type of Message object that a folder contains.
        /// </summary>
        PidTagContainerClass = 0x3613,

        /// <summary>
        /// Contains the EntryID of the Calendar folder.
        /// </summary>
        PidTagIpmAppointmentEntryId = 0x36D0,

        /// <summary>
        /// Contains the EntryID of the Contacts folder.
        /// </summary>
        PidTagIpmContactEntryId = 0x36D1,

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
        /// Contains an EntryID for the Reminders folder.
        /// </summary>
        PidTagRemindersOnlineEntryId = 0x36D5,

        /// <summary>
        /// Contains the EntryID of the Drafts folder.
        /// </summary>
        PidTagIpmDraftsEntryId = 0x36D7,

        /// <summary>
        /// Contains the indexed entry IDs for several special folders related to conflicts, sync issues, local failures, server failures, junk email and spam.
        /// </summary>
        PidTagAdditionalRenEntryIds = 0x36D8,

        /// <summary>
        /// Contains an array of blocks that specify the EntryIDs of several special folders.
        /// </summary>
        PidTagAdditionalRenEntryIdsEx = 0x36D9,

        /// <summary>
        /// Contains encoded sub-properties for a folder.
        /// </summary>
        PidTagExtendedFolderFlags = 0x36DA,

        /// <summary>
        /// Contains a positive number whose negative is less than or equal to the value of the PidLidTaskOrdinal property (section 2.327) of all of the Task objects in the folder.
        /// </summary>
        PidTagOrdinalMost = 0x36E2,

        /// <summary>
        /// Contains EntryIDs of the Delegate Information object, the free/busy message of the logged on user, and the folder with the PidTagDisplayName property (section 2.667) value of "Freebusy Data".
        /// </summary>
        PidTagFreeBusyEntryIds = 0x36E4,

        /// <summary>
        /// Contains the message class of the object.
        /// </summary>
        PidTagDefaultPostMessageClass = 0x36E5,

        /// <summary>
        /// Specifies the date and time, in UTC, until which the client expects to be actively editing the object.
        /// </summary>
        PidTagClientActivelyEditingUntil = 0x3700,

        /// <summary>
        /// Contains the binary representation of the Attachment object in an application-specific format.
        /// </summary>
        PidTagAttachDataObject = 0x3701,

        /// <summary>
        /// Contains the contents of the file to be attached.
        /// </summary>
        PidTagAttachDataBinary = 0x3701,

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
        /// Represents the way the contents of an attachment are accessed.
        /// </summary>
        PidTagAttachMethod = 0x3705,

        /// <summary>
        /// Contains the full filename and extension of the Attachment object.
        /// </summary>
        PidTagAttachLongFilename = 0x3707,

        /// <summary>
        /// Contains the 8.3 name of the PidTagAttachLongPathname property (section 2.587).
        /// </summary>
        PidTagAttachPathname = 0x3708,

        /// <summary>
        /// Contains a Windows Metafile, as specified in [MS-WMF], for the Attachment object.
        /// </summary>
        PidTagAttachRendering = 0x3709,

        /// <summary>
        /// Contains the identifier information for the application that supplied the Attachment object data.
        /// </summary>
        PidTagAttachTag = 0x370A,

        /// <summary>
        /// Represents an offset, in rendered characters, to use when rendering an attachment within the main message text.
        /// </summary>
        PidTagRenderingPosition = 0x370B,

        /// <summary>
        /// Contains the name of an attachment file, modified so that it can be correlated with TNEF messages.
        /// </summary>
        PidTagAttachTransportName = 0x370C,

        /// <summary>
        /// Contains the fully-qualified path and file name with extension.
        /// </summary>
        PidTagAttachLongPathname = 0x370D,

        /// <summary>
        /// Contains a content-type MIME header.
        /// </summary>
        PidTagAttachMimeTag = 0x370E,

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
        /// Indicates which body formats might reference this attachment when rendering data.
        /// </summary>
        PidTagAttachFlags = 0x3714,

        /// <summary>
        /// Contains the GUID of the software component that can display the contents of the message.
        /// </summary>
        PidTagAttachPayloadProviderGuidString = 0x3719,

        /// <summary>
        /// Contains the class name of an object that can display the contents of the message.
        /// </summary>
        PidTagAttachPayloadClass = 0x371A,

        /// <summary>
        /// Specifies the character set of an attachment received via MIME with the content-type of text.
        /// </summary>
        PidTagTextAttachmentCharset = 0x371B,

        /// <summary>
        /// Contains an integer value that indicates how to display an Address Book object in a table or as an addressee on a message.
        /// </summary>
        PidTagDisplayType = 0x3900,

        /// <summary>
        /// Contains the value of the PidTagEntryId property (section 2.674), expressed as a Permanent Entry ID format.
        /// </summary>
        PidTagTemplateid = 0x3902,

        /// <summary>
        /// Contains an integer value that indicates how to display an Address Book object in a table or as a recipient on a message.
        /// </summary>
        PidTagDisplayTypeEx = 0x3905,

        /// <summary>
        /// Contains the SMTP address of the Message object.
        /// </summary>
        PidTagSmtpAddress = 0x39FE,

        /// <summary>
        /// Contains the printable string version of the display name.
        /// </summary>
        PidTagAddressBookDisplayNamePrintable = 0x39FF,

        /// <summary>
        /// Contains the alias of an Address Book object, which is an alternative name by which the object can be identified.
        /// </summary>
        PidTagAccount = 0x3A00,

        /// <summary>
        /// Contains a telephone number to reach the mail user.
        /// </summary>
        PidTagCallbackTelephoneNumber = 0x3A02,

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
        /// Contains the primary telephone number of the mail user's place of business.
        /// </summary>
        PidTagBusinessTelephoneNumber = 0x3A08,

        /// <summary>
        /// Contains the primary telephone number of the mail user's home.
        /// </summary>
        PidTagHomeTelephoneNumber = 0x3A09,

        /// <summary>
        /// Contains the initials for parts of the full name of the mail user.
        /// </summary>
        PidTagInitials = 0x3A0A,

        /// <summary>
        /// Contains a keyword that identifies the mail user to the mail user's system administrator.
        /// </summary>
        PidTagKeyword = 0x3A0B,

        /// <summary>
        /// Contains a value that indicates the language in which the messaging user is writing messages.
        /// </summary>
        PidTagLanguage = 0x3A0C,

        /// <summary>
        /// Contains the location of the mail user.
        /// </summary>
        PidTagLocation = 0x3A0D,

        /// <summary>
        /// Contains the common name of a messaging user for use in a message header.
        /// </summary>
        PidTagMessageHandlingSystemCommonName = 0x3A0F,

        /// <summary>
        /// Contains an identifier for the mail user used within the mail user's organization.
        /// </summary>
        PidTagOrganizationalIdNumber = 0x3A10,

        /// <summary>
        /// Contains the mail user's family name.
        /// </summary>
        PidTagSurname = 0x3A11,

        /// <summary>
        /// Contains the original EntryID of an object.
        /// </summary>
        PidTagOriginalEntryId = 0x3A12,

        /// <summary>
        /// Contains the mail user's postal address.
        /// </summary>
        PidTagPostalAddress = 0x3A15,

        /// <summary>
        /// Contains the mail user's company name.
        /// </summary>
        PidTagCompanyName = 0x3A16,

        /// <summary>
        /// Contains the mail user's job title.
        /// </summary>
        PidTagTitle = 0x3A17,

        /// <summary>
        /// Contains a name for the department in which the mail user works.
        /// </summary>
        PidTagDepartmentName = 0x3A18,

        /// <summary>
        /// Contains the mail user's office location.
        /// </summary>
        PidTagOfficeLocation = 0x3A19,

        /// <summary>
        /// Contains the mail user's primary telephone number.
        /// </summary>
        PidTagPrimaryTelephoneNumber = 0x3A1A,

        /// <summary>
        /// Contains a secondary telephone number at the mail user's place of business.
        /// </summary>
        PidTagBusiness2TelephoneNumber = 0x3A1B,

        /// <summary>
        /// Contains secondary telephone numbers at the mail user's place of business.
        /// </summary>
        PidTagBusiness2TelephoneNumbers = 0x3A1B,

        /// <summary>
        /// Contains the mail user's cellular telephone number.
        /// </summary>
        PidTagMobileTelephoneNumber = 0x3A1C,

        /// <summary>
        /// Contains the mail user's radio telephone number.
        /// </summary>
        PidTagRadioTelephoneNumber = 0x3A1D,

        /// <summary>
        /// Contains the mail user's car telephone number.
        /// </summary>
        PidTagCarTelephoneNumber = 0x3A1E,

        /// <summary>
        /// Contains an alternate telephone number for the mail user.
        /// </summary>
        PidTagOtherTelephoneNumber = 0x3A1F,

        /// <summary>
        /// Contains an Address Book object's display name that is transmitted with the message.
        /// </summary>
        PidTagTransmittableDisplayName = 0x3A20,

        /// <summary>
        /// Contains the mail user's pager telephone number.
        /// </summary>
        PidTagPagerTelephoneNumber = 0x3A21,

        /// <summary>
        /// Contains an ASN.1 authentication certificate for a messaging user.
        /// </summary>
        PidTagUserCertificate = 0x3A22,

        /// <summary>
        /// Contains the telephone number of the mail user's primary fax machine.
        /// </summary>
        PidTagPrimaryFaxNumber = 0x3A23,

        /// <summary>
        /// Contains the telephone number of the mail user's business fax machine.
        /// </summary>
        PidTagBusinessFaxNumber = 0x3A24,

        /// <summary>
        /// Contains the telephone number of the mail user's home fax machine.
        /// </summary>
        PidTagHomeFaxNumber = 0x3A25,

        /// <summary>
        /// Contains the name of the mail user's country/region.
        /// </summary>
        PidTagCountry = 0x3A26,

        /// <summary>
        /// Contains the name of the mail user's locality, such as the town or city.
        /// </summary>
        PidTagLocality = 0x3A27,

        /// <summary>
        /// Contains the name of the mail user's state or province.
        /// </summary>
        PidTagStateOrProvince = 0x3A28,

        /// <summary>
        /// Contains the mail user's street address.
        /// </summary>
        PidTagStreetAddress = 0x3A29,

        /// <summary>
        /// Contains the postal code for the mail user's postal address.
        /// </summary>
        PidTagPostalCode = 0x3A2A,

        /// <summary>
        /// Contains the number or identifier of the mail user's post office box.
        /// </summary>
        PidTagPostOfficeBox = 0x3A2B,

        /// <summary>
        /// Contains the mail user's telex number. This property is returned from an NSPI server as a PtypMultipleBinary. Otherwise, the data type is PtypString.
        /// </summary>
        PidTagTelexNumber = 0x3A2C,

        /// <summary>
        /// Contains the Integrated Services Digital Network (ISDN) telephone number of the mail user.
        /// </summary>
        PidTagIsdnNumber = 0x3A2D,

        /// <summary>
        /// Contains the telephone number of the mail user's administrative assistant.
        /// </summary>
        PidTagAssistantTelephoneNumber = 0x3A2E,

        /// <summary>
        /// Contains a secondary telephone number at the mail user's home.
        /// </summary>
        PidTagHome2TelephoneNumber = 0x3A2F,

        /// <summary>
        /// Contains secondary telephone numbers at the mail user's home.
        /// </summary>
        PidTagHome2TelephoneNumbers = 0x3A2F,

        /// <summary>
        /// Contains the name of the mail user's administrative assistant.
        /// </summary>
        PidTagAssistant = 0x3A30,

        /// <summary>
        /// Indicates whether the email-enabled entity represented by the Address Book object can receive all message content, including Rich Text Format (RTF) and other embedded objects.
        /// </summary>
        PidTagSendRichInfo = 0x3A40,

        /// <summary>
        /// Contains the date of the mail user's wedding anniversary.
        /// </summary>
        PidTagWeddingAnniversary = 0x3A41,

        /// <summary>
        /// Contains the date of the mail user's birthday at midnight.
        /// </summary>
        PidTagBirthday = 0x3A42,

        /// <summary>
        /// Contains the names of the mail user's hobbies.
        /// </summary>
        PidTagHobbies = 0x3A43,

        /// <summary>
        /// Specifies the middle name(s) of the contact.
        /// </summary>
        PidTagMiddleName = 0x3A44,

        /// <summary>
        /// Contains the mail user's honorific title.
        /// </summary>
        PidTagDisplayNamePrefix = 0x3A45,

        /// <summary>
        /// Contains the name of the mail user's line of business.
        /// </summary>
        PidTagProfession = 0x3A46,

        /// <summary>
        /// Contains the name of the mail user's referral.
        /// </summary>
        PidTagReferredByName = 0x3A47,

        /// <summary>
        /// Contains the name of the mail user's spouse/partner.
        /// </summary>
        PidTagSpouseName = 0x3A48,

        /// <summary>
        /// Contains the name of the mail user's computer network.
        /// </summary>
        PidTagComputerNetworkName = 0x3A49,

        /// <summary>
        /// Contains the mail user's customer identification number.
        /// </summary>
        PidTagCustomerId = 0x3A4A,

        /// <summary>
        /// Contains the mail user's telecommunication device for the deaf (TTY/TDD) telephone number.
        /// </summary>
        PidTagTelecommunicationsDeviceForDeafTelephoneNumber = 0x3A4B,

        /// <summary>
        /// Contains the File Transfer Protocol (FTP) site address of the mail user.
        /// </summary>
        PidTagFtpSite = 0x3A4C,

        /// <summary>
        /// Contains a value that represents the mail user's gender.
        /// </summary>
        PidTagGender = 0x3A4D,

        /// <summary>
        /// Contains the name of the mail user's manager.
        /// </summary>
        PidTagManagerName = 0x3A4E,

        /// <summary>
        /// Contains the mail user's nickname.
        /// </summary>
        PidTagNickname = 0x3A4F,

        /// <summary>
        /// Contains the URL of the mail user's personal home page.
        /// </summary>
        PidTagPersonalHomePage = 0x3A50,

        /// <summary>
        /// Contains the URL of the mail user's business home page.
        /// </summary>
        PidTagBusinessHomePage = 0x3A51,

        /// <summary>
        /// Contains the main telephone number of the mail user's company.
        /// </summary>
        PidTagCompanyMainTelephoneNumber = 0x3A57,

        /// <summary>
        /// Specifies the names of the children of the contact.
        /// </summary>
        PidTagChildrensNames = 0x3A58,

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
        /// Contains the name of the mail user's home state or province.
        /// </summary>
        PidTagHomeAddressStateOrProvince = 0x3A5C,

        /// <summary>
        /// Contains the mail user's home street address.
        /// </summary>
        PidTagHomeAddressStreet = 0x3A5D,

        /// <summary>
        /// Contains the number or identifier of the mail user's home post office box.
        /// </summary>
        PidTagHomeAddressPostOfficeBox = 0x3A5E,

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
        /// Contains the name of the mail user's other state or province.
        /// </summary>
        PidTagOtherAddressStateOrProvince = 0x3A62,

        /// <summary>
        /// Contains the mail user's other street address.
        /// </summary>
        PidTagOtherAddressStreet = 0x3A63,

        /// <summary>
        /// Contains the number or identifier of the mail user's other post office box.
        /// </summary>
        PidTagOtherAddressPostOfficeBox = 0x3A64,

        /// <summary>
        /// Contains a list of certificates for the mail user.
        /// </summary>
        PidTagUserX509Certificate = 0x3A70,

        /// <summary>
        /// Contains a bitmask of message encoding preferences for email sent to an email-enabled entity that is represented by this Address Book object.
        /// </summary>
        PidTagSendInternetEncoding = 0x3A71,

        /// <summary>
        /// Indicates which page of a display template to display first.
        /// </summary>
        PidTagInitialDetailsPane = 0x3F08,

        /// <summary>
        /// Indicates the code page used for the PidTagBody property (section 2.609) or the PidTagBodyHtml property (section 2.612).
        /// </summary>
        PidTagInternetCodepage = 0x3FDE,

        /// <summary>
        /// Specifies whether a client or server application should forego sending automated replies in response to this message.
        /// </summary>
        PidTagAutoResponseSuppress = 0x3FDF,

        /// <summary>
        /// Contains a permissions list for a folder.
        /// </summary>
        PidTagAccessControlListData = 0x3FE0,

        /// <summary>
        /// Specifies whether the message was forwarded due to the triggering of a delegate forward rule.
        /// </summary>
        PidTagDelegatedByRule = 0x3FE3,

        /// <summary>
        /// Specifies how to resolve any conflicts with the message.
        /// </summary>
        PidTagResolveMethod = 0x3FE7,

        /// <summary>
        /// Indicates whether a Message object has a deferred action message associated with it.
        /// </summary>
        PidTagHasDeferredActionMessages = 0x3FEA,

        /// <summary>
        /// Contains a number used in the calculation of how long to defer sending a message.
        /// </summary>
        PidTagDeferredSendNumber = 0x3FEB,

        /// <summary>
        /// Specifies the unit of time used as a multiplier with the PidTagDeferredSendNumber property (section 2.654) value.
        /// </summary>
        PidTagDeferredSendUnits = 0x3FEC,

        /// <summary>
        /// Contains an integer value that is used along with the PidTagExpiryUnits property (section 2.681) to define the expiry send time.
        /// </summary>
        PidTagExpiryNumber = 0x3FED,

        /// <summary>
        /// Contains the unit of time that the value of the PidTagExpiryNumber property (section 2.679) multiplies.
        /// </summary>
        PidTagExpiryUnits = 0x3FEE,

        /// <summary>
        /// Contains the amount of time after which a client would like to defer sending the message.
        /// </summary>
        PidTagDeferredSendTime = 0x3FEF,

        /// <summary>
        /// Contains the EntryID of the conflict resolve message.
        /// </summary>
        PidTagConflictEntryId = 0x3FF0,

        /// <summary>
        /// Contains the Windows Locale ID of the end-user who created this message.
        /// </summary>
        PidTagMessageLocaleId = 0x3FF1,

        /// <summary>
        /// Contains the name of the creator of a Message object.
        /// </summary>
        PidTagCreatorName = 0x3FF8,

        /// <summary>
        /// Specifies the original author of the message according to their Address Book EntryID.
        /// </summary>
        PidTagCreatorEntryId = 0x3FF9,

        /// <summary>
        /// Contains the name of the last mail user to change the Message object.
        /// </summary>
        PidTagLastModifierName = 0x3FFA,

        /// <summary>
        /// Specifies the Address Book EntryID of the last user to modify the contents of the message.
        /// </summary>
        PidTagLastModifierEntryId = 0x3FFB,

        /// <summary>
        /// Specifies the code page used to encode the non-Unicode string properties on this Message object.
        /// </summary>
        PidTagMessageCodepage = 0x3FFD,

        // Cloned from MetaProperties for better parsing
        /// <summary>
        /// The MetaTagDnPrefix meta-property MUST be ignored when received
        /// </summary>
        MetaTagDnPrefix = 0x4008,

        /// <summary>
        /// The MetaTagEcWarning meta-property contains a warning that occurred when producing output for an element in context
        /// </summary>
        MetaTagEcWarning = 0x400f,

        /// <summary>
        /// The MetaTagNewFXFolder meta-property provides information about alternative replicas (1) for a public folder in context
        /// </summary>
        MetaTagNewFXFolder = 0x4011,

        /// <summary>
        /// The MetaTagFXDelProp meta-property represents a directive to a client to delete specific subobjects of the object in context
        /// </summary>
        MetaTagFXDelProp = 0x4016,

        /// <summary>
        /// The IDSETs contain Folder ID structures ([MS-OXCDATA] section 2.2.1.1) for hierarchy synchronization operations, or
        /// Message ID structures for content synchronization operations, that exist in the local replica of the client.
        /// 2.2.1.1.1 MetaTagIdsetGiven ICS State Property
        /// </summary>
        MetaTagIdsetGiven = 0x4017,

        /// <summary>
        /// The PidTagSentRepresentingFlags flag
        /// </summary>
        PidTagSentRepresentingFlags = 0x401A,

        /// <summary>
        /// The IDSETs contain the IDs of messages that got out of the synchronization scope since the last synchronization identified by the initial ICS state.
        /// 2.2.1.3.2 MetaTagIdsetNoLongerInScope Meta-Property
        /// </summary>
        MetaTagIdsetNoLongerInScope = 0x4021,

        /// <summary>
        /// Contains the address type of the end user to whom a read receipt is directed.
        /// </summary>
        PidTagReadReceiptAddressType = 0x4029,

        /// <summary>
        /// Contains the email address of the end user to whom a read receipt is directed.
        /// </summary>
        PidTagReadReceiptEmailAddress = 0x402A,

        /// <summary>
        /// Contains the display name for the end user to whom a read receipt is directed.
        /// </summary>
        PidTagReadReceiptName = 0x402B,

        /// <summary>
        /// The IDSETs contain IDs of messages that were marked as read (as specified by the PidTagMessageFlags property
        /// in [MS-OXCMSG] section 2.2.1.6) since the last synchronization, as identified by the initial ICS state.
        /// 2.2.1.3.4 MetaTagIdsetRead Meta-Property
        /// </summary>
        MetaTagIdsetRead = 0x402D,

        /// <summary>
        /// The IDSETs contain IDs of messages that were marked as unread (as specified by the PidTagMessageFlags property
        /// in [MS-OXCMSG] section 2.2.1.6) since the last synchronization, as identified by the initial ICS state.
        /// 2.2.1.3.5 MetaTagIdsetUnread Meta-Property
        /// </summary>
        MetaTagIdsetUnread = 0x402E,

        /// <summary>
        /// Indicates a confidence level that the message is spam.
        /// </summary>
        PidTagContentFilterSpamConfidenceLevel = 0x4076,

        /// <summary>
        /// Reports the results of a Sender-ID check.
        /// </summary>
        PidTagSenderIdStatus = 0x4079,

        /// <summary>
        /// The MetaTagIncrementalSyncMessagePartial meta-property specifies an index of a property group within a property
        /// group mapping currently in context
        /// </summary>
        MetaTagIncrementalSyncMessagePartial = 0x407a,

        /// <summary>
        /// The MetaTagIncrSyncGroupId meta-property specifies an identifier of a property group mapping
        /// </summary>
        MetaTagIncrSyncGroupId = 0x407c,

        /// <summary>
        /// Contains the domain responsible for transmitting the current message.
        /// </summary>
        PidTagPurportedSenderDomain = 0x4083,

        /// <summary>
        /// Indicates the encoding method and HTML inclusion for attachments.
        /// </summary>
        PidTagInternetMailOverrideFormat = 0x5902,

        /// <summary>
        /// Specifies the format that an email editor can use for editing the message body.
        /// </summary>
        PidTagMessageEditorFormat = 0x5909,

        /// <summary>
        /// Contains the SMTP email address format of the e–mail address of the sending mailbox owner.
        /// </summary>
        PidTagSenderSmtpAddress = 0x5D01,

        /// <summary>
        /// Contains the SMTP email address of the end user who is represented by the sending mailbox owner.
        /// </summary>
        PidTagSentRepresentingSmtpAddress = 0x5D02,

        /// <summary>
        /// Contains the SMTP email address of the user to whom a read receipt is directed.
        /// </summary>
        PidTagReadReceiptSmtpAddress = 0x5D05,

        /// <summary>
        /// Contains the email message receiver's SMTP email address.
        /// </summary>
        PidTagReceivedBySmtpAddress = 0x5D07,

        /// <summary>
        /// Contains the SMTP email address of the user represented by the receiving mailbox owner.
        /// </summary>
        PidTagReceivedRepresentingSmtpAddress = 0x5D08,

        /// <summary>
        /// Specifies the location of the current recipient in the recipient table.
        /// </summary>
        PidTagRecipientOrder = 0x5FDF,

        /// <summary>
        /// Indicates that the attendee proposed a new date and/or time.
        /// </summary>
        PidTagRecipientProposed = 0x5FE1,

        /// <summary>
        /// Indicates the meeting start time requested by the attendee in a counter proposal.
        /// </summary>
        PidTagRecipientProposedStartTime = 0x5FE3,

        /// <summary>
        /// Indicates the meeting end time requested by the attendee in a counter proposal.
        /// </summary>
        PidTagRecipientProposedEndTime = 0x5FE4,

        /// <summary>
        /// Specifies the display name of the recipient.
        /// </summary>
        PidTagRecipientDisplayName = 0x5FF6,

        /// <summary>
        /// Identifies an Address Book object that specifies the recipient.
        /// </summary>
        PidTagRecipientEntryId = 0x5FF7,

        /// <summary>
        /// Indicates the date and time at which the attendee responded.
        /// </summary>
        PidTagRecipientTrackStatusTime = 0x5FFB,

        /// <summary>
        /// Specifies a bit field that describes the recipient status.
        /// </summary>
        PidTagRecipientFlags = 0x5FFD,

        /// <summary>
        /// Indicates the response status that is returned by the attendee.
        /// </summary>
        PidTagRecipientTrackStatus = 0x5FFF,

        /// <summary>
        /// Indicates whether email addresses of the contacts in the Contacts folder are treated in a special way with respect to the spam filter.
        /// </summary>
        PidTagJunkIncludeContacts = 0x6100,

        /// <summary>
        /// Indicates how aggressively incoming email is to be sent to the Junk Email folder.
        /// </summary>
        PidTagJunkThreshold = 0x6101,

        /// <summary>
        /// Indicates whether messages identified as spam can be permanently deleted.
        /// </summary>
        PidTagJunkPermanentlyDelete = 0x6102,

        /// <summary>
        /// Indicates whether email recipients are to be added to the safe senders list.
        /// </summary>
        PidTagJunkAddRecipientsToSafeSendersList = 0x6103,

        /// <summary>
        /// Indicated whether the phishing stamp on a message is to be ignored.
        /// </summary>
        PidTagJunkPhishingEnableLinks = 0x6107,

        /// <summary>
        /// Contains the top-level MIME message headers, all MIME message body part headers, and body part content that is not already converted to Message object properties, including attachments.
        /// </summary>
        PidTagMimeSkeleton = 0x64F0,

        /// <summary>
        /// Contains the value of the GUID that points to a Reply template.
        /// </summary>
        PidTagReplyTemplateId = 0x65C2,

        /// <summary>
        /// Contains a bitmask of flags indicating details about a message submission.
        /// </summary>
        PidTagSecureSubmitFlags = 0x65C6,

        /// <summary>
        /// Contains a value that contains an internal global identifier (GID) for this folder or message.
        /// </summary>
        PidTagSourceKey = 0x65E0,

        /// <summary>
        /// Contains a value on a folder that contains the PidTagSourceKey property (section 2.1012) of the parent folder.
        /// </summary>
        PidTagParentSourceKey = 0x65E1,

        /// <summary>
        /// Contains a structure that identifies the last change to the object.
        /// </summary>
        PidTagChangeKey = 0x65E2,

        /// <summary>
        /// Contains a value that contains a serialized representation of a PredecessorChangeList structure.
        /// </summary>
        PidTagPredecessorChangeList = 0x65E3,

        /// <summary>
        /// Contains flags that specify the state of the rule. Set on the FAI message.
        /// </summary>
        PidTagRuleMessageState = 0x65E9,

        /// <summary>
        /// Contains an opaque property that the client sets for the exclusive use of the client. Set on the FAI message.
        /// </summary>
        PidTagRuleMessageUserFlags = 0x65EA,

        /// <summary>
        /// Identifies the client application that owns the rule. Set on the FAI message.
        /// </summary>
        PidTagRuleMessageProvider = 0x65EB,

        /// <summary>
        /// Specifies the name of the rule. Set on the FAI message.
        /// </summary>
        PidTagRuleMessageName = 0x65EC,

        /// <summary>
        /// Contains 0x00000000. Set on the FAI message.
        /// </summary>
        PidTagRuleMessageLevel = 0x65ED,

        /// <summary>
        /// Contains opaque data set by the client for the exclusive use of the client. Set on the FAI message.
        /// </summary>
        PidTagRuleMessageProviderData = 0x65EE,

        /// <summary>
        /// Contains a value used to determine the order in which rules are evaluated and executed. Set on the FAI message.
        /// </summary>
        PidTagRuleMessageSequence = 0x65F3,

        /// <summary>
        /// Address book EntryID of the user logged on to the public folders.
        /// </summary>
        PidTagUserEntryId = 0x6619,

        /// <summary>
        /// Contains the EntryID in the Global Address List (GAL) of the owner of the mailbox.
        /// </summary>
        PidTagMailboxOwnerEntryId = 0x661B,

        /// <summary>
        /// Contains the display name of the owner of the mailbox.
        /// </summary>
        PidTagMailboxOwnerName = 0x661C,

        /// <summary>
        /// Indicates whether the user is OOF.
        /// </summary>
        PidTagOutOfOfficeState = 0x661D,

        /// <summary>
        /// Contains the EntryID of the folder named "SCHEDULE+ FREE BUSY" under the non-IPM subtree of the public folder message store.
        /// </summary>
        PidTagSchedulePlusFreeBusyEntryId = 0x6622,

        /// <summary>
        /// Specifies a user's folder permissions.
        /// </summary>
        PidTagRights = 0x6639,

        /// <summary>
        /// Indicates whether a Folder object has rules.
        /// </summary>
        PidTagHasRules = 0x663A,

        /// <summary>
        /// Contains the name-service EntryID of a directory object that refers to a public folder.
        /// </summary>
        PidTagAddressBookEntryId = 0x663B,

        /// <summary>
        /// Contains a number that monotonically increases every time a subfolder is added to, or deleted from, this folder.
        /// </summary>
        PidTagHierarchyChangeNumber = 0x663E,

        /// <summary>
        /// Specifies the actions the client is required to take on the message.
        /// </summary>
        PidTagClientActions = 0x6645,

        /// <summary>
        /// Contains the EntryID of the delivered message that the client has to process.
        /// </summary>
        PidTagDamOriginalEntryId = 0x6646,

        /// <summary>
        /// Indicates whether the Deferred Action Message (DAM) was updated by the server.
        /// </summary>
        PidTagDamBackPatched = 0x6647,

        /// <summary>
        /// Contains the error code that indicates the cause of an error encountered during the execution of the rule.
        /// </summary>
        PidTagRuleError = 0x6648,

        /// <summary>
        /// Contains the ActionType field ([MS-OXORULE] section 2.2.5.1) of a rule that failed.
        /// </summary>
        PidTagRuleActionType = 0x6649,

        /// <summary>
        /// Indicates whether the Message object has a named property.
        /// </summary>
        PidTagHasNamedProperties = 0x664A,

        /// <summary>
        /// Contains the index of a rule action that failed.
        /// </summary>
        PidTagRuleActionNumber = 0x6650,

        /// <summary>
        /// Contains the EntryID of the folder where the rule that triggered the generation of a DAM is stored.
        /// </summary>
        PidTagRuleFolderEntryId = 0x6651,

        /// <summary>
        /// Maximum size, in kilobytes, that a user is allowed to accumulate in their mailbox before no further email will be delivered to their mailbox.
        /// </summary>
        PidTagProhibitReceiveQuota = 0x666A,

        /// <summary>
        /// Specifies whether the attachment represents an alternate replica.
        /// </summary>
        PidTagInConflict = 0x666C,

        /// <summary>
        /// Maximum size, in kilobytes, of a message that a user is allowed to submit for transmission to another user.
        /// </summary>
        PidTagMaximumSubmitMessageSize = 0x666D,

        /// <summary>
        /// Maximum size, in kilobytes, that a user is allowed to accumulate in their mailbox before the user can no longer send any more email.
        /// </summary>
        PidTagProhibitSendQuota = 0x666E,

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
        /// Specifies a unique identifier that is generated by the messaging server for each rule when the rule is first created.
        /// </summary>
        PidTagRuleId = 0x6674,

        /// <summary>
        /// Contains a buffer that is obtained by concatenating the PidTagRuleId property (section 2.940) values from all of the rules contributing actions that are contained in the PidTagClientActions property (section 2.625).
        /// </summary>
        PidTagRuleIds = 0x6675,

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
        /// Defines the conditions under which a rule action is to be executed.
        /// </summary>
        PidTagRuleCondition = 0x6679,

        /// <summary>
        /// Contains the set of actions associated with the rule.
        /// </summary>
        PidTagRuleActions = 0x6680,

        /// <summary>
        /// A string identifying the client application that owns a rule.
        /// </summary>
        PidTagRuleProvider = 0x6681,

        /// <summary>
        /// Specifies the name of the rule.
        /// </summary>
        PidTagRuleName = 0x6682,

        /// <summary>
        /// Contains 0x00000000. This property is not used.
        /// </summary>
        PidTagRuleLevel = 0x6683,

        /// <summary>
        /// Contains opaque data set by the client for the exclusive use of the client.
        /// </summary>
        PidTagRuleProviderData = 0x6684,

        /// <summary>
        /// Specifies the time, in UTC, when the item or folder was soft deleted.
        /// </summary>
        PidTagDeletedOn = 0x668F,

        /// <summary>
        /// Contains the Logon object LocaleID.
        /// </summary>
        PidTagLocaleId = 0x66A1,

        /// <summary>
        /// Contains the identifier for the client code page used for Unicode to double-byte character set (DBCS) string conversion.
        /// </summary>
        PidTagCodePageId = 0x66C3,

        /// <summary>
        /// Contains information for use in display templates for distribution lists.
        /// </summary>
        PidTagAddressBookManageDistributionList = 0x6704,

        /// <summary>
        /// Contains the locale identifier.
        /// </summary>
        PidTagSortLocaleId = 0x6705,

        /// <summary>
        /// Specifies the time, in UTC, that a Message object or Folder object was last changed.
        /// </summary>
        PidTagLocalCommitTime = 0x6709,

        /// <summary>
        /// Contains the time of the most recent message change within the folder container, excluding messages changed within subfolders.
        /// </summary>
        PidTagLocalCommitTimeMax = 0x670A,

        /// <summary>
        /// Contains the total count of messages that have been deleted from a folder, excluding messages deleted within subfolders.
        /// </summary>
        PidTagDeletedCountTotal = 0x670B,

        /// <summary>
        /// Contains a unique identifier for an item across the message store.
        /// </summary>
        PidTagFlatUrlName = 0x670E,

        /// <summary>
        /// Contains an EntryID that represents the Sent Items folder for the message.
        /// </summary>
        PidTagSentMailSvrEID = 0x6740,

        /// <summary>
        /// Contains the server EntryID for the DAM.
        /// </summary>
        PidTagDeferredActionMessageOriginalEntryId = 0x6741,

        /// <summary>
        /// Contains the Folder ID (FID) ([MS-OXCDATA] section 2.2.1.1) of the folder.
        /// </summary>
        PidTagFolderId = 0x6748,

        /// <summary>
        /// Contains a value that contains the Folder ID (FID), as specified in [MS-OXCDATA] section 2.2.1.1, that identifies the parent folder of the messaging object being synchronized.
        /// </summary>
        PidTagParentFolderId = 0x6749,

        /// <summary>
        /// Contains a value that contains the MID of the message currently being synchronized.
        /// </summary>
        PidTagMid = 0x674A,

        /// <summary>
        /// Contains an identifier for all instances of a row in the table.
        /// </summary>
        PidTagInstID = 0x674D,

        /// <summary>
        /// Contains an identifier for a single instance of a row in the table.
        /// </summary>
        PidTagInstanceNum = 0x674E,

        /// <summary>
        /// Contains the Short-term Message ID (MID) ([MS-OXCDATA] section 2.2.1.2) of the first message in the local site's offline address book public folder.
        /// </summary>
        PidTagAddressBookMessageId = 0x674F,

        /// <summary>
        /// The IDSETs contain IDs of expired Message objects in a public folder that expired since the last synchronization identified by the initial ICS state.
        /// 2.2.1.3.3 MetaTagIdsetExpired Meta-Property
        /// </summary>
        MetaTagIdsetExpired = 0x6793,

        /// <summary>
        /// The CN structures, as specified in section 2.2.2.1, in the CNSET track changes to folders (for hierarchy synchronization
        /// operations) or normal messages (for content synchronization operations) in the current synchronization scope that have
        /// been previously communicated to a client, and are reflected in its local replica.
        /// 2.2.1.1.2 MetaTagCnsetSeen ICS State Property
        /// </summary>
        MetaTagCnsetSeen = 0x6796,

        /// <summary>
        /// Contains a structure that identifies the last change to the message or folder that is currently being synchronized.
        /// </summary>
        PidTagChangeNumber = 0x67A4,

        /// <summary>
        /// Specifies whether the message being synchronized is an FAI message.
        /// </summary>
        PidTagAssociated = 0x67AA,

        /// <summary>
        /// The CN structures, as specified in section 2.2.2.1, in the CNSET track changes to the read state for messages in
        /// the current synchronization scope that have been previously communicated to the client and are reflected in its
        /// local replica.
        /// 2.2.1.1.4 MetaTagCnsetRead ICS State Property
        /// </summary>
        MetaTagCnsetRead = 0x67D2,

        /// <summary>
        /// The semantics of this property are identical to the MetaTagCnsetSeen property (section 2.2.1.1.2), except that this
        /// property contains IDs for folder associated information (FAI) messages and is therefore only used in content
        /// synchronization operations.
        /// 2.2.1.1.3 MetaTagCnsetSeenFAI ICS State Property
        /// </summary>
        MetaTagCnsetSeenFAI = 0x67DA,

        /// <summary>
        /// The IDSETs contain the IDs of folders (for hierarchy synchronization operations) or messages (for content
        /// synchronization operations) that were hard deleted or soft deleted since the last synchronization identified
        /// by the initial ICS state.
        /// 2.2.1.3.1 MetaTagIdsetDeleted Meta-Property
        /// </summary>
        MetaTagIdsetDeleted = 0x67E5,

        /// <summary>
        /// Contains the display name of the address list.
        /// </summary>
        PidTagOfflineAddressBookName = 0x6800,

        /// <summary>
        /// Contains the sequence number of the OAB.
        /// </summary>
        PidTagOfflineAddressBookSequence = 0x6801,

        /// <summary>
        /// A string-formatted GUID that represents the address list container object.
        /// </summary>
        PidTagOfflineAddressBookContainerGuid = 0x6802,

        /// <summary>
        /// Contains additional rule data about the Rule FAI message.
        /// </summary>
        PidTagRwRulesStream = 0x6802,

        /// <summary>
        /// Contains the telephone number of the caller associated with a voice mail message.
        /// </summary>
        PidTagSenderTelephoneNumber = 0x6802,

        /// <summary>
        /// Contains the message class for full OAB messages.
        /// </summary>
        PidTagOfflineAddressBookMessageClass = 0x6803,

        /// <summary>
        /// Specifies the name of the caller who left the attached voice message, as provided by the voice network's caller ID system.
        /// </summary>
        PidTagVoiceMessageSenderName = 0x6803,

        /// <summary>
        /// Contains the DN of the address list that is contained in the OAB message.
        /// </summary>
        PidTagOfflineAddressBookDistinguishedName = 0x6804,

        /// <summary>
        /// Contains the number of pages in a Fax object.
        /// </summary>
        PidTagFaxNumberOfPages = 0x6804,

        /// <summary>
        /// Contains a list of file names for the audio file attachments that are to be played as part of a message.
        /// </summary>
        PidTagVoiceMessageAttachmentOrder = 0x6805,

        /// <summary>
        /// Contains a list of the property tags that have been truncated or limited by the server.
        /// </summary>
        PidTagOfflineAddressBookTruncatedProperties = 0x6805,

        /// <summary>
        /// Contains a unique identifier associated with the phone call.
        /// </summary>
        PidTagCallId = 0x6806,

        /// <summary>
        /// Contains the value of the Reporting-MTA field for a delivery status notification, as specified in [RFC3464].
        /// </summary>
        PidTagReportingMessageTransferAgent = 0x6820,

        /// <summary>
        /// Contains the last time, in UTC, that the folder was accessed.
        /// </summary>
        PidTagSearchFolderLastUsed = 0x6834,

        /// <summary>
        /// Contains the time, in UTC, at which the search folder container will be stale and has to be updated or recreated.
        /// </summary>
        PidTagSearchFolderExpiration = 0x683A,

        /// <summary>
        /// Set to 0x00000000 when sending and is ignored on receipt.
        /// </summary>
        PidTagScheduleInfoResourceType = 0x6841,

        /// <summary>
        /// Contains a GUID that identifies the search folder.
        /// </summary>
        PidTagSearchFolderId = 0x6842,

        /// <summary>
        /// Indicates whether the delegator wants to receive copies of the meeting-related objects that are sent to the delegate.
        /// </summary>
        PidTagScheduleInfoDelegatorWantsCopy = 0x6842,

        /// <summary>
        /// Contains a value set to TRUE by the client, regardless of user input.
        /// </summary>
        PidTagScheduleInfoDontMailDelegates = 0x6843,

        /// <summary>
        /// Specifies the names of the delegates.
        /// </summary>
        PidTagScheduleInfoDelegateNames = 0x6844,

        /// <summary>
        /// This property is not to be used.
        /// </summary>
        PidTagSearchFolderRecreateInfo = 0x6844,

        /// <summary>
        /// Specifies the EntryIDs of the delegates.
        /// </summary>
        PidTagScheduleInfoDelegateEntryIds = 0x6845,

        /// <summary>
        /// Specifies the search criteria and search options.
        /// </summary>
        PidTagSearchFolderDefinition = 0x6845,

        /// <summary>
        /// Contains flags that specify the binary large object (BLOB) data that appears in the PidTagSearchFolderDefinition (section 2.979) property.
        /// </summary>
        PidTagSearchFolderStorageType = 0x6846,

        /// <summary>
        /// This property is deprecated and SHOULD NOT be used.
        /// </summary>
        PidTagGatewayNeedsToRefresh = 0x6846,

        /// <summary>
        /// Specifies the start time, in UTC, of the publishing range.
        /// </summary>
        PidTagFreeBusyPublishStart = 0x6847,

        /// <summary>
        /// Specifies the end time, in UTC, of the publishing range.
        /// </summary>
        PidTagFreeBusyPublishEnd = 0x6848,

        /// <summary>
        /// Specifies the email address of the user or resource to whom this free/busy message applies.
        /// </summary>
        PidTagFreeBusyMessageEmailAddress = 0x6849,

        /// <summary>
        /// Specifies the type of navigation shortcut.
        /// </summary>
        PidTagWlinkType = 0x6849,

        /// <summary>
        /// Specifies conditions associated with the shortcut.
        /// </summary>
        PidTagWlinkFlags = 0x684A,

        /// <summary>
        /// Specifies the names of the delegates in Unicode.
        /// </summary>
        PidTagScheduleInfoDelegateNamesW = 0x684A,

        /// <summary>
        /// Indicates whether the delegator wants to receive informational updates.
        /// </summary>
        PidTagScheduleInfoDelegatorWantsInfo = 0x684B,

        /// <summary>
        /// Specifies a variable-length binary property to be used to sort shortcuts lexicographically.
        /// </summary>
        PidTagWlinkOrdinal = 0x684B,

        /// <summary>
        /// Specifies the EntryID of the folder pointed to by the shortcut.
        /// </summary>
        PidTagWlinkEntryId = 0x684C,

        /// <summary>
        /// Specifies the value of PidTagRecordKey property (section 2.901) of the folder pointed to by the shortcut.
        /// </summary>
        PidTagWlinkRecordKey = 0x684D,

        /// <summary>
        /// Specifies the value of the PidTagStoreEntryId property (section 2.1018) of the folder pointed to by the shortcut.
        /// </summary>
        PidTagWlinkStoreEntryId = 0x684E,

        /// <summary>
        /// Specifies the months for which free/busy data of type busy or OOF is present in the free/busy message.
        /// </summary>
        PidTagScheduleInfoMonthsMerged = 0x684F,

        /// <summary>
        /// Specifies the type of folder pointed to by the shortcut.
        /// </summary>
        PidTagWlinkFolderType = 0x684F,

        /// <summary>
        /// Specifies the blocks for which free/busy data of type busy or OOF is present in the free/busy message.
        /// </summary>
        PidTagScheduleInfoFreeBusyMerged = 0x6850,

        /// <summary>
        /// Specifies the value of the PidTagWlinkGroupHeaderID property (section 2.1060) of the group header associated with the shortcut.
        /// </summary>
        PidTagWlinkGroupClsid = 0x6850,

        /// <summary>
        /// Specifies the months for which free/busy data of type tentative is present in the free/busy message.
        /// </summary>
        PidTagScheduleInfoMonthsTentative = 0x6851,

        /// <summary>
        /// Specifies the value of the PidTagNormalizedSubject (section 2.803) of the group header associated with the shortcut.
        /// </summary>
        PidTagWlinkGroupName = 0x6851,

        /// <summary>
        /// Specifies the blocks of times for which the free/busy status is set to a value of tentative.
        /// </summary>
        PidTagScheduleInfoFreeBusyTentative = 0x6852,

        /// <summary>
        /// Specifies the section where the shortcut should be grouped.
        /// </summary>
        PidTagWlinkSection = 0x6852,

        /// <summary>
        /// Specifies the background color of the calendar.
        /// </summary>
        PidTagWlinkCalendarColor = 0x6853,

        /// <summary>
        /// Specifies the months for which free/busy data of type busy is present in the free/busy message.
        /// </summary>
        PidTagScheduleInfoMonthsBusy = 0x6853,

        /// <summary>
        /// Specifies the blocks of time for which the free/busy status is set to a value of busy.
        /// </summary>
        PidTagScheduleInfoFreeBusyBusy = 0x6854,

        /// <summary>
        /// Specifies the value of the PidTagEntryId property (section 2.674) of the user to whom the folder belongs.
        /// </summary>
        PidTagWlinkAddressBookEID = 0x6854,

        /// <summary>
        /// Specifies the months for which free/busy data of type OOF is present in the free/busy message.
        /// </summary>
        PidTagScheduleInfoMonthsAway = 0x6855,

        /// <summary>
        /// Specifies the times for which the free/busy status is set a value of OOF.
        /// </summary>
        PidTagScheduleInfoFreeBusyAway = 0x6856,

        /// <summary>
        /// Specifies the time, in UTC, that the data was published.
        /// </summary>
        PidTagFreeBusyRangeTimestamp = 0x6868,

        /// <summary>
        /// Contains an integer value used to calculate the start and end dates of the range of free/busy data to be published to the public folders.
        /// </summary>
        PidTagFreeBusyCountMonths = 0x6869,

        /// <summary>
        /// Contains a list of tombstones, where each tombstone represents a Meeting object that has been declined.
        /// </summary>
        PidTagScheduleInfoAppointmentTombstone = 0x686A,

        /// <summary>
        /// Indicates whether delegates can view Message objects that are marked as private.
        /// </summary>
        PidTagDelegateFlags = 0x686B,

        /// <summary>
        /// This property is deprecated and is not to be used.
        /// </summary>
        PidTagScheduleInfoFreeBusy = 0x686C,

        /// <summary>
        /// Indicates whether a client or server is to automatically respond to all meeting requests for the attendee or resource.
        /// </summary>
        PidTagScheduleInfoAutoAcceptAppointments = 0x686D,

        /// <summary>
        /// Indicates whether a client or server, when automatically responding to meeting requests, is to decline Meeting Request objects that represent a recurring series.
        /// </summary>
        PidTagScheduleInfoDisallowRecurringAppts = 0x686E,

        /// <summary>
        /// Indicates whether a client or server, when automatically responding to meeting requests, is to decline Meeting Request objects that overlap with previously scheduled events.
        /// </summary>
        PidTagScheduleInfoDisallowOverlappingAppts = 0x686F,

        /// <summary>
        /// Specifies the Client ID that allows the client to determine whether the shortcut was created on the current machine/user via an equality test.
        /// </summary>
        PidTagWlinkClientID = 0x6890,

        /// <summary>
        /// Specifies the value of the PidTagStoreEntryId property (section 2.1018) of the current user (not the owner of the folder).
        /// </summary>
        PidTagWlinkAddressBookStoreEID = 0x6891,

        /// <summary>
        /// Specifies the type of group header.
        /// </summary>
        PidTagWlinkROGroupType = 0x6892,

        /// <summary>
        /// Contains view definitions.
        /// </summary>
        PidTagViewDescriptorBinary = 0x7001,

        /// <summary>
        /// Contains view definitions in string format.
        /// </summary>
        PidTagViewDescriptorStrings = 0x7002,

        /// <summary>
        /// The PidTagViewDescriptorName
        /// </summary>
        PidTagViewDescriptorName = 0x7006,

        /// <summary>
        /// Contains the View Descriptor version.
        /// </summary>
        PidTagViewDescriptorVersion = 0x7007,

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
        /// Specifies whether contact synchronization with an external source is handled by the server.
        /// </summary>
        PidTagOscSyncEnabled = 0x7C24,

        /// <summary>
        /// Indicates whether a client has already processed a received task communication.
        /// </summary>
        PidTagProcessed = 0x7D01,

        /// <summary>
        /// Indicates the original date and time, in UTC, at which the instance in the recurrence pattern would have occurred if it were not an exception.
        /// </summary>
        PidTagExceptionReplaceTime = 0x7FF9,

        /// <summary>
        /// Contains the type of Message object to which an attachment is linked.
        /// </summary>
        PidTagAttachmentLinkId = 0x7FFA,

        /// <summary>
        /// Contains the start date and time of the exception in the local time zone of the computer when the exception is created.
        /// </summary>
        PidTagExceptionStartTime = 0x7FFB,

        /// <summary>
        /// Contains the end date and time of the exception in the local time zone of the computer when the exception is created.
        /// </summary>
        PidTagExceptionEndTime = 0x7FFC,

        /// <summary>
        /// Indicates special handling for an Attachment object.
        /// </summary>
        PidTagAttachmentFlags = 0x7FFD,

        /// <summary>
        /// Indicates whether an Attachment object is hidden from the end user.
        /// </summary>
        PidTagAttachmentHidden = 0x7FFE,

        /// <summary>
        /// Indicates that a contact photo attachment is attached to a Contact object.
        /// </summary>
        PidTagAttachmentContactPhoto = 0x7FFF,

        /// <summary>
        /// This property is deprecated and is to be ignored.
        /// </summary>
        PidTagAddressBookFolderPathname = 0x8004,

        /// <summary>
        /// Contains one row that references the mail user's manager.
        /// </summary>
        PidTagAddressBookManager = 0x8005,

        /// <summary>
        /// Contains the DN of the mail user's manager.
        /// </summary>
        PidTagAddressBookManagerDistinguishedName = 0x8005,

        /// <summary>
        /// Contains the DN expressed in the X500 DN format. This property is returned from a name service provider interface (NSPI) server as a PtypEmbeddedTable. Otherwise, the data type is PtypString8.
        /// </summary>
        PidTagAddressBookHomeMessageDatabase = 0x8006,

        /// <summary>
        /// Lists all of the distribution lists for which the object is a member. This property is returned from an NSPI server as a PtypEmbeddedTable. Otherwise, the data type is PtypString8.
        /// </summary>
        PidTagAddressBookIsMemberOfDistributionList = 0x8008,

        /// <summary>
        /// Contains the members of the distribution list.
        /// </summary>
        PidTagAddressBookMember = 0x8009,

        /// <summary>
        /// Contains one row that references the distribution list's owner.
        /// </summary>
        PidTagAddressBookOwner = 0x800C,

        /// <summary>
        /// Lists all of the mail user’s direct reports.
        /// </summary>
        PidTagAddressBookReports = 0x800E,

        /// <summary>
        /// Contains alternate email addresses for the Address Book object.
        /// </summary>
        PidTagAddressBookProxyAddresses = 0x800F,

        /// <summary>
        /// Contains the foreign system email address of an Address Book object.
        /// </summary>
        PidTagAddressBookTargetAddress = 0x8011,

        /// <summary>
        /// Contains a list of mail users who are allowed to send email on behalf of the mailbox owner.
        /// </summary>
        PidTagAddressBookPublicDelegates = 0x8015,

        /// <summary>
        /// Contains a list of the distribution lists owned by a mail user.
        /// </summary>
        PidTagAddressBookOwnerBackLink = 0x8024,

        /// <summary>
        /// Contains custom values defined and populated by the organization that modified the display templates.
        /// </summary>
        PidTagAddressBookExtensionAttribute1 = 0x802D,

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
        /// Contains custom values defined and populated by the organization that modified the display templates.
        /// </summary>
        PidTagAddressBookExtensionAttribute10 = 0x8036,

        /// <summary>
        /// Contains the DN of the Address Book object.
        /// </summary>
        PidTagAddressBookObjectDistinguishedName = 0x803C,

        /// <summary>
        /// Specifies the maximum size, in bytes, of a message that a recipient can receive.
        /// </summary>
        PidTagAddressBookDeliveryContentLength = 0x806A,

        /// <summary>
        /// Indicates that delivery restrictions exist for a recipient.
        /// </summary>
        PidTagAddressBookDistributionListMemberSubmitAccepted = 0x8073,

        /// <summary>
        /// Contains a list of names by which a server is known to the various transports in use by the network.
        /// </summary>
        PidTagAddressBookNetworkAddress = 0x8170,

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
        /// Contains the ASN_1 DER encoded X.509 certificates for the mail user.
        /// </summary>
        PidTagAddressBookX509Certificate = 0x8C6A,

        /// <summary>
        /// Contains a GUID that identifies an Address Book object.
        /// </summary>
        PidTagAddressBookObjectGuid = 0x8C6D,

        /// <summary>
        /// Contains the phonetic representation of the PidTagGivenName property (section 2.705).
        /// </summary>
        PidTagAddressBookPhoneticGivenName = 0x8C8E,

        /// <summary>
        /// Contains the phonetic representation of the PidTagSurname property (section 2.1026).
        /// </summary>
        PidTagAddressBookPhoneticSurname = 0x8C8F,

        /// <summary>
        /// Contains the phonetic representation of the PidTagDepartmentName property (section 2.663).
        /// </summary>
        PidTagAddressBookPhoneticDepartmentName = 0x8C90,

        /// <summary>
        /// Contains the phonetic representation of the PidTagCompanyName property (section 2.630).
        /// </summary>
        PidTagAddressBookPhoneticCompanyName = 0x8C91,

        /// <summary>
        /// Contains the phonetic representation of the PidTagDisplayName property (section 2.667).
        /// </summary>
        PidTagAddressBookPhoneticDisplayName = 0x8C92,

        /// <summary>
        /// Contains a value that indicates how to display an Address Book object in a table or as a recipient on a message.
        /// </summary>
        PidTagAddressBookDisplayTypeExtended = 0x8C93,

        /// <summary>
        /// Lists all Department objects of which the mail user is a member.
        /// </summary>
        PidTagAddressBookHierarchicalShowInDepartments = 0x8C94,

        /// <summary>
        /// Contains a list of DNs that represent the address book containers that hold Resource objects, such as conference rooms and equipment.
        /// </summary>
        PidTagAddressBookRoomContainers = 0x8C96,

        /// <summary>
        /// Contains all of the mail users that belong to this department.
        /// </summary>
        PidTagAddressBookHierarchicalDepartmentMembers = 0x8C97,

        /// <summary>
        /// Contains the distinguished name (DN) of either the root Department object or the root departmental group in the department hierarchy for the organization.
        /// </summary>
        PidTagAddressBookHierarchicalRootDepartment = 0x8C98,

        /// <summary>
        /// Contains all of the departments to which this department is a child.
        /// </summary>
        PidTagAddressBookHierarchicalParentDepartment = 0x8C99,

        /// <summary>
        /// Contains the child departments in a hierarchy of departments.
        /// </summary>
        PidTagAddressBookHierarchicalChildDepartments = 0x8C9A,

        /// <summary>
        /// Contains the mail user's photo in .jpg format.
        /// </summary>
        PidTagThumbnailPhoto = 0x8C9E,

        /// <summary>
        /// Contains a signed integer that specifies the seniority order of Address Book objects that represent members of a department and are referenced by a Department object or departmental group, with larger values specifying members that are more senior.
        /// </summary>
        PidTagAddressBookSeniorityIndex = 0x8CA0,

        /// <summary>
        /// Contains the DN of the Organization object of the mail user's organization.
        /// </summary>
        PidTagAddressBookOrganizationalUnitRootDistinguishedName = 0x8CA8,

        /// <summary>
        /// Contains the locale ID and translations of the default mail tip.
        /// </summary>
        PidTagAddressBookSenderHintTranslations = 0x8CAC,

        /// <summary>
        /// Indicates whether moderation is enabled for the mail user or distribution list.
        /// </summary>
        PidTagAddressBookModerationEnabled = 0x8CB5,

        /// <summary>
        /// Contains a recording of the mail user's name pronunciation.
        /// </summary>
        PidTagSpokenName = 0x8CC2,

        /// <summary>
        /// Indicates whether delivery restrictions exist for a recipient.
        /// </summary>
        PidTagAddressBookAuthorizedSenders = 0x8CD8,

        /// <summary>
        /// Indicates whether delivery restrictions exist for a recipient.
        /// </summary>
        PidTagAddressBookUnauthorizedSenders = 0x8CD9,

        /// <summary>
        /// Indicates that delivery restrictions exist for a recipient.
        /// </summary>
        PidTagAddressBookDistributionListMemberSubmitRejected = 0x8CDA,

        /// <summary>
        /// Indicates that delivery restrictions exist for a recipient.
        /// </summary>
        PidTagAddressBookDistributionListRejectMessagesFromDLMembers = 0x8CDB,

        /// <summary>
        /// Indicates whether the distribution list represents a departmental group.
        /// </summary>
        PidTagAddressBookHierarchicalIsHierarchicalGroup = 0x8CDD,

        /// <summary>
        /// Contains the total number of recipients in the distribution list.
        /// </summary>
        PidTagAddressBookDistributionListMemberCount = 0x8CE2,

        /// <summary>
        /// Contains the number of external recipients in the distribution list.
        /// </summary>
        PidTagAddressBookDistributionListExternalMemberCount = 0x8CE3,

        /// <summary>
        /// Contains a Boolean value of TRUE if it is possible to create Address Book objects in that container, and FALSE otherwise.
        /// </summary>
        PidTagAddressBookIsMaster = 0xFFFB,

        /// <summary>
        /// Contains the EntryID of the parent container in a hierarchy of address book containers.
        /// </summary>
        PidTagAddressBookParentEntryId = 0xFFFC,

        /// <summary>
        /// Contains the ID of a container on an NSPI server.
        /// </summary>
        PidTagAddressBookContainerId = 0xFFFD
    }
}
