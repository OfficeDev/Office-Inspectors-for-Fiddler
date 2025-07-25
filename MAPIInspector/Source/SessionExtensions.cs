using Fiddler;
using System.Collections.Generic;

namespace MapiInspector
{
    public static class SessionExtensions
    {
        /// <summary>
        /// Gets the previous session using the global AllSessionsNavigator.
        /// </summary>
        public static Session Previous(this Session session)
        {
            return AllSessionsNavigator?.Previous(session);
        }

        /// <summary>
        /// Gets the next session using the global AllSessionsNavigator.
        /// </summary>
        public static Session Next(this Session session)
        {
            return AllSessionsNavigator?.Next(session);
        }

        /// <summary>
        /// Global navigator instance to be set by the application.
        /// </summary>
        public static SessionNavigator AllSessionsNavigator { get; set; }
    }
}
