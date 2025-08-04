using System;

namespace MAPIInspector.Parsers
{
    public static class Guids
    {
        public static Guid PSETID_Meeting = new Guid("6ED8DA90-450B-101B-98DA-00AA003F1305");
        public static Guid PSETID_CalendarAssistant = new Guid("11000E07-B51B-40D6-AF21-CAA85EDAB1D0");
        public static Guid PSETID_Appointment = new Guid("00062002-0000-0000-C000-000000000046");
        public static Guid PSETID_Address = new Guid("00062004-0000-0000-C000-000000000046");
        public static Guid PSETID_Task = new Guid("00062003-0000-0000-C000-000000000046");
        public static Guid PSETID_Common = new Guid("00062008-0000-0000-C000-000000000046");
        public static Guid PSETID_Log = new Guid("0006200A-0000-0000-C000-000000000046");
        public static Guid PSETID_PostRss = new Guid("00062041-0000-0000-C000-000000000046");
        public static Guid PSETID_Sharing = new Guid("00062040-0000-0000-C000-000000000046");
        public static Guid PSETID_Note = new Guid("0006200E-0000-0000-C000-000000000046");
        public static Guid PS_PUBLIC_STRINGS = new Guid("00020329-0000-0000-C000-000000000046");

        public static string Name(this Guid guid)
        {
            // compare to each known guid and return the name
            if (guid == PSETID_Meeting) return "PSETID_Meeting";
            if (guid == PSETID_CalendarAssistant) return "PSETID_CalendarAssistant";
            if (guid == PSETID_Appointment) return "PSETID_Appointment";
            if (guid == PSETID_Address) return "PSETID_Address";
            if (guid == PSETID_Task) return "PSETID_Task";
            if (guid == PSETID_Common) return "PSETID_Common";
            if (guid == PSETID_Log) return "PSETID_Log";
            if (guid == PSETID_PostRss) return "PSETID_PostRss";
            if (guid == PSETID_Sharing) return "PSETID_Sharing";
            if (guid == PSETID_Note) return "PSETID_Note";
            if (guid == PS_PUBLIC_STRINGS) return "PS_PUBLIC_STRINGS";
            return null;
        }

        public static string ToString(this Guid guid)
        {
            var name = Name(guid);
            return name != null ? $"{name} = {guid}" : guid.ToString();
        }
    }
}
