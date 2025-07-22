using System;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The DecodingContext is shared between some ROP request and response.
    /// </summary>
    public class DecodingContext
    {
        /// <summary>
        /// Record the LogonId and RopLogon flags.
        /// </summary>
        private static Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<byte, LogonFlags>>>> logonFlagMapLogId;

        /// <summary>
        /// Record the map in session information,LogonId and RopLogon flags.
        /// </summary>
        private static Dictionary<int, Dictionary<byte, LogonFlags>> sessionLogonFlagMapLogId;

        /// <summary>
        /// Record the map in session information, handle index and logonFlags in RopLogon ROP.
        /// </summary>
        private static Dictionary<int, Dictionary<uint, LogonFlags>> sessionLogonFlagsInLogonRop;

        /// <summary>
        /// Record the map in session information, handle index, and PropertyTags for getPropertiesSpecific ROP.
        /// </summary>
        private static Dictionary<int, Dictionary<uint, Queue<PropertyTag[]>>> getPropertiesSpecPropertyTags;

        /// <summary>
        /// Record the map in session id and the remain seize in ROP list parsing.
        /// </summary>
        private static Dictionary<int, List<uint>> sessionRequestRemainSize;

        /// <summary>
        /// Record RopSetColumn InputObjectHandle in setColumn Response.
        /// </summary>
        private static List<uint> setColumnInputHandlesInResponse;

        /// <summary>
        /// Record the map of SetColumns's output handle, session id and tuple for row ROPs.
        /// </summary>
        private static Dictionary<uint, Dictionary<int, Tuple<string, string, string, PropertyTag[]>>> rowRopsHandlePropertyTags;

        /// <summary>
        /// Record the map in session id, handle index and PropertyTags for row ROPs.
        /// </summary>
        private static Dictionary<string, Dictionary<int, Dictionary<uint, PropertyTag[]>>> rowRopsSessionPropertyTags;

        /// <summary>
        /// Record the map of SetColumns's output handle, session id and tuple for RopNotify.
        /// </summary>
        private static Dictionary<uint, Dictionary<int, Tuple<string, string, string, PropertyTag[], string>>> notifyHandlePropertyTags;

        /// <summary>
        /// Record the map of serverUrl, session id, object handle and PropertyTags for RopNotify.
        /// </summary>
        private static Dictionary<string, Dictionary<int, Dictionary<uint, PropertyTag[]>>> notifySessionPropertyTags;

        /// <summary>
        /// Record the map in session id and partial information is ready.
        /// </summary>
        private static Dictionary<int, bool> partialInformationReady;

        /// <summary>
        /// Initializes a new instance of the DecodingContext class
        /// </summary>
        public DecodingContext()
        {
            logonFlagMapLogId = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<byte, LogonFlags>>>>();
            sessionLogonFlagMapLogId = new Dictionary<int, Dictionary<byte, LogonFlags>>();
            sessionLogonFlagsInLogonRop = new Dictionary<int, Dictionary<uint, LogonFlags>>();
            getPropertiesSpecPropertyTags = new Dictionary<int, Dictionary<uint, Queue<PropertyTag[]>>>();
            sessionRequestRemainSize = new Dictionary<int, List<uint>>();
            rowRopsHandlePropertyTags = new Dictionary<uint, Dictionary<int, Tuple<string, string, string, PropertyTag[]>>>();
            notifyHandlePropertyTags = new Dictionary<uint, Dictionary<int, Tuple<string, string, string, PropertyTag[], string>>>();
            notifySessionPropertyTags = new Dictionary<string, Dictionary<int, Dictionary<uint, PropertyTag[]>>>();
            rowRopsSessionPropertyTags = new Dictionary<string, Dictionary<int, Dictionary<uint, PropertyTag[]>>>();
            setColumnInputHandlesInResponse = new List<uint>();
            partialInformationReady = new Dictionary<int, bool>();
        }

        /// <summary>
        /// Gets or sets the LogonId and RopLogon flags
        /// </summary>
        public static Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<byte, LogonFlags>>>> LogonFlagMapLogId
        {
            get
            {
                return logonFlagMapLogId;
            }

            set
            {
                logonFlagMapLogId = value;
            }
        }

        /// <summary>
        /// Gets or sets the sessionLogonFlagMapLogId
        /// </summary>
        public static Dictionary<int, Dictionary<byte, LogonFlags>> SessionLogonFlagMapLogId
        {
            get
            {
                return sessionLogonFlagMapLogId;
            }

            set
            {
                sessionLogonFlagMapLogId = value;
            }
        }

        /// <summary>
        /// Gets or sets the sessionLogonFlagsInLogonRop
        /// </summary>
        public static Dictionary<int, Dictionary<uint, LogonFlags>> SessionLogonFlagsInLogonRop
        {
            get
            {
                return sessionLogonFlagsInLogonRop;
            }

            set
            {
                sessionLogonFlagsInLogonRop = value;
            }
        }

        /// <summary>
        /// Gets or sets the getPropertiesSpec_propertyTags
        /// </summary>
        public static Dictionary<int, Dictionary<uint, Queue<PropertyTag[]>>> GetPropertiesSpec_propertyTags
        {
            get
            {
                return getPropertiesSpecPropertyTags;
            }

            set
            {
                getPropertiesSpecPropertyTags = value;
            }
        }

        /// <summary>
        /// Gets or sets the sessionRequestRemainSize
        /// </summary>
        public static Dictionary<int, List<uint>> SessionRequestRemainSize
        {
            get
            {
                return sessionRequestRemainSize;
            }

            set
            {
                sessionRequestRemainSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the rowRops_handlePropertyTags
        /// </summary>
        public static Dictionary<uint, Dictionary<int, Tuple<string, string, string, PropertyTag[]>>> RowRops_handlePropertyTags
        {
            get
            {
                return rowRopsHandlePropertyTags;
            }

            set
            {
                rowRopsHandlePropertyTags = value;
            }
        }

        /// <summary>
        /// Gets or sets the rowRops_sessionpropertyTags
        /// </summary>
        public static Dictionary<string, Dictionary<int, Dictionary<uint, PropertyTag[]>>> RowRops_sessionPropertyTags
        {
            get
            {
                return rowRopsSessionPropertyTags;
            }

            set
            {
                rowRopsSessionPropertyTags = value;
            }
        }

        /// <summary>
        /// Gets or sets the notify_handlePropertyTags
        /// </summary>
        public static Dictionary<uint, Dictionary<int, Tuple<string, string, string, PropertyTag[], string>>> Notify_handlePropertyTags
        {
            get
            {
                return notifyHandlePropertyTags;
            }

            set
            {
                notifyHandlePropertyTags = value;
            }
        }

        /// <summary>
        /// Gets or sets the notify_sessionPropertyTags
        /// </summary>
        public static Dictionary<string, Dictionary<int, Dictionary<uint, PropertyTag[]>>> Notify_sessionPropertyTags
        {
            get
            {
                return notifySessionPropertyTags;
            }

            set
            {
                notifySessionPropertyTags = value;
            }
        }

        /// <summary>
        /// Gets or sets the setColumn_InputHandles
        /// </summary>
        public static List<uint> SetColumn_InputHandles_InResponse
        {
            get
            {
                return setColumnInputHandlesInResponse;
            }

            set
            {
                setColumnInputHandlesInResponse = value;
            }
        }

        /// <summary>
        /// Gets or sets the partialInformationReady
        /// </summary>
        public static Dictionary<int, bool> PartialInformationReady
        {
            get
            {
                return partialInformationReady;
            }

            set
            {
                partialInformationReady = value;
            }
        }
    }
}
