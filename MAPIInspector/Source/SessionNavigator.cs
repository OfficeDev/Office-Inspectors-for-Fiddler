using System;
using System.Collections;
using System.Collections.Generic;
using Fiddler;

namespace MapiInspector
{
    /// <summary>
    /// Provides fast navigation of Session objects by their id property.
    /// Allows retrieval of previous, next, and range of sessions based on their id.
    /// </summary>
    public class SessionNavigator : IEnumerable<Session>
    {
        /// <summary>
        /// Array of sessions in their input order.
        /// </summary>
        private readonly Session[] sessions;

        /// <summary>
        /// Maps session id to its index in the sessions array.
        /// </summary>
        private readonly Dictionary<int, int> idToIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionNavigator"/> class.
        /// Builds navigation structures based on Session.id.
        /// </summary>
        /// <param name="sessions">Array of Session objects to navigate.</param>
        public SessionNavigator(Session[] sessions)
        {
            sessions = sessions ?? FiddlerApplication.UI.GetAllSessions();

            if (sessions.Length == 0)
            {
                this.sessions = new Session[0];
                idToIndex = new Dictionary<int, int>();
                return;
            }

            // Sort sessions by their id property to ensure they are in order.
            Array.Sort(sessions, (p1, p2) => p1.id.CompareTo(p2.id));

            this.sessions = new Session[sessions.Length];
            idToIndex = new Dictionary<int, int>();

            for (int i = 0; i < sessions.Length; i++)
            {
                var s = sessions[i];
                if (s != null)
                {
                    this.sessions[i] = s;
                    idToIndex[s.id] = i;
                }
            }
        }

        /// <summary>
        /// Gets the previous session in the order relative to the specified session.
        /// </summary>
        /// <param name="current">The current session.</param>
        /// <returns>The previous session, or null if not found.</returns>
        public Session Previous(Session current)
        {
            if (current == null)
                return null;
            if (!idToIndex.TryGetValue(current.id, out int idx))
                return null;
            if (idx > 0)
                return sessions[idx - 1];
            return null;
        }

        /// <summary>
        /// Gets the next session in the order relative to the specified session.
        /// </summary>
        /// <param name="current">The current session.</param>
        /// <returns>The next session, or null if not found.</returns>
        public Session Next(Session current)
        {
            if (current == null)
                return null;
            if (!idToIndex.TryGetValue(current.id, out int idx))
                return null;
            if (idx < sessions.Length - 1)
                return sessions[idx + 1];
            return null;
        }

        /// <summary>
        /// Retrieves the first session from the list of sessions.
        /// </summary>
        /// <returns>The first <see cref="Session"/> object in the list, or <see langword="null"/> if the list is empty.</returns>
        public Session First()
        {
            if (sessions.Length > 0)
            {
                return sessions[0];
            }

            return null;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the sessions.
        /// </summary>
        public IEnumerator<Session> GetEnumerator()
        {
            for (int i = 0; i < sessions.Length; i++)
            {
                yield return sessions[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}