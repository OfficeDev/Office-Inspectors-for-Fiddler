using Fiddler;

namespace MapiInspector
{
    public static class SessionExtensions
    {
        private static SessionNavigator _allSessionsNavigator;

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
        /// Global navigator instance to be set by the application. Lazily initializes on first access.
        /// </summary>
        public static SessionNavigator AllSessionsNavigator
        {
            get
            {
                return _allSessionsNavigator ?? (_allSessionsNavigator = new SessionNavigator());
            }
            private set
            {
                _allSessionsNavigator = value;
            }
        }

        /// <summary>
        /// Initializes the global AllSessionsNavigator singleton.
        /// </summary>
        /// <param name="sessions">Array of Session objects to navigate. If null, uses FiddlerApplication.UI.GetAllSessions().</param>
        public static void Init(Session[] sessions = null) => AllSessionsNavigator.Init(sessions);
    }
}
