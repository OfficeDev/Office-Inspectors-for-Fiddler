namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The array of Property long ID (LID) related to Canonical name and guid
    /// </summary>
    public static class NamedProperties
    {
        public static NamedProperty[] Properties =
        {
            /// <summary>
            /// Specifies the state of the electronic addresses of the contact and represents a set of bit flags.
            /// </summary>
            new NamedProperty("PidLidAddressBookProviderArrayType", 0x8029, Guids.PSETID_Address, PropertyDataType.PtypInteger32, "Contact"),

            /// <summary>
            /// Specifies which electronic address properties are set on the Contact object.
            /// </summary>
            new NamedProperty("PidLidAddressBookProviderEmailList", 0x8028, Guids.PSETID_Address, PropertyDataType.PtypMultipleInteger32, "Contact"),

            /// <summary>
            /// Specifies the country code portion of the mailing address of the contact.
            /// </summary>
            new NamedProperty("PidLidAddressCountryCode", 0x80DD, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies whether to automatically archive the message.
            /// </summary>
            new NamedProperty("PidLidAgingDontAgeMe", 0x850E, Guids.PSETID_Common, PropertyDataType.PtypBoolean, "Common"),

            /// <summary>
            /// Specifies a list of all the attendees except for the organizer, including resources and unsendable attendees.
            /// </summary>
            new NamedProperty("PidLidAllAttendeesString", 0x8238, Guids.PSETID_Appointment, PropertyDataType.PtypString, "Meetings"),

            /// <summary>
            /// This property is set to TRUE.
            /// </summary>
            new NamedProperty("PidLidAllowExternalCheck", 0x8246, Guids.PSETID_Appointment, PropertyDataType.PtypBoolean, "Conferencing"),

            /// <summary>
            /// Specifies the EntryID of the Appointment object that represents an anniversary of the contact.
            /// </summary>
            new NamedProperty("PidLidAnniversaryEventEntryId", 0x804E, Guids.PSETID_Address, PropertyDataType.PtypBinary, "Contact"),

            /// <summary>
            /// Specifies a bit field that describes the auxiliary state of the object.
            /// </summary>
            new NamedProperty("PidLidAppointmentAuxiliaryFlags", 0x8207, Guids.PSETID_Appointment, PropertyDataType.PtypInteger32, "Meetings"),

            /// <summary>
            /// Specifies the color to be used when displaying the Calendar object.
            /// </summary>
            new NamedProperty("PidLidAppointmentColor", 0x8214, Guids.PSETID_Appointment, PropertyDataType.PtypInteger32, "Calendar"),

            /// <summary>
            /// Indicates whether a Meeting Response object is a counter proposal.
            /// </summary>
            new NamedProperty("PidLidAppointmentCounterProposal", 0x8257, Guids.PSETID_Appointment, PropertyDataType.PtypBoolean, "Meetings"),

            /// <summary>
            /// Specifies the length of the event, in minutes.
            /// </summary>
            new NamedProperty("PidLidAppointmentDuration", 0x8213, Guids.PSETID_Appointment, PropertyDataType.PtypInteger32, "Calendar"),

            /// <summary>
            /// Indicates the date that the appointment ends.
            /// </summary>
            new NamedProperty("PidLidAppointmentEndDate", 0x8211, Guids.PSETID_Appointment, PropertyDataType.PtypTime, "Calendar"),

            /// <summary>
            /// Indicates the time that the appointment ends.
            /// </summary>
            new NamedProperty("PidLidAppointmentEndTime", 0x8210, Guids.PSETID_Appointment, PropertyDataType.PtypTime, "Calendar"),

            /// <summary>
            /// Specifies the end date and time for the event.
            /// </summary>
            new NamedProperty("PidLidAppointmentEndWhole", 0x820E, Guids.PSETID_Appointment, PropertyDataType.PtypTime, "Calendar"),

            /// <summary>
            /// Indicates to the organizer the last sequence number that was sent to any attendee.
            /// </summary>
            new NamedProperty("PidLidAppointmentLastSequence", 0x8203, Guids.PSETID_Appointment, PropertyDataType.PtypInteger32, "Meetings"),

            /// <summary>
            /// Indicates the message class of the Meeting object to be generated from the Meeting Request object.
            /// </summary>
            new NamedProperty("PidLidAppointmentMessageClass", 0x0024, Guids.PSETID_Meeting, PropertyDataType.PtypString, "Meetings"),

            /// <summary>
            /// Indicates whether attendees are not allowed to propose a new date and/or time for the meeting.
            /// </summary>
            new NamedProperty("PidLidAppointmentNotAllowPropose", 0x825A, Guids.PSETID_Appointment, PropertyDataType.PtypBoolean, "Meetings"),

            /// <summary>
            /// Specifies the number of attendees who have sent counter proposals that have not been accepted or rejected by the organizer.
            /// </summary>
            new NamedProperty("PidLidAppointmentProposalNumber", 0x8259, Guids.PSETID_Appointment, PropertyDataType.PtypInteger32, "Meetings"),

            /// <summary>
            /// Indicates the proposed value for the PidLidAppointmentDuration property (section 2.11) for a counter proposal.
            /// </summary>
            new NamedProperty("PidLidAppointmentProposedDuration", 0x8256, Guids.PSETID_Appointment, PropertyDataType.PtypInteger32, "Meetings"),

            /// <summary>
            /// Specifies the proposed value for the PidLidAppointmentEndWhole property (section 2.14) for a counter proposal.
            /// </summary>
            new NamedProperty("PidLidAppointmentProposedEndWhole", 0x8251, Guids.PSETID_Appointment, PropertyDataType.PtypTime, "Meetings"),

            /// <summary>
            /// Specifies the proposed value for the PidLidAppointmentStartWhole property (section 2.29) for a counter proposal.
            /// </summary>
            new NamedProperty("PidLidAppointmentProposedStartWhole", 0x8250, Guids.PSETID_Appointment, PropertyDataType.PtypTime, "Meetings"),

            /// <summary>
            /// Specifies the dates and times when a recurring series occurs.
            /// </summary>
            new NamedProperty("PidLidAppointmentRecur", 0x8216, Guids.PSETID_Appointment, PropertyDataType.PtypBinary, "Calendar"),

            /// <summary>
            /// Specifies the user who last replied to the meeting request or meeting update.
            /// </summary>
            new NamedProperty("PidLidAppointmentReplyName", 0x8230, Guids.PSETID_Appointment, PropertyDataType.PtypString, "Meetings"),

            /// <summary>
            /// Specifies the date and time at which the attendee responded to a received meeting request or Meeting Update object.
            /// </summary>
            new NamedProperty("PidLidAppointmentReplyTime", 0x8220, Guids.PSETID_Appointment, PropertyDataType.PtypTime, "Meetings"),

            /// <summary>
            /// Specifies the sequence number of a Meeting object.
            /// </summary>
            new NamedProperty("PidLidAppointmentSequence", 0x8201, Guids.PSETID_Appointment, PropertyDataType.PtypInteger32, "Meetings"),

            /// <summary>
            /// Indicates the date and time at which the PidLidAppointmentSequence property (section 2.25) was last modified.
            /// </summary>
            new NamedProperty("PidLidAppointmentSequenceTime", 0x8202, Guids.PSETID_Appointment, PropertyDataType.PtypTime, "Meetings"),

            /// <summary>
            /// Identifies the date that the appointment starts.
            /// </summary>
            new NamedProperty("PidLidAppointmentStartDate", 0x8212, Guids.PSETID_Appointment, PropertyDataType.PtypTime, "Calendar"),

            /// <summary>
            /// Identifies the time that the appointment starts.
            /// </summary>
            new NamedProperty("PidLidAppointmentStartTime", 0x820F, Guids.PSETID_Appointment, PropertyDataType.PtypTime, "Calendar"),

            /// <summary>
            /// Specifies the start date and time of the appointment.
            /// </summary>
            new NamedProperty("PidLidAppointmentStartWhole", 0x820D, Guids.PSETID_Appointment, PropertyDataType.PtypTime, "Calendar"),

            /// <summary>
            /// Specifies a bit field that describes the state of the object.
            /// </summary>
            new NamedProperty("PidLidAppointmentStateFlags", 0x8217, Guids.PSETID_Appointment, PropertyDataType.PtypInteger32, "Meetings"),

            /// <summary>
            /// Specifies whether the event is an all-day event.
            /// </summary>
            new NamedProperty("PidLidAppointmentSubType", 0x8215, Guids.PSETID_Appointment, PropertyDataType.PtypBoolean, "Calendar"),

            /// <summary>
            /// Specifies time zone information that indicates the time zone of the PidLidAppointmentEndWhole property (section 2.14).
            /// </summary>
            new NamedProperty("PidLidAppointmentTimeZoneDefinitionEndDisplay", 0x825F, Guids.PSETID_Appointment, PropertyDataType.PtypBinary, "Calendar"),

            /// <summary>
            /// Specifies time zone information that describes how to convert the meeting date and time on a recurring series to and from UTC.
            /// </summary>
            new NamedProperty("PidLidAppointmentTimeZoneDefinitionRecur", 0x8260, Guids.PSETID_Appointment, PropertyDataType.PtypBinary, "Calendar"),

            /// <summary>
            /// Specifies time zone information that indicates the time zone of the PidLidAppointmentStartWhole property (section 2.29).
            /// </summary>
            new NamedProperty("PidLidAppointmentTimeZoneDefinitionStartDisplay", 0x825E, Guids.PSETID_Appointment, PropertyDataType.PtypBinary, "Calendar"),

            /// <summary>
            /// Contains a list of unsendable attendees.
            /// </summary>
            new NamedProperty("PidLidAppointmentUnsendableRecipients", 0x825D, Guids.PSETID_Appointment, PropertyDataType.PtypBinary, "Meetings"),

            /// <summary>
            /// Indicates the time at which the appointment was last updated.
            /// </summary>
            new NamedProperty("PidLidAppointmentUpdateTime", 0x8226, Guids.PSETID_Appointment, PropertyDataType.PtypTime, "Meetings"),

            /// <summary>
            /// Specifies the date and time at which the meeting-related object was sent.
            /// </summary>
            new NamedProperty("PidLidAttendeeCriticalChange", 0x0001, Guids.PSETID_Meeting, PropertyDataType.PtypTime, "Meetings"),

            /// <summary>
            /// Indicates whether the value of the PidLidLocation property (section 2.159) is set to the PidTagDisplayName property (section 2.667).
            /// </summary>
            new NamedProperty("PidLidAutoFillLocation", 0x823A, Guids.PSETID_Appointment, PropertyDataType.PtypBoolean, "Meetings"),

            /// <summary>
            /// Specifies to the application whether to create a Journal object for each action associated with this Contact object.
            /// </summary>
            new NamedProperty("PidLidAutoLog", 0x8025, Guids.PSETID_Address, PropertyDataType.PtypBoolean, "Contact"),

            /// <summary>
            /// Specifies the options used in the automatic processing of email messages.
            /// </summary>
            new NamedProperty("PidLidAutoProcessState", 0x851A, Guids.PSETID_Common, PropertyDataType.PtypInteger32, "General"),

            /// <summary>
            /// Specifies whether to automatically start the conferencing application when a reminder for the start of a meeting is executed.
            /// </summary>
            new NamedProperty("PidLidAutoStartCheck", 0x8244, Guids.PSETID_Appointment, PropertyDataType.PtypBoolean, "Conferencing"),

            /// <summary>
            /// Specifies billing information for the contact.
            /// </summary>
            new NamedProperty("PidLidBilling", 0x8535, Guids.PSETID_Common, PropertyDataType.PtypString, "General"),

            /// <summary>
            /// Specifies the EntryID of an optional Appointment object that represents the birthday of the contact.
            /// </summary>
            new NamedProperty("PidLidBirthdayEventEntryId", 0x804D, Guids.PSETID_Address, PropertyDataType.PtypBinary, "Contact"),

            /// <summary>
            /// Specifies the birthday of a contact.
            /// </summary>
            new NamedProperty("PidLidBirthdayLocal", 0x80DE, Guids.PSETID_Address, PropertyDataType.PtypTime, "Contact"),

            /// <summary>
            /// Contains the image to be used on a business card.
            /// </summary>
            new NamedProperty("PidLidBusinessCardCardPicture", 0x8041, Guids.PSETID_Address, PropertyDataType.PtypBinary, "Contact"),

            /// <summary>
            /// Contains user customization details for displaying a contact as a business card.
            /// </summary>
            new NamedProperty("PidLidBusinessCardDisplayDefinition", 0x8040, Guids.PSETID_Address, PropertyDataType.PtypBinary, "Contact"),

            /// <summary>
            /// Specifies the availability of a user for the event described by the object.
            /// </summary>
            new NamedProperty("PidLidBusyStatus", 0x8205, Guids.PSETID_Appointment, PropertyDataType.PtypInteger32, "Calendar"),

            /// <summary>
            /// Contains the value of the CalendarType field from the PidLidAppointmentRecur property (section 2.22).
            /// </summary>
            new NamedProperty("PidLidCalendarType", 0x001C, Guids.PSETID_Meeting, PropertyDataType.PtypInteger32, "Meetings"),

            /// <summary>
            /// Contains the array of text labels assigned to this Message object.
            /// </summary>
            new NamedProperty("PidLidCategories", 0x9000, Guids.PS_PUBLIC_STRINGS, PropertyDataType.PtypMultipleString, "Common"),

            /// <summary>
            /// Contains a list of all the sendable attendees who are also optional attendees.
            /// </summary>
            new NamedProperty("PidLidCcAttendeesString", 0x823C, Guids.PSETID_Appointment, PropertyDataType.PtypString, "Meetings"),

            /// <summary>
            /// Specifies a bit field that indicates how the Meeting object has changed.
            /// </summary>
            new NamedProperty("PidLidChangeHighlight", 0x8204, Guids.PSETID_Appointment, PropertyDataType.PtypInteger32, "Meetings"),

            /// <summary>
            /// Contains a list of the classification categories to which the associated Message object has been assigned.
            /// </summary>
            new NamedProperty("PidLidClassification", 0x85B6, Guids.PSETID_Common, PropertyDataType.PtypString, "General"),

            /// <summary>
            /// The PidLidClassificationDescription
            /// </summary>
            new NamedProperty("PidLidClassificationDescription", 0x85B7, Guids.PSETID_Common, PropertyDataType.PtypString, "General"),

            /// <summary>
            /// Contains the GUID that identifies the list of email classification categories used by a Message object.
            /// </summary>
            new NamedProperty("PidLidClassificationGuid", 0x85B8, Guids.PSETID_Common, PropertyDataType.PtypString, "General"),

            /// <summary>
            /// Indicates whether the message uses any classification categories.
            /// </summary>
            new NamedProperty("PidLidClassificationKeep", 0x85BA, Guids.PSETID_Common, PropertyDataType.PtypBoolean, "General"),

            /// <summary>
            /// Indicates whether the contents of this message are regarded as classified information.
            /// </summary>
            new NamedProperty("PidLidClassified", 0x85B5, Guids.PSETID_Common, PropertyDataType.PtypBoolean, "General"),

            /// <summary>
            /// Contains the value of the PidLidGlobalObjectId property (section 2.142) for an object that represents an Exception object to a recurring series, where the Year, Month, and Day fields are all zero.
            /// </summary>
            new NamedProperty("PidLidCleanGlobalObjectId", 0x0023, Guids.PSETID_Meeting, PropertyDataType.PtypBinary, "Meetings"),

            /// <summary>
            /// Indicates what actions the user has taken on this Meeting object.
            /// </summary>
            new NamedProperty("PidLidClientIntent", 0x0015, Guids.PSETID_CalendarAssistant, PropertyDataType.PtypInteger32, "Calendar"),

            /// <summary>
            /// Specifies the end date and time of the event in UTC.
            /// </summary>
            new NamedProperty("PidLidClipEnd", 0x8236, Guids.PSETID_Appointment, PropertyDataType.PtypTime, "Calendar"),

            /// <summary>
            /// Specifies the start date and time of the event in UTC.
            /// </summary>
            new NamedProperty("PidLidClipStart", 0x8235, Guids.PSETID_Appointment, PropertyDataType.PtypTime, "Calendar"),

            /// <summary>
            /// Specifies the document to be launched when the user joins the meeting.
            /// </summary>
            new NamedProperty("PidLidCollaborateDoc", 0x8247, Guids.PSETID_Appointment, PropertyDataType.PtypString, "Conferencing"),

            /// <summary>
            /// Indicates the end time for the Message object.
            /// </summary>
            new NamedProperty("PidLidCommonEnd", 0x8517, Guids.PSETID_Common, PropertyDataType.PtypTime, "General"),

            /// <summary>
            /// Indicates the start time for the Message object.
            /// </summary>
            new NamedProperty("PidLidCommonStart", 0x8516, Guids.PSETID_Common, PropertyDataType.PtypTime, "General"),

            /// <summary>
            /// Contains a list of company names, each of which is associated with a contact that is specified in the PidLidContacts property ([MS-OXCMSG] section 2.2.1.57.2).
            /// </summary>
            new NamedProperty("PidLidCompanies", 0x8539, Guids.PSETID_Common, PropertyDataType.PtypMultipleString, "General"),

            /// <summary>
            /// The PidLidConferencingCheck
            /// </summary>
            new NamedProperty("PidLidConferencingCheck", 0x8240, Guids.PSETID_Appointment, PropertyDataType.PtypBoolean, "Conferencing"),

            /// <summary>
            /// Specifies the type of the meeting.
            /// </summary>
            new NamedProperty("PidLidConferencingType", 0x8241, Guids.PSETID_Appointment, PropertyDataType.PtypInteger32, "Conferencing"),

            /// <summary>
            /// Specifies the character set used for a Contact object.
            /// </summary>
            new NamedProperty("PidLidContactCharacterSet", 0x8023, Guids.PSETID_Address, PropertyDataType.PtypInteger32, "Contact"),

            /// <summary>
            /// Specifies the visible fields in the application's user interface that are used to help display the contact information.
            /// </summary>
            new NamedProperty("PidLidContactItemData", 0x8007, Guids.PSETID_Address, PropertyDataType.PtypMultipleInteger32, "Contact"),

            /// <summary>
            /// Specifies the EntryID of the GAL contact to which the duplicate contact is linked.
            /// </summary>
            new NamedProperty("PidLidContactLinkedGlobalAddressListEntryId", 0x80E2, Guids.PSETID_Address, PropertyDataType.PtypBinary, "Contact"),

            /// <summary>
            /// Contains the elements of the PidLidContacts property (section 2.77).
            /// </summary>
            new NamedProperty("PidLidContactLinkEntry", 0x8585, Guids.PSETID_Common, PropertyDataType.PtypBinary, "Contact"),

            /// <summary>
            /// Specifies the GUID of the GAL contact to which the duplicate contact is linked.
            /// </summary>
            new NamedProperty("PidLidContactLinkGlobalAddressListLinkId", 0x80E8, Guids.PSETID_Address, PropertyDataType.PtypGuid, "Contact"),

            /// <summary>
            /// Specifies the state of the linking between the GAL contact and the duplicate contact.
            /// </summary>
            new NamedProperty("PidLidContactLinkGlobalAddressListLinkState", 0x80E6, Guids.PSETID_Address, PropertyDataType.PtypInteger32, "Contact"),

            /// <summary>
            /// Contains a list of GAL contacts that were previously rejected for linking with the duplicate contact.
            /// </summary>
            new NamedProperty("PidLidContactLinkLinkRejectHistory", 0x80E5, Guids.PSETID_Address, PropertyDataType.PtypMultipleBinary, "Contact"),

            /// <summary>
            /// The PidLidContactLinkName
            /// </summary>
            new NamedProperty("PidLidContactLinkName", 0x8586, Guids.PSETID_Common, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Contains the list of SearchKeys for a Contact object linked to by the Message object.
            /// </summary>
            new NamedProperty("PidLidContactLinkSearchKey", 0x8584, Guids.PSETID_Common, PropertyDataType.PtypBinary, "Contact"),

            /// <summary>
            /// Contains a list of the SMTP addresses that are used by the contact.
            /// </summary>
            new NamedProperty("PidLidContactLinkSMTPAddressCache", 0x80E3, Guids.PSETID_Address, PropertyDataType.PtypMultipleString, "Contact"),

            /// <summary>
            /// Contains the PidTagDisplayName property (section 2.667) of each Address Book EntryID referenced in the value of the PidLidContactLinkEntry property (section 2.70).
            /// </summary>
            new NamedProperty("PidLidContacts", 0x853A, Guids.PSETID_Common, PropertyDataType.PtypMultipleString, "General"),

            /// <summary>
            /// Contains text used to add custom text to a business card representation of a Contact object.
            /// </summary>
            new NamedProperty("PidLidContactUserField1", 0x804F, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Contains text used to add custom text to a business card representation of a Contact object.
            /// </summary>
            new NamedProperty("PidLidContactUserField2", 0x8050, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Contains text used to add custom text to a business card representation of a Contact object.
            /// </summary>
            new NamedProperty("PidLidContactUserField3", 0x8051, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Contains text used to add custom text to a business card representation of a Contact object.
            /// </summary>
            new NamedProperty("PidLidContactUserField4", 0x8052, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Contains the time, in UTC, that an Email object was last received in the conversation, or the last time that the user modified the conversation action, whichever occurs later.
            /// </summary>
            new NamedProperty("PidLidConversationActionLastAppliedTime", 0x85CA, Guids.PSETID_Common, PropertyDataType.PtypTime, "Conversation"),

            /// <summary>
            /// Contains the maximum value of the PidTagMessageDeliveryTime property (section 2.780) of all of the Email objects modified in response to the last time that the user changed a conversation action on the client.
            /// </summary>
            new NamedProperty("PidLidConversationActionMaxDeliveryTime", 0x85C8, Guids.PSETID_Common, PropertyDataType.PtypTime, "Conversation"),

            /// <summary>
            /// Contains the EntryID for the destination folder.
            /// </summary>
            new NamedProperty("PidLidConversationActionMoveFolderEid", 0x85C6, Guids.PSETID_Common, PropertyDataType.PtypBinary, "Conversation"),

            /// <summary>
            /// Contains the EntryID for a move to a folder in a different message store.
            /// </summary>
            new NamedProperty("PidLidConversationActionMoveStoreEid", 0x85C7, Guids.PSETID_Common, PropertyDataType.PtypBinary, "Conversation"),

            /// <summary>
            /// Contains the version of the conversation action FAI message.
            /// </summary>
            new NamedProperty("PidLidConversationActionVersion", 0x85CB, Guids.PSETID_Common, PropertyDataType.PtypInteger32, "Conversation"),

            /// <summary>
            /// Specifies a sequential number to be used in the processing of a conversation action.
            /// </summary>
            new NamedProperty("PidLidConversationProcessed", 0x85C9, Guids.PSETID_Common, PropertyDataType.PtypInteger32, "Conversation"),

            /// <summary>
            /// Specifies the build number of the client application that sent the message.
            /// </summary>
            new NamedProperty("PidLidCurrentVersion", 0x8552, Guids.PSETID_Common, PropertyDataType.PtypInteger32, "General"),

            /// <summary>
            /// Specifies the name of the client application that sent the message.
            /// </summary>
            new NamedProperty("PidLidCurrentVersionName", 0x8554, Guids.PSETID_Common, PropertyDataType.PtypString, "General"),

            /// <summary>
            /// Identifies the day interval for the recurrence pattern.
            /// </summary>
            new NamedProperty("PidLidDayInterval", 0x0011, Guids.PSETID_Meeting, PropertyDataType.PtypInteger16, "Meetings"),

            /// <summary>
            /// Identifies the day of the month for the appointment or meeting.
            /// </summary>
            new NamedProperty("PidLidDayOfMonth", 0x1000, Guids.PSETID_Common, PropertyDataType.PtypInteger32, "Calendar"),

            /// <summary>
            /// Indicates whether a delegate responded to the meeting request.
            /// </summary>
            new NamedProperty("PidLidDelegateMail", 0x0009, Guids.PSETID_Meeting, PropertyDataType.PtypBoolean, "Meetings"),

            /// <summary>
            /// This property is ignored by the server and is set to an empty string by the client.
            /// </summary>
            new NamedProperty("PidLidDepartment", 0x8010, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies the directory server to be used.
            /// </summary>
            new NamedProperty("PidLidDirectory", 0x8242, Guids.PSETID_Appointment, PropertyDataType.PtypString, "Conferencing"),

            /// <summary>
            /// Specifies the 32-bit cyclic redundancy check (CRC) polynomial checksum, as specified in [ISO/IEC8802-3], calculated on the value of the PidLidDistributionListMembers property (section 2.96).
            /// </summary>
            new NamedProperty("PidLidDistributionListChecksum", 0x804C, Guids.PSETID_Address, PropertyDataType.PtypInteger32, "Contact"),

            /// <summary>
            /// Specifies the list of EntryIDs of the objects corresponding to the members of the personal distribution list.
            /// </summary>
            new NamedProperty("PidLidDistributionListMembers", 0x8055, Guids.PSETID_Address, PropertyDataType.PtypMultipleBinary, "Contact"),

            /// <summary>
            /// Specifies the name of the personal distribution list.
            /// </summary>
            new NamedProperty("PidLidDistributionListName", 0x8053, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies the list of one-off EntryIDs corresponding to the members of the personal distribution list.
            /// </summary>
            new NamedProperty("PidLidDistributionListOneOffMembers", 0x8054, Guids.PSETID_Address, PropertyDataType.PtypMultipleBinary, "Contact"),

            /// <summary>
            /// Specifies the list of EntryIDs and one-off EntryIDs corresponding to the members of the personal distribution list.
            /// </summary>
            new NamedProperty("PidLidDistributionListStream", 0x8064, Guids.PSETID_Address, PropertyDataType.PtypBinary, "Contact"),

            /// <summary>
            /// Specifies the address type of an electronic address.
            /// </summary>
            new NamedProperty("PidLidEmail1AddressType", 0x8082, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies the user-readable display name for the email address.
            /// </summary>
            new NamedProperty("PidLidEmail1DisplayName", 0x8080, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies the email address of the contact.
            /// </summary>
            new NamedProperty("PidLidEmail1EmailAddress", 0x8083, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies the SMTP email address that corresponds to the email address for the Contact object.
            /// </summary>
            new NamedProperty("PidLidEmail1OriginalDisplayName", 0x8084, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies the EntryID of the object corresponding to this electronic address.
            /// </summary>
            new NamedProperty("PidLidEmail1OriginalEntryId", 0x8085, Guids.PSETID_Address, PropertyDataType.PtypBinary, "Contact"),

            /// <summary>
            /// Specifies the address type of the electronic address.
            /// </summary>
            new NamedProperty("PidLidEmail2AddressType", 0x8092, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies the user-readable display name for the email address.
            /// </summary>
            new NamedProperty("PidLidEmail2DisplayName", 0x8090, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies the email address of the contact.
            /// </summary>
            new NamedProperty("PidLidEmail2EmailAddress", 0x8093, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies the SMTP email address that corresponds to the email address for the Contact object.
            /// </summary>
            new NamedProperty("PidLidEmail2OriginalDisplayName", 0x8094, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies the EntryID of the object that corresponds to this electronic address.
            /// </summary>
            new NamedProperty("PidLidEmail2OriginalEntryId", 0x8095, Guids.PSETID_Address, PropertyDataType.PtypBinary, "Contact"),

            /// <summary>
            /// Specifies the address type of the electronic address.
            /// </summary>
            new NamedProperty("PidLidEmail3AddressType", 0x80A2, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies the user-readable display name for the email address.
            /// </summary>
            new NamedProperty("PidLidEmail3DisplayName", 0x80A0, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies the email address of the contact.
            /// </summary>
            new NamedProperty("PidLidEmail3EmailAddress", 0x80A3, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies the SMTP email address that corresponds to the email address for the Contact object.
            /// </summary>
            new NamedProperty("PidLidEmail3OriginalDisplayName", 0x80A4, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies the EntryID of the object that corresponds to this electronic address.
            /// </summary>
            new NamedProperty("PidLidEmail3OriginalEntryId", 0x80A5, Guids.PSETID_Address, PropertyDataType.PtypBinary, "Contact"),

            /// <summary>
            /// Identifies the end date of the recurrence range.
            /// </summary>
            new NamedProperty("PidLidEndRecurrenceDate", 0x000F, Guids.PSETID_Meeting, PropertyDataType.PtypInteger32, "Meetings"),

            /// <summary>
            /// Identifies the end time of the recurrence range.
            /// </summary>
            new NamedProperty("PidLidEndRecurrenceTime", 0x0010, Guids.PSETID_Meeting, PropertyDataType.PtypInteger32, "Meetings"),

            /// <summary>
            /// Specifies the date and time, in UTC, within a recurrence pattern that an exception will replace.
            /// </summary>
            new NamedProperty("PidLidExceptionReplaceTime", 0x8228, Guids.PSETID_Appointment, PropertyDataType.PtypTime, "Calendar"),

            /// <summary>
            /// Contains the string value "FAX".
            /// </summary>
            new NamedProperty("PidLidFax1AddressType", 0x80B2, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Contains a user-readable display name, followed by the "@" character, followed by a fax number.
            /// </summary>
            new NamedProperty("PidLidFax1EmailAddress", 0x80B3, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Contains the same value as the PidTagNormalizedSubject property (section 2.803).
            /// </summary>
            new NamedProperty("PidLidFax1OriginalDisplayName", 0x80B4, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies a one-off EntryID that corresponds to this fax address.
            /// </summary>
            new NamedProperty("PidLidFax1OriginalEntryId", 0x80B5, Guids.PSETID_Address, PropertyDataType.PtypBinary, "Contact"),

            /// <summary>
            /// Contains the string value "FAX".
            /// </summary>
            new NamedProperty("PidLidFax2AddressType", 0x80C2, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Contains a user-readable display name, followed by the "@" character, followed by a fax number.
            /// </summary>
            new NamedProperty("PidLidFax2EmailAddress", 0x80C3, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Contains the same value as the PidTagNormalizedSubject property (section 2.803).
            /// </summary>
            new NamedProperty("PidLidFax2OriginalDisplayName", 0x80C4, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies a one-off EntryID corresponding to this fax address.
            /// </summary>
            new NamedProperty("PidLidFax2OriginalEntryId", 0x80C5, Guids.PSETID_Address, PropertyDataType.PtypBinary, "Contact"),

            /// <summary>
            /// Contains the string value "FAX".
            /// </summary>
            new NamedProperty("PidLidFax3AddressType", 0x80D2, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Contains a user-readable display name, followed by the "@" character, followed by a fax number.
            /// </summary>
            new NamedProperty("PidLidFax3EmailAddress", 0x80D3, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Contains the same value as the PidTagNormalizedSubject property (section 2.803).
            /// </summary>
            new NamedProperty("PidLidFax3OriginalDisplayName", 0x80D4, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies a one-off EntryID that corresponds to this fax address.
            /// </summary>
            new NamedProperty("PidLidFax3OriginalEntryId", 0x80D5, Guids.PSETID_Address, PropertyDataType.PtypBinary, "Contact"),

            /// <summary>
            /// Indicates that the object is a Recurring Calendar object with one or more exceptions, and that at least one of the Exception Embedded Message objects has at least one RecipientRow structure, as described in [MS-OXCDATA] section 2.8.3.
            /// </summary>
            new NamedProperty("PidLidFExceptionalAttendees", 0x822B, Guids.PSETID_Appointment, PropertyDataType.PtypBoolean, "Meetings"),

            /// <summary>
            /// Indicates that the Exception Embedded Message object has a body that differs from the Recurring Calendar object.
            /// </summary>
            new NamedProperty("PidLidFExceptionalBody", 0x8206, Guids.PSETID_Appointment, PropertyDataType.PtypBoolean, "Meetings"),

            /// <summary>
            /// Specifies the name under which to file a contact when displaying a list of contacts.
            /// </summary>
            new NamedProperty("PidLidFileUnder", 0x8005, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies how to generate and recompute the value of the PidLidFileUnder property (section 2.132) when other contact name properties change.
            /// </summary>
            new NamedProperty("PidLidFileUnderId", 0x8006, Guids.PSETID_Address, PropertyDataType.PtypInteger32, "Contact"),

            /// <summary>
            /// Specifies a list of possible values for the PidLidFileUnderId property (section 2.133).
            /// </summary>
            new NamedProperty("PidLidFileUnderList", 0x8026, Guids.PSETID_Address, PropertyDataType.PtypMultipleInteger32, "Contact"),

            /// <summary>
            /// Indicates whether invitations have been sent for the meeting that this Meeting object represents.
            /// </summary>
            new NamedProperty("PidLidFInvited", 0x8229, Guids.PSETID_Appointment, PropertyDataType.PtypBoolean, "Meetings"),

            /// <summary>
            /// Contains user-specifiable text to be associated with the flag.
            /// </summary>
            new NamedProperty("PidLidFlagRequest", 0x8530, Guids.PSETID_Common, PropertyDataType.PtypString, "Flagging"),

            /// <summary>
            /// Contains an index identifying one of a set of pre-defined text strings to be associated with the flag.
            /// </summary>
            new NamedProperty("PidLidFlagString", 0x85C0, Guids.PSETID_Common, PropertyDataType.PtypInteger32, "Tasks"),

            /// <summary>
            /// Indicates whether the Meeting Request object represents an exception to a recurring series, and whether it was forwarded (even when forwarded by the organizer) rather than being an invitation sent by the organizer.
            /// </summary>
            new NamedProperty("PidLidForwardInstance", 0x820A, Guids.PSETID_Appointment, PropertyDataType.PtypBoolean, "Meetings"),

            /// <summary>
            /// Contains a list of RecipientRow structures, as described in [MS-OXCDATA] section 2.8.3, that indicate the recipients of a meeting forward.
            /// </summary>
            new NamedProperty("PidLidForwardNotificationRecipients", 0x8261, Guids.PSETID_Appointment, PropertyDataType.PtypBinary, "Meetings"),

            /// <summary>
            /// Indicates whether the Calendar folder from which the meeting was opened is another user's calendar.
            /// </summary>
            new NamedProperty("PidLidFOthersAppointment", 0x822F, Guids.PSETID_Appointment, PropertyDataType.PtypBoolean, "Meetings"),

            /// <summary>
            /// Specifies a URL path from which a client can retrieve free/busy status information for the contact.
            /// </summary>
            new NamedProperty("PidLidFreeBusyLocation", 0x80D8, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Contains an ID for an object that represents an exception to a recurring series.
            /// </summary>
            new NamedProperty("PidLidGlobalObjectId", 0x0003, Guids.PSETID_Meeting, PropertyDataType.PtypBinary, "Meetings"),

            /// <summary>
            /// Specifies whether the attachment has a picture.
            /// </summary>
            new NamedProperty("PidLidHasPicture", 0x8015, Guids.PSETID_Address, PropertyDataType.PtypBoolean, "Contact"),

            /// <summary>
            /// Specifies the complete address of the home address of the contact.
            /// </summary>
            new NamedProperty("PidLidHomeAddress", 0x801A, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies the country code portion of the home address of the contact.
            /// </summary>
            new NamedProperty("PidLidHomeAddressCountryCode", 0x80DA, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies the business webpage URL of the contact.
            /// </summary>
            new NamedProperty("PidLidHtml", 0x802B, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Identifies the day of the week for the appointment or meeting.
            /// </summary>
            new NamedProperty("PidLidICalendarDayOfWeekMask", 0x1001, Guids.PSETID_Common, PropertyDataType.PtypInteger32, "Calendar"),

            /// <summary>
            /// Contains the contents of the iCalendar MIME part of the original MIME message.
            /// </summary>
            new NamedProperty("PidLidInboundICalStream", 0x827A, Guids.PSETID_Appointment, PropertyDataType.PtypBinary, "Calendar"),

            /// <summary>
            /// Contains the name of the form associated with this message.
            /// </summary>
            new NamedProperty("PidLidInfoPathFormName", 0x85B1, Guids.PSETID_Common, PropertyDataType.PtypString, "Common"),

            /// <summary>
            /// Specifies the instant messaging address of the contact.
            /// </summary>
            new NamedProperty("PidLidInstantMessagingAddress", 0x8062, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Contains the value of the PidLidBusyStatus property (section 2.47) on the Meeting object in the organizer's calendar at the time that the Meeting Request object or Meeting Update object was sent.
            /// </summary>
            new NamedProperty("PidLidIntendedBusyStatus", 0x8224, Guids.PSETID_Appointment, PropertyDataType.PtypInteger32, "Meetings"),

            /// <summary>
            /// Specifies the user-visible email account name through which the email message is sent.
            /// </summary>
            new NamedProperty("PidLidInternetAccountName", 0x8580, Guids.PSETID_Common, PropertyDataType.PtypString, "General"),

            /// <summary>
            /// Specifies the email account ID through which the email message is sent.
            /// </summary>
            new NamedProperty("PidLidInternetAccountStamp", 0x8581, Guids.PSETID_Common, PropertyDataType.PtypString, "General"),

            /// <summary>
            /// Specifies whether the contact is linked to other contacts.
            /// </summary>
            new NamedProperty("PidLidIsContactLinked", 0x80E0, Guids.PSETID_Address, PropertyDataType.PtypBoolean, "Contact"),

            /// <summary>
            /// Indicates whether the object represents an exception (including an orphan instance).
            /// </summary>
            new NamedProperty("PidLidIsException", 0x000A, Guids.PSETID_Meeting, PropertyDataType.PtypBoolean, "Meetings"),

            /// <summary>
            /// Specifies whether the object is associated with a recurring series.
            /// </summary>
            new NamedProperty("PidLidIsRecurring", 0x0005, Guids.PSETID_Meeting, PropertyDataType.PtypBoolean, "Meetings"),

            /// <summary>
            /// Indicates whether the user did not include any text in the body of the Meeting Response object.
            /// </summary>
            new NamedProperty("PidLidIsSilent", 0x0004, Guids.PSETID_Meeting, PropertyDataType.PtypBoolean, "Meetings"),

            /// <summary>
            /// Indicates whether the user did not include any text in the body of the Meeting Response object.
            /// </summary>
            new NamedProperty("PidLidLinkedTaskItems", 0x820C, Guids.PSETID_Appointment, PropertyDataType.PtypMultipleBinary, "Tasks"),

            /// <summary>
            /// Specifies the location of the event.
            /// </summary>
            new NamedProperty("PidLidLocation", 0x8208, Guids.PSETID_Appointment, PropertyDataType.PtypString, "Calendar"),

            /// <summary>
            /// Indicates whether the document was sent by email or posted to a server folder during journaling.
            /// </summary>
            new NamedProperty("PidLidLogDocumentPosted", 0x8711, Guids.PSETID_Log, PropertyDataType.PtypBoolean, "Journal"),

            /// <summary>
            /// Indicates whether the document was printed during journaling.
            /// </summary>
            new NamedProperty("PidLidLogDocumentPrinted", 0x870E, Guids.PSETID_Log, PropertyDataType.PtypBoolean, "Journal"),

            /// <summary>
            /// Indicates whether the document was sent to a routing recipient during journaling.
            /// </summary>
            new NamedProperty("PidLidLogDocumentRouted", 0x8710, Guids.PSETID_Log, PropertyDataType.PtypBoolean, "Journal"),

            /// <summary>
            /// Indicates whether the document was saved during journaling.
            /// </summary>
            new NamedProperty("PidLidLogDocumentSaved", 0x870F, Guids.PSETID_Log, PropertyDataType.PtypBoolean, "Journal"),

            /// <summary>
            /// Contains the duration, in minutes, of the activity.
            /// </summary>
            new NamedProperty("PidLidLogDuration", 0x8707, Guids.PSETID_Log, PropertyDataType.PtypInteger32, "Journal"),

            /// <summary>
            /// Contains the time, in UTC, at which the activity ended.
            /// </summary>
            new NamedProperty("PidLidLogEnd", 0x8708, Guids.PSETID_Log, PropertyDataType.PtypTime, "Journal"),

            /// <summary>
            /// Contains metadata about the Journal object.
            /// </summary>
            new NamedProperty("PidLidLogFlags", 0x870C, Guids.PSETID_Log, PropertyDataType.PtypInteger32, "Journal"),

            /// <summary>
            /// Contains the time, in UTC, at which the activity began.
            /// </summary>
            new NamedProperty("PidLidLogStart", 0x8706, Guids.PSETID_Log, PropertyDataType.PtypTime, "Journal"),

            /// <summary>
            /// Briefly describes the journal activity that is being recorded.
            /// </summary>
            new NamedProperty("PidLidLogType", 0x8700, Guids.PSETID_Log, PropertyDataType.PtypString, "Journal"),

            /// <summary>
            /// Contains an expanded description of the journal activity that is being recorded.
            /// </summary>
            new NamedProperty("PidLidLogTypeDesc", 0x8712, Guids.PSETID_Log, PropertyDataType.PtypString, "Journal"),

            /// <summary>
            /// Indicates the type of Meeting Request object or Meeting Update object.
            /// </summary>
            new NamedProperty("PidLidMeetingType", 0x0026, Guids.PSETID_Meeting, PropertyDataType.PtypInteger32, "Meetings"),

            /// <summary>
            /// Specifies the URL of the Meeting Workspace that is associated with a Calendar object.
            /// </summary>
            new NamedProperty("PidLidMeetingWorkspaceUrl", 0x8209, Guids.PSETID_Appointment, PropertyDataType.PtypString, "Meetings"),

            /// <summary>
            /// Indicates the monthly interval of the appointment or meeting.
            /// </summary>
            new NamedProperty("PidLidMonthInterval", 0x0013, Guids.PSETID_Meeting, PropertyDataType.PtypInteger16, "Meetings"),

            /// <summary>
            /// Indicates the month of the year in which the appointment or meeting occurs.
            /// </summary>
            new NamedProperty("PidLidMonthOfYear", 0x1006, Guids.PSETID_Common, PropertyDataType.PtypInteger32, "Calendar"),

            /// <summary>
            /// Indicates the calculated month of the year in which the appointment or meeting occurs.
            /// </summary>
            new NamedProperty("PidLidMonthOfYearMask", 0x0017, Guids.PSETID_Meeting, PropertyDataType.PtypInteger32, "Meetings"),

            /// <summary>
            /// Specifies the URL to be launched when the user joins the meeting.
            /// </summary>
            new NamedProperty("PidLidNetShowUrl", 0x8248, Guids.PSETID_Appointment, PropertyDataType.PtypString, "Conferencing"),

            /// <summary>
            /// Indicates whether the recurrence pattern has an end date.
            /// </summary>
            new NamedProperty("PidLidNoEndDateFlag", 0x100B, Guids.PSETID_Common, PropertyDataType.PtypBoolean, "Calendar"),

            /// <summary>
            /// Contains a list of all of the unsendable attendees who are also resources.
            /// </summary>
            new NamedProperty("PidLidNonSendableBcc", 0x8538, Guids.PSETID_Common, PropertyDataType.PtypString, "Meetings"),

            /// <summary>
            /// Contains a list of all of the unsendable attendees who are also optional attendees.
            /// </summary>
            new NamedProperty("PidLidNonSendableCc", 0x8537, Guids.PSETID_Common, PropertyDataType.PtypString, "Meetings"),

            /// <summary>
            /// Contains a list of all of the unsendable attendees who are also required attendees.
            /// </summary>
            new NamedProperty("PidLidNonSendableTo", 0x8536, Guids.PSETID_Common, PropertyDataType.PtypString, "Meetings"),

            /// <summary>
            /// Contains the value from the response table.
            /// </summary>
            new NamedProperty("PidLidNonSendBccTrackStatus", 0x8545, Guids.PSETID_Common, PropertyDataType.PtypMultipleInteger32, "General"),

            /// <summary>
            /// Contains the value from the response table.
            /// </summary>
            new NamedProperty("PidLidNonSendCcTrackStatus", 0x8544, Guids.PSETID_Common, PropertyDataType.PtypMultipleInteger32, "General"),

            /// <summary>
            /// Contains the value from the response table.
            /// </summary>
            new NamedProperty("PidLidNonSendToTrackStatus", 0x8543, Guids.PSETID_Common, PropertyDataType.PtypMultipleInteger32, "General"),

            /// <summary>
            /// Specifies the suggested background color of the Note object.
            /// </summary>
            new NamedProperty("PidLidNoteColor", 0x8B00, Guids.PSETID_Note, PropertyDataType.PtypInteger32, "Sticky"),

            /// <summary>
            /// Specifies the height of the visible message window in pixels.
            /// </summary>
            new NamedProperty("PidLidNoteHeight", 0x8B03, Guids.PSETID_Note, PropertyDataType.PtypInteger32, "Sticky"),

            /// <summary>
            /// Specifies the width of the visible message window in pixels.
            /// </summary>
            new NamedProperty("PidLidNoteWidth", 0x8B02, Guids.PSETID_Note, PropertyDataType.PtypInteger32, "Sticky"),

            /// <summary>
            /// Specifies the distance, in pixels, from the left edge of the screen that a user interface displays a Note object.
            /// </summary>
            new NamedProperty("PidLidNoteX", 0x8B04, Guids.PSETID_Note, PropertyDataType.PtypInteger32, "Sticky"),

            /// <summary>
            /// Specifies the distance, in pixels, from the top edge of the screen that a user interface displays a Note object.
            /// </summary>
            new NamedProperty("PidLidNoteY", 0x8B05, Guids.PSETID_Note, PropertyDataType.PtypInteger32, "Sticky"),

            /// <summary>
            /// Indicates the number of occurrences in the recurring appointment or meeting.
            /// </summary>
            new NamedProperty("PidLidOccurrences", 0x1005, Guids.PSETID_Common, PropertyDataType.PtypInteger32, "Calendar"),

            /// <summary>
            /// Indicates the original value of the PidLidLocation property (section 2.159) before a meeting update.
            /// </summary>
            new NamedProperty("PidLidOldLocation", 0x0028, Guids.PSETID_Meeting, PropertyDataType.PtypString, "Meetings"),

            /// <summary>
            /// Indicates the recurrence pattern for the appointment or meeting.
            /// </summary>
            new NamedProperty("PidLidOldRecurrenceType", 0x0018, Guids.PSETID_Meeting, PropertyDataType.PtypInteger16, "Meetings"),

            /// <summary>
            /// Indicates the original value of the PidLidAppointmentEndWhole property (section 2.14) before a meeting update.
            /// </summary>
            new NamedProperty("PidLidOldWhenEndWhole", 0x002A, Guids.PSETID_Meeting, PropertyDataType.PtypTime, "Meetings"),

            /// <summary>
            /// Indicates the original value of the PidLidAppointmentStartWhole property (section 2.29) before a meeting update.
            /// </summary>
            new NamedProperty("PidLidOldWhenStartWhole", 0x0029, Guids.PSETID_Meeting, PropertyDataType.PtypTime, "Meetings"),

            /// <summary>
            /// Specifies the password for a meeting on which the PidLidConferencingType property (section 2.66) has the value 0x00000002.
            /// </summary>
            new NamedProperty("PidLidOnlinePassword", 0x8249, Guids.PSETID_Appointment, PropertyDataType.PtypString, "Conferencing"),

            /// <summary>
            /// Specifies optional attendees.
            /// </summary>
            new NamedProperty("PidLidOptionalAttendees", 0x0007, Guids.PSETID_Meeting, PropertyDataType.PtypString, "Meetings"),

            /// <summary>
            /// Specifies the email address of the organizer.
            /// </summary>
            new NamedProperty("PidLidOrganizerAlias", 0x8243, Guids.PSETID_Appointment, PropertyDataType.PtypString, "Conferencing"),

            /// <summary>
            /// Specifies the EntryID of the delegator’s message store.
            /// </summary>
            new NamedProperty("PidLidOriginalStoreEntryId", 0x8237, Guids.PSETID_Appointment, PropertyDataType.PtypBinary, "Meetings"),

            /// <summary>
            /// Specifies the complete address of the other address of the contact.
            /// </summary>
            new NamedProperty("PidLidOtherAddress", 0x801C, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies the country code portion of the other address of the contact.
            /// </summary>
            new NamedProperty("PidLidOtherAddressCountryCode", 0x80DC, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies the date and time at which a Meeting Request object was sent by the organizer.
            /// </summary>
            new NamedProperty("PidLidOwnerCriticalChange", 0x001A, Guids.PSETID_Meeting, PropertyDataType.PtypTime, "Meetings"),

            /// <summary>
            /// Indicates the name of the owner of the mailbox.
            /// </summary>
            new NamedProperty("PidLidOwnerName", 0x822E, Guids.PSETID_Appointment, PropertyDataType.PtypString, "Meetings"),

            /// <summary>
            /// Specifies the synchronization state of the Document object that is in the Document Libraries folder of the site mailbox.
            /// </summary>
            new NamedProperty("PidLidPendingStateForSiteMailboxDocument", 0x85E0, Guids.PSETID_Common, PropertyDataType.PtypInteger32, "Site"),

            /// <summary>
            /// Indicates whether a time-flagged Message object is complete.
            /// </summary>
            new NamedProperty("PidLidPercentComplete", 0x8102, Guids.PSETID_Task, PropertyDataType.PtypFloating64, "Tasks"),
            /// <summary>
            /// Specifies which physical address is the mailing address for this contact.
            /// </summary>
            new NamedProperty("PidLidPostalAddressId", 0x8022, Guids.PSETID_Address, PropertyDataType.PtypInteger32, "Contact"),

            /// <summary>
            /// Contains the contents of the title field from the XML of the Atom feed or RSS channel.
            /// </summary>
            new NamedProperty("PidLidPostRssChannel", 0x8904, Guids.PSETID_PostRss, PropertyDataType.PtypString, "RSS"),

            /// <summary>
            /// Contains the URL of the RSS or Atom feed from which the XML file came.
            /// </summary>
            new NamedProperty("PidLidPostRssChannelLink", 0x8900, Guids.PSETID_PostRss, PropertyDataType.PtypString, "RSS"),

            /// <summary>
            /// Contains a unique identifier for the RSS object.
            /// </summary>
            new NamedProperty("PidLidPostRssItemGuid", 0x8903, Guids.PSETID_PostRss, PropertyDataType.PtypString, "RSS"),

            /// <summary>
            /// Contains a hash of the feed XML computed by using an implementation-dependent algorithm.
            /// </summary>
            new NamedProperty("PidLidPostRssItemHash", 0x8902, Guids.PSETID_PostRss, PropertyDataType.PtypInteger32, "RSS"),

            /// <summary>
            /// Contains the URL of the link from an RSS or Atom item.
            /// </summary>
            new NamedProperty("PidLidPostRssItemLink", 0x8901, Guids.PSETID_PostRss, PropertyDataType.PtypString, "RSS"),

            /// <summary>
            /// Contains the item element and all of its sub-elements from an RSS feed, or the entry element and all of its sub-elements from an Atom feed.
            /// </summary>
            new NamedProperty("PidLidPostRssItemXml", 0x8905, Guids.PSETID_PostRss, PropertyDataType.PtypString, "RSS"),

            /// <summary>
            /// Contains the user's preferred name for the RSS or Atom subscription.
            /// </summary>
            new NamedProperty("PidLidPostRssSubscription", 0x8906, Guids.PSETID_PostRss, PropertyDataType.PtypString, "RSS"),

            /// <summary>
            /// Indicates whether the end user wishes for this Message object to be hidden from other users who have access to the Message object.
            /// </summary>
            new NamedProperty("PidLidPrivate", 0x8506, Guids.PSETID_Common, PropertyDataType.PtypBoolean, "General"),

            /// <summary>
            /// Indicates that the Meeting Response object was out-of-date when it was received.
            /// </summary>
            new NamedProperty("PidLidPromptSendUpdate", 0x8045, Guids.PSETID_Common, PropertyDataType.PtypBoolean, "Meeting"),

            /// <summary>
            /// Identifies the length, in minutes, of the appointment or meeting.
            /// </summary>
            new NamedProperty("PidLidRecurrenceDuration", 0x100D, Guids.PSETID_Common, PropertyDataType.PtypInteger32, "Calendar"),

            /// <summary>
            /// Specifies a description of the recurrence pattern of the Calendar object.
            /// </summary>
            new NamedProperty("PidLidRecurrencePattern", 0x8232, Guids.PSETID_Appointment, PropertyDataType.PtypString, "Calendar"),

            /// <summary>
            /// Specifies the recurrence type of the recurring series.
            /// </summary>
            new NamedProperty("PidLidRecurrenceType", 0x8231, Guids.PSETID_Appointment, PropertyDataType.PtypInteger32, "Calendar"),

            /// <summary>
            /// Specifies whether the object represents a recurring series.
            /// </summary>
            new NamedProperty("PidLidRecurring", 0x8223, Guids.PSETID_Appointment, PropertyDataType.PtypBoolean, "Calendar"),

            /// <summary>
            /// Specifies the value of the EntryID of the Contact object unless the Contact object is a copy of an earlier original.
            /// </summary>
            new NamedProperty("PidLidReferenceEntryId", 0x85BD, Guids.PSETID_Common, PropertyDataType.PtypBinary, "Contact"),

            /// <summary>
            /// Specifies the interval, in minutes, between the time at which the reminder first becomes overdue and the start time of the Calendar object.
            /// </summary>
            new NamedProperty("PidLidReminderDelta", 0x8501, Guids.PSETID_Common, PropertyDataType.PtypInteger32, "Reminders"),

            /// <summary>
            /// Specifies the filename of the sound that a client is to play when the reminder for that object becomes overdue.
            /// </summary>
            new NamedProperty("PidLidReminderFileParameter", 0x851F, Guids.PSETID_Common, PropertyDataType.PtypString, "Reminders"),

            /// <summary>
            /// Specifies whether the client is to respect the current values of the PidLidReminderPlaySound property (section 2.221) and the PidLidReminderFileParameter property (section 2.219), or use the default values for those properties.
            /// </summary>
            new NamedProperty("PidLidReminderOverride", 0x851C, Guids.PSETID_Common, PropertyDataType.PtypBoolean, "Reminders"),

            /// <summary>
            /// Specifies whether the client is to play a sound when the reminder becomes overdue.
            /// </summary>
            new NamedProperty("PidLidReminderPlaySound", 0x851E, Guids.PSETID_Common, PropertyDataType.PtypBoolean, "Reminders"),

            /// <summary>
            /// Specifies whether a reminder is set on the object.
            /// </summary>
            new NamedProperty("PidLidReminderSet", 0x8503, Guids.PSETID_Common, PropertyDataType.PtypBoolean, "Reminders"),

            /// <summary>
            /// Specifies the point in time when a reminder transitions from pending to overdue.
            /// </summary>
            new NamedProperty("PidLidReminderSignalTime", 0x8560, Guids.PSETID_Common, PropertyDataType.PtypTime, "Reminders"),

            /// <summary>
            /// Specifies the initial signal time for objects that are not Calendar objects.
            /// </summary>
            new NamedProperty("PidLidReminderTime", 0x8502, Guids.PSETID_Common, PropertyDataType.PtypTime, "Reminders"),

            /// <summary>
            /// Indicates the time and date of the reminder for the appointment or meeting.
            /// </summary>
            new NamedProperty("PidLidReminderTimeDate", 0x8505, Guids.PSETID_Common, PropertyDataType.PtypTime, "Reminders"),

            /// <summary>
            /// Indicates the time of the reminder for the appointment or meeting.
            /// </summary>
            new NamedProperty("PidLidReminderTimeTime", 0x8504, Guids.PSETID_Common, PropertyDataType.PtypTime, "Reminders"),

            /// <summary>
            /// This property is not set and, if set, is ignored.
            /// </summary>
            new NamedProperty("PidLidReminderType", 0x851D, Guids.PSETID_Common, PropertyDataType.PtypInteger32, "Reminders"),

            /// <summary>
            /// Indicates the remote status of the calendar item.
            /// </summary>
            new NamedProperty("PidLidRemoteStatus", 0x8511, Guids.PSETID_Common, PropertyDataType.PtypInteger32, "Run"),

            /// <summary>
            /// Identifies required attendees for the appointment or meeting.
            /// </summary>
            new NamedProperty("PidLidRequiredAttendees", 0x0006, Guids.PSETID_Meeting, PropertyDataType.PtypString, "Meetings"),

            /// <summary>
            /// Identifies resource attendees for the appointment or meeting.
            /// </summary>
            new NamedProperty("PidLidResourceAttendees", 0x0008, Guids.PSETID_Meeting, PropertyDataType.PtypString, "Meetings"),

            /// <summary>
            /// Specifies the response status of an attendee.
            /// </summary>
            new NamedProperty("PidLidResponseStatus", 0x8218, Guids.PSETID_Appointment, PropertyDataType.PtypInteger32, "Meetings"),

            /// <summary>
            /// Indicates whether the Meeting Request object or Meeting Update object has been processed.
            /// </summary>
            new NamedProperty("PidLidServerProcessed", 0x85CC, Guids.PSETID_CalendarAssistant, PropertyDataType.PtypBoolean, "Calendar"),

            /// <summary>
            /// Indicates what processing actions have been taken on this Meeting Request object or Meeting Update object.
            /// </summary>
            new NamedProperty("PidLidServerProcessingActions", 0x85CD, Guids.PSETID_CalendarAssistant, PropertyDataType.PtypInteger32, "Calendar"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingAnonymity", 0x8A19, Guids.PSETID_Sharing, PropertyDataType.PtypInteger32, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingBindingEntryId", 0x8A2D, Guids.PSETID_Sharing, PropertyDataType.PtypBinary, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingBrowseUrl", 0x8A51, Guids.PSETID_Sharing, PropertyDataType.PtypString, "Sharing"),

            /// <summary>
            /// Indicates that the Message object relates to a special folder.
            /// </summary>
            new NamedProperty("PidLidSharingCapabilities", 0x8A17, Guids.PSETID_Sharing, PropertyDataType.PtypInteger32, "Sharing"),

            /// <summary>
            /// Contains a zero-length string.
            /// </summary>
            new NamedProperty("PidLidSharingConfigurationUrl", 0x8A24, Guids.PSETID_Sharing, PropertyDataType.PtypString, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingDataRangeEnd", 0x8A45, Guids.PSETID_Sharing, PropertyDataType.PtypTime, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingDataRangeStart", 0x8A44, Guids.PSETID_Sharing, PropertyDataType.PtypTime, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingDetail", 0x8A2B, Guids.PSETID_Sharing, PropertyDataType.PtypInteger32, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingExtensionXml", 0x8A21, Guids.PSETID_Sharing, PropertyDataType.PtypString, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingFilter", 0x8A13, Guids.PSETID_Sharing, PropertyDataType.PtypBinary, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingFlags", 0x8A0A, Guids.PSETID_Sharing, PropertyDataType.PtypInteger32, "Sharing"),

            /// <summary>
            /// Indicates the type of Sharing Message object.
            /// </summary>
            new NamedProperty("PidLidSharingFlavor", 0x8A18, Guids.PSETID_Sharing, PropertyDataType.PtypInteger32, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingFolderEntryId", 0x8A15, Guids.PSETID_Sharing, PropertyDataType.PtypBinary, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingIndexEntryId", 0x8A2E, Guids.PSETID_Sharing, PropertyDataType.PtypBinary, "Sharing"),

            /// <summary>
            /// Contains the value of the PidTagEntryId property (section 2.674) for the Address Book object of the currently logged-on user.
            /// </summary>
            new NamedProperty("PidLidSharingInitiatorEntryId", 0x8A09, Guids.PSETID_Sharing, PropertyDataType.PtypBinary, "Sharing"),

            /// <summary>
            /// Contains the value of the PidTagDisplayName property (section 2.667) from the Address Book object identified by the PidLidSharingInitiatorEntryId property (section 2.248).
            /// </summary>
            new NamedProperty("PidLidSharingInitiatorName", 0x8A07, Guids.PSETID_Sharing, PropertyDataType.PtypString, "Sharing"),

            /// <summary>
            /// Contains the value of the PidTagSmtpAddress property (section 2.1010) from the Address Book object identified by the PidLidSharingInitiatorEntryId property (section 2.248).
            /// </summary>
            new NamedProperty("PidLidSharingInitiatorSmtp", 0x8A08, Guids.PSETID_Sharing, PropertyDataType.PtypString, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingInstanceGuid", 0x8A1C, Guids.PSETID_Sharing, PropertyDataType.PtypGuid, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingLastAutoSyncTime", 0x8A55, Guids.PSETID_Sharing, PropertyDataType.PtypTime, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingLastSyncTime", 0x8A1F, Guids.PSETID_Sharing, PropertyDataType.PtypTime, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingLocalComment", 0x8A4D, Guids.PSETID_Sharing, PropertyDataType.PtypString, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingLocalLastModificationTime", 0x8A23, Guids.PSETID_Sharing, PropertyDataType.PtypTime, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingLocalName", 0x8A0F, Guids.PSETID_Sharing, PropertyDataType.PtypString, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingLocalPath", 0x8A0E, Guids.PSETID_Sharing, PropertyDataType.PtypString, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingLocalStoreUid", 0x8A49, Guids.PSETID_Sharing, PropertyDataType.PtypString, "Sharing"),

            /// <summary>
            /// Contains the value of the PidTagContainerClass property (section 2.633) of the folder being shared.
            /// </summary>
            new NamedProperty("PidLidSharingLocalType", 0x8A14, Guids.PSETID_Sharing, PropertyDataType.PtypString, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingLocalUid", 0x8A10, Guids.PSETID_Sharing, PropertyDataType.PtypString, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingOriginalMessageEntryId", 0x8A29, Guids.PSETID_Sharing, PropertyDataType.PtypBinary, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingParentBindingEntryId", 0x8A5C, Guids.PSETID_Sharing, PropertyDataType.PtypBinary, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingParticipants", 0x8A1E, Guids.PSETID_Sharing, PropertyDataType.PtypString, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingPermissions", 0x8A1B, Guids.PSETID_Sharing, PropertyDataType.PtypInteger32, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingProviderExtension", 0x8A0B, Guids.PSETID_Sharing, PropertyDataType.PtypString, "Sharing"),

            /// <summary>
            /// Contains the value "%xAE.F0.06.00.00.00.00.00.C0.00.00.00.00.00.00.46".
            /// </summary>
            new NamedProperty("PidLidSharingProviderGuid", 0x8A01, Guids.PSETID_Sharing, PropertyDataType.PtypGuid, "Sharing"),

            /// <summary>
            /// Contains a user-displayable name of the sharing provider identified by the PidLidSharingProviderGuid property (section 2.266).
            /// </summary>
            new NamedProperty("PidLidSharingProviderName", 0x8A02, Guids.PSETID_Sharing, PropertyDataType.PtypString, "Sharing"),

            /// <summary>
            /// Contains a URL related to the sharing provider identified by the PidLidSharingProviderGuid property (section 2.266).
            /// </summary>
            new NamedProperty("PidLidSharingProviderUrl", 0x8A03, Guids.PSETID_Sharing, PropertyDataType.PtypString, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingRangeEnd", 0x8A47, Guids.PSETID_Sharing, PropertyDataType.PtypInteger32, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingRangeStart", 0x8A46, Guids.PSETID_Sharing, PropertyDataType.PtypInteger32, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingReciprocation", 0x8A1A, Guids.PSETID_Sharing, PropertyDataType.PtypInteger32, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingRemoteByteSize", 0x8A4B, Guids.PSETID_Sharing, PropertyDataType.PtypInteger32, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingRemoteComment", 0x8A2F, Guids.PSETID_Sharing, PropertyDataType.PtypString, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingRemoteCrc", 0x8A4C, Guids.PSETID_Sharing, PropertyDataType.PtypInteger32, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingRemoteLastModificationTime", 0x8A22, Guids.PSETID_Sharing, PropertyDataType.PtypTime, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingRemoteMessageCount", 0x8A4F, Guids.PSETID_Sharing, PropertyDataType.PtypInteger32, "Sharing"),

            /// <summary>
            /// Contains the value of the PidTagDisplayName property (section 2.667) on the folder being shared.
            /// </summary>
            new NamedProperty("PidLidSharingRemoteName", 0x8A05, Guids.PSETID_Sharing, PropertyDataType.PtypString, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingRemotePass", 0x8A0D, Guids.PSETID_Sharing, PropertyDataType.PtypString, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingRemotePath", 0x8A04, Guids.PSETID_Sharing, PropertyDataType.PtypString, "Sharing"),

            /// <summary>
            /// Contains a hexadecimal string representation of the value of the PidTagStoreEntryId property (section 2.1018) on the folder being shared.
            /// </summary>
            new NamedProperty("PidLidSharingRemoteStoreUid", 0x8A48, Guids.PSETID_Sharing, PropertyDataType.PtypString, "Sharing"),

            /// <summary>
            /// Contains the same value as the PidLidSharingLocalType property (section 2.259).
            /// </summary>
            new NamedProperty("PidLidSharingRemoteType", 0x8A1D, Guids.PSETID_Sharing, PropertyDataType.PtypString, "Sharing"),

            /// <summary>
            /// Contains the EntryID of the folder being shared.
            /// </summary>
            new NamedProperty("PidLidSharingRemoteUid", 0x8A06, Guids.PSETID_Sharing, PropertyDataType.PtypString, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingRemoteUser", 0x8A0C, Guids.PSETID_Sharing, PropertyDataType.PtypString, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingRemoteVersion", 0x8A5B, Guids.PSETID_Sharing, PropertyDataType.PtypString, "Sharing"),

            /// <summary>
            /// Contains the time at which the recipient of the sharing request sent a sharing response.
            /// </summary>
            new NamedProperty("PidLidSharingResponseTime", 0x8A28, Guids.PSETID_Sharing, PropertyDataType.PtypTime, "Sharing"),

            /// <summary>
            /// Contains the type of response with which the recipient of the sharing request responded.
            /// </summary>
            new NamedProperty("PidLidSharingResponseType", 0x8A27, Guids.PSETID_Sharing, PropertyDataType.PtypInteger32, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingRoamLog", 0x8A4E, Guids.PSETID_Sharing, PropertyDataType.PtypInteger32, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingStart", 0x8A25, Guids.PSETID_Sharing, PropertyDataType.PtypTime, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingStatus", 0x8A00, Guids.PSETID_Sharing, PropertyDataType.PtypInteger32, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingStop", 0x8A26, Guids.PSETID_Sharing, PropertyDataType.PtypTime, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingSyncFlags", 0x8A60, Guids.PSETID_Sharing, PropertyDataType.PtypInteger32, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingSyncInterval", 0x8A2A, Guids.PSETID_Sharing, PropertyDataType.PtypInteger32, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingTimeToLive", 0x8A2C, Guids.PSETID_Sharing, PropertyDataType.PtypInteger32, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingTimeToLiveAuto", 0x8A56, Guids.PSETID_Sharing, PropertyDataType.PtypInteger32, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingWorkingHoursDays", 0x8A42, Guids.PSETID_Sharing, PropertyDataType.PtypInteger32, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingWorkingHoursEnd", 0x8A41, Guids.PSETID_Sharing, PropertyDataType.PtypTime, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingWorkingHoursStart", 0x8A40, Guids.PSETID_Sharing, PropertyDataType.PtypTime, "Sharing"),

            /// <summary>
            /// Contains a value that is ignored by the server no matter what value is generated by the client.
            /// </summary>
            new NamedProperty("PidLidSharingWorkingHoursTimeZone", 0x8A43, Guids.PSETID_Sharing, PropertyDataType.PtypBinary, "Sharing"),

            /// <summary>
            /// Specifies how a Message object is handled by the client in relation to certain user interface actions by the user, such as deleting a message.
            /// </summary>
            new NamedProperty("PidLidSideEffects", 0x8510, Guids.PSETID_Common, PropertyDataType.PtypInteger32, "Run"),

            /// <summary>
            /// Indicates that the original MIME message contained a single MIME part.
            /// </summary>
            new NamedProperty("PidLidSingleBodyICal", 0x827B, Guids.PSETID_Appointment, PropertyDataType.PtypBoolean, "Calendar"),

            /// <summary>
            /// Indicates whether the Message object has no end-user visible attachments.
            /// </summary>
            new NamedProperty("PidLidSmartNoAttach", 0x8514, Guids.PSETID_Common, PropertyDataType.PtypBoolean, "Run"),

            /// <summary>
            /// Specifies which folder a message was in before it was filtered into the Junk Email folder.
            /// </summary>
            new NamedProperty("PidLidSpamOriginalFolder", 0x859C, Guids.PSETID_Common, PropertyDataType.PtypBinary, "Spam"),

            /// <summary>
            /// Identifies the start date of the recurrence pattern.
            /// </summary>
            new NamedProperty("PidLidStartRecurrenceDate", 0x000D, Guids.PSETID_Meeting, PropertyDataType.PtypInteger32, "Meetings"),

            /// <summary>
            /// Identifies the start time of the recurrence pattern.
            /// </summary>
            new NamedProperty("PidLidStartRecurrenceTime", 0x000E, Guids.PSETID_Meeting, PropertyDataType.PtypInteger32, "Meetings"),

            /// <summary>
            /// Indicates the acceptance state of the task.
            /// </summary>
            new NamedProperty("PidLidTaskAcceptanceState", 0x812A, Guids.PSETID_Task, PropertyDataType.PtypInteger32, "Tasks"),

            /// <summary>
            /// Indicates whether a task assignee has replied to a task request for this Task object.
            /// </summary>
            new NamedProperty("PidLidTaskAccepted", 0x8108, Guids.PSETID_Task, PropertyDataType.PtypBoolean, "Tasks"),

            /// <summary>
            /// Indicates the number of minutes that the user actually spent working on a task.
            /// </summary>
            new NamedProperty("PidLidTaskActualEffort", 0x8110, Guids.PSETID_Task, PropertyDataType.PtypInteger32, "Tasks"),

            /// <summary>
            /// Specifies the name of the user that last assigned the task.
            /// </summary>
            new NamedProperty("PidLidTaskAssigner", 0x8121, Guids.PSETID_Task, PropertyDataType.PtypString, "Tasks"),

            /// <summary>
            /// Contains a stack of entries, each of which represents a task assigner.
            /// </summary>
            new NamedProperty("PidLidTaskAssigners", 0x8117, Guids.PSETID_Task, PropertyDataType.PtypBinary, "Tasks"),

            /// <summary>
            /// Indicates that the task is complete.
            /// </summary>
            new NamedProperty("PidLidTaskComplete", 0x811C, Guids.PSETID_Task, PropertyDataType.PtypBoolean, "Tasks"),

            /// <summary>
            /// The client can set this property, but it has no impact on the Task-Related Objects Protocol and is ignored by the server.
            /// </summary>
            new NamedProperty("PidLidTaskCustomFlags", 0x8139, Guids.PSETID_Task, PropertyDataType.PtypInteger32, "Tasks"),

            /// <summary>
            /// Specifies the date when the user completed work on the task.
            /// </summary>
            new NamedProperty("PidLidTaskDateCompleted", 0x810F, Guids.PSETID_Task, PropertyDataType.PtypTime, "Tasks"),

            /// <summary>
            /// Indicates whether new occurrences remain to be generated.
            /// </summary>
            new NamedProperty("PidLidTaskDeadOccurrence", 0x8109, Guids.PSETID_Task, PropertyDataType.PtypBoolean, "Tasks"),

            /// <summary>
            /// Specifies the date by which the user expects work on the task to be complete.
            /// </summary>
            new NamedProperty("PidLidTaskDueDate", 0x8105, Guids.PSETID_Task, PropertyDataType.PtypTime, "Tasks"),

            /// <summary>
            /// Indicates the number of minutes that the user expects to work on a task.
            /// </summary>
            new NamedProperty("PidLidTaskEstimatedEffort", 0x8111, Guids.PSETID_Task, PropertyDataType.PtypInteger32, "Tasks"),

            /// <summary>
            /// Indicates that the Task object was originally created by the action of the current user or user agent instead of by the processing of a task request.
            /// </summary>
            new NamedProperty("PidLidTaskFCreator", 0x811E, Guids.PSETID_Task, PropertyDataType.PtypBoolean, "Tasks"),

            /// <summary>
            /// Indicates the accuracy of the PidLidTaskOwner property (section 2.328).
            /// </summary>
            new NamedProperty("PidLidTaskFFixOffline", 0x812C, Guids.PSETID_Task, PropertyDataType.PtypBoolean, "Tasks"),

            /// <summary>
            /// Indicates whether the task includes a recurrence pattern.
            /// </summary>
            new NamedProperty("PidLidTaskFRecurring", 0x8126, Guids.PSETID_Task, PropertyDataType.PtypBoolean, "Tasks"),

            /// <summary>
            /// Contains a unique GUID for this task, which is used to locate an existing task upon receipt of a task response or task update.
            /// </summary>
            new NamedProperty("PidLidTaskGlobalId", 0x8519, Guids.PSETID_Common, PropertyDataType.PtypBinary, "Tasks"),

            /// <summary>
            /// Indicates the type of change that was last made to the Task object.
            /// </summary>
            new NamedProperty("PidLidTaskHistory", 0x811A, Guids.PSETID_Task, PropertyDataType.PtypInteger32, "Tasks"),

            /// <summary>
            /// Contains the name of the user who most recently assigned the task, or the user to whom it was most recently assigned.
            /// </summary>
            new NamedProperty("PidLidTaskLastDelegate", 0x8125, Guids.PSETID_Task, PropertyDataType.PtypString, "Tasks"),

            /// <summary>
            /// Contains the date and time of the most recent change made to the Task object.
            /// </summary>
            new NamedProperty("PidLidTaskLastUpdate", 0x8115, Guids.PSETID_Task, PropertyDataType.PtypTime, "Tasks"),

            /// <summary>
            /// Contains the name of the most recent user to have been the owner of the task.
            /// </summary>
            new NamedProperty("PidLidTaskLastUser", 0x8122, Guids.PSETID_Task, PropertyDataType.PtypString, "Tasks"),

            /// <summary>
            /// Specifies the assignment status of the embedded Task object.
            /// </summary>
            new NamedProperty("PidLidTaskMode", 0x8518, Guids.PSETID_Common, PropertyDataType.PtypInteger32, "Tasks"),

            /// <summary>
            /// Provides optimization hints about the recipients of a Task object.
            /// </summary>
            new NamedProperty("PidLidTaskMultipleRecipients", 0x8120, Guids.PSETID_Task, PropertyDataType.PtypInteger32, "Tasks"),

            /// <summary>
            /// Not used. The client can set this property, but it has no impact on the Task-Related Objects Protocol and is ignored by the server.
            /// </summary>
            new NamedProperty("PidLidTaskNoCompute", 0x8124, Guids.PSETID_Task, PropertyDataType.PtypBoolean, "Tasks"),

            /// <summary>
            /// Provides an aid to custom sorting of Task objects.
            /// </summary>
            new NamedProperty("PidLidTaskOrdinal", 0x8123, Guids.PSETID_Task, PropertyDataType.PtypInteger32, "Tasks"),

            /// <summary>
            /// Contains the name of the owner of the task.
            /// </summary>
            new NamedProperty("PidLidTaskOwner", 0x811F, Guids.PSETID_Task, PropertyDataType.PtypString, "Tasks"),

            /// <summary>
            /// Indicates the role of the current user relative to the Task object.
            /// </summary>
            new NamedProperty("PidLidTaskOwnership", 0x8129, Guids.PSETID_Task, PropertyDataType.PtypInteger32, "Tasks"),

            /// <summary>
            /// Contains a RecurrencePattern structure that provides information about recurring tasks.
            /// </summary>
            new NamedProperty("PidLidTaskRecurrence", 0x8116, Guids.PSETID_Task, PropertyDataType.PtypBinary, "Tasks"),

            /// <summary>
            /// Indicates whether future instances of recurring tasks need reminders, even though the value of the PidLidReminderSet property (section 2.222) is 0x00.
            /// </summary>
            new NamedProperty("PidLidTaskResetReminder", 0x8107, Guids.PSETID_Task, PropertyDataType.PtypBoolean, "Tasks"),

            /// <summary>
            /// Not used. The client can set this property, but it has no impact on the Task-Related Objects Protocol and is ignored by the server.
            /// </summary>
            new NamedProperty("PidLidTaskRole", 0x8127, Guids.PSETID_Task, PropertyDataType.PtypString, "Tasks"),

            /// <summary>
            /// Specifies the date on which the user expects work on the task to begin.
            /// </summary>
            new NamedProperty("PidLidTaskStartDate", 0x8104, Guids.PSETID_Task, PropertyDataType.PtypTime, "Tasks"),

            /// <summary>
            /// Indicates the current assignment state of the Task object.
            /// </summary>
            new NamedProperty("PidLidTaskState", 0x8113, Guids.PSETID_Task, PropertyDataType.PtypInteger32, "Tasks"),

            /// <summary>
            /// Specifies the status of a task.
            /// </summary>
            new NamedProperty("PidLidTaskStatus", 0x8101, Guids.PSETID_Task, PropertyDataType.PtypInteger32, "Tasks"),

            /// <summary>
            /// Indicates whether the task assignee has been requested to send an email message update upon completion of the assigned task.
            /// </summary>
            new NamedProperty("PidLidTaskStatusOnComplete", 0x8119, Guids.PSETID_Task, PropertyDataType.PtypBoolean, "Tasks"),

            /// <summary>
            /// Indicates whether the task assignee has been requested to send a task update when the assigned Task object changes.
            /// </summary>
            new NamedProperty("PidLidTaskUpdates", 0x811B, Guids.PSETID_Task, PropertyDataType.PtypBoolean, "Tasks"),

            /// <summary>
            /// Indicates which copy is the latest update of a Task object.
            /// </summary>
            new NamedProperty("PidLidTaskVersion", 0x8112, Guids.PSETID_Task, PropertyDataType.PtypInteger32, "Tasks"),

            /// <summary>
            /// This property is set by the client but is ignored by the server.
            /// </summary>
            new NamedProperty("PidLidTeamTask", 0x8103, Guids.PSETID_Task, PropertyDataType.PtypBoolean, "Tasks"),

            /// <summary>
            /// Specifies information about the time zone of a recurring meeting.
            /// </summary>
            new NamedProperty("PidLidTimeZone", 0x000C, Guids.PSETID_Meeting, PropertyDataType.PtypInteger32, "Meetings"),

            /// <summary>
            /// The PidLidTimeZoneDescription
            /// </summary>
            new NamedProperty("PidLidTimeZoneDescription", 0x8234, Guids.PSETID_Appointment, PropertyDataType.PtypString, "Calendar"),

            /// <summary>
            /// Specifies a human-readable description of the time zone that is represented by the data in the PidLidTimeZoneStruct property (section 2.342).
            /// </summary>
            new NamedProperty("PidLidTimeZoneStruct", 0x8233, Guids.PSETID_Appointment, PropertyDataType.PtypBinary, "Calendar"),

            /// <summary>
            /// Contains a list of all of the sendable attendees who are also required attendees.
            /// </summary>
            new NamedProperty("PidLidToAttendeesString", 0x823B, Guids.PSETID_Appointment, PropertyDataType.PtypString, "Meetings"),

            /// <summary>
            /// Contains the current time, in UTC, which is used to determine the sort order of objects in a consolidated to-do list.
            /// </summary>
            new NamedProperty("PidLidToDoOrdinalDate", 0x85A0, Guids.PSETID_Common, PropertyDataType.PtypTime, "Tasks"),

            /// <summary>
            /// Contains the numerals 0 through 9 that are used to break a tie when the PidLidToDoOrdinalDate property (section 2.344) is used to perform a sort of objects.
            /// </summary>
            new NamedProperty("PidLidToDoSubOrdinal", 0x85A1, Guids.PSETID_Common, PropertyDataType.PtypString, "Tasks"),

            /// <summary>
            /// Contains user-specifiable text to identify this Message object in a consolidated to-do list.
            /// </summary>
            new NamedProperty("PidLidToDoTitle", 0x85A4, Guids.PSETID_Common, PropertyDataType.PtypString, "Tasks"),

            /// <summary>
            /// Specifies whether Transport Neutral Encapsulation Format (TNEF) is to be included on a message when the message is converted from TNEF to MIME or SMTP format.
            /// </summary>
            new NamedProperty("PidLidUseTnef", 0x8582, Guids.PSETID_Common, PropertyDataType.PtypBoolean, "Run"),

            /// <summary>
            /// Contains the value of the PidTagMessageDeliveryTime property (section 2.780) when modifying the PidLidFlagRequest property (section 2.136).
            /// </summary>
            new NamedProperty("PidLidValidFlagStringProof", 0x85BF, Guids.PSETID_Common, PropertyDataType.PtypTime, "Tasks"),

            /// <summary>
            /// Specifies the voting option that a respondent has selected.
            /// </summary>
            new NamedProperty("PidLidVerbResponse", 0x8524, Guids.PSETID_Common, PropertyDataType.PtypString, "General"),

            /// <summary>
            /// Specifies what voting responses the user can make in response to the message.
            /// </summary>
            new NamedProperty("PidLidVerbStream", 0x8520, Guids.PSETID_Common, PropertyDataType.PtypBinary, "Run"),

            /// <summary>
            /// Specifies the wedding anniversary of the contact, at midnight in the client's local time zone, and is saved without any time zone conversions.
            /// </summary>
            new NamedProperty("PidLidWeddingAnniversaryLocal", 0x80DF, Guids.PSETID_Address, PropertyDataType.PtypTime, "Contact"),

            /// <summary>
            /// Identifies the number of weeks that occur between each meeting.
            /// </summary>
            new NamedProperty("PidLidWeekInterval", 0x0012, Guids.PSETID_Meeting, PropertyDataType.PtypInteger16, "Meetings"),

            /// <summary>
            /// Contains the value of the PidLidLocation property (section 2.159) from the associated Meeting object.
            /// </summary>
            new NamedProperty("PidLidWhere", 0x0002, Guids.PSETID_Meeting, PropertyDataType.PtypString, "Meetings"),

            /// <summary>
            /// Specifies the complete address of the work address of the contact.
            /// </summary>
            new NamedProperty("PidLidWorkAddress", 0x801B, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies the city or locality portion of the work address of the contact.
            /// </summary>
            new NamedProperty("PidLidWorkAddressCity", 0x8046, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies the country or region portion of the work address of the contact.
            /// </summary>
            new NamedProperty("PidLidWorkAddressCountry", 0x8049, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies the country code portion of the work address of the contact.
            /// </summary>
            new NamedProperty("PidLidWorkAddressCountryCode", 0x80DB, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies the postal code (ZIP code) portion of the work address of the contact.
            /// </summary>
            new NamedProperty("PidLidWorkAddressPostalCode", 0x8048, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies the post office box portion of the work address of the contact.
            /// </summary>
            new NamedProperty("PidLidWorkAddressPostOfficeBox", 0x804A, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies the state or province portion of the work address of the contact.
            /// </summary>
            new NamedProperty("PidLidWorkAddressState", 0x8047, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies the street portion of the work address of the contact.
            /// </summary>
            new NamedProperty("PidLidWorkAddressStreet", 0x8045, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Indicates the yearly interval of the appointment or meeting.
            /// </summary>
            new NamedProperty("PidLidYearInterval", 0x0014, Guids.PSETID_Meeting, PropertyDataType.PtypInteger16, "Meetings"),

            /// <summary>
            /// Specifies the phonetic pronunciation of the company name of the contact.
            /// </summary>
            new NamedProperty("PidLidYomiCompanyName", 0x802E, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies the phonetic pronunciation of the given name of the contact.
            /// </summary>
            new NamedProperty("PidLidYomiFirstName", 0x802C, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),

            /// <summary>
            /// Specifies the phonetic pronunciation of the surname of the contact.
            /// </summary>
            new NamedProperty("PidLidYomiLastName", 0x802D, Guids.PSETID_Address, PropertyDataType.PtypString, "Contact"),
        };
    }
}
