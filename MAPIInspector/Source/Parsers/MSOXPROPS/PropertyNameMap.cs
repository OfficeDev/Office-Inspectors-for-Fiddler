using System;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
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

            return $"{this.PidNamePropertyDic[key]} ({propName}), PropertySet: {propSet}" + ", " + guidValue.ToString();
        }
    }
}
