using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCNOTIF] 2.2.1.4.1.2 NotificationData Structure
    /// A class indicates the NotificationData
    /// </summary>
    public class NotificationFlags : Block
    {
        /// <summary>
        /// A combination of an enumeration and flags that describe the type of the notification and the availability of the notification data fields.
        /// The least significant 12 bits of the NotificationFlags field contain the NotificationType enumeration, which defines the type of the notification.
        /// The most significant four bits of the NotificationFlags field are flags that specify the availability of the notification data fields.
        /// </summary>
        public BlockT<NotificationTypes> Flags;

        public NotificationTypes NotificationType => (NotificationTypes)((ushort)Flags?.Data & 0x0FFF);

        public bool T => (Flags?.Data & NotificationTypes.T) != 0;
        public bool U => (Flags?.Data & NotificationTypes.U) != 0;
        public bool S => (Flags?.Data & NotificationTypes.S) != 0;
        public bool M => (Flags?.Data & NotificationTypes.M) != 0;

        public bool HasFlag(NotificationTypes flag) => Flags != null && Flags.Data.HasFlag(flag);

        /// <summary>
        /// Parse the NotificationFlags structure.
        /// </summary>
        protected override void Parse()
        {
            Flags = ParseT<NotificationTypes>();
        }

        protected override void ParseBlocks()
        {
            Text = "NotificationFlags";
            if (Flags != null)
            {
                AddChildBlockT(Flags, "NotificationType");
                if (T) Flags.AddHeader("(T) Message Count Changed");
                if (U) Flags.AddHeader("(U) Unread Count Changed");
                if (S) Flags.AddHeader("(S) Search Folder Event");
                if (M) Flags.AddHeader("(M) Message Event");
            }
        }
    }
}
