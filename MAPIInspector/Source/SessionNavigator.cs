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
        private Session[] inputSessions;
        private Session[] sessions;
        private Dictionary<int, int> idToIndex;

        /// <summary>
        /// Updates the session list and reinitializes the navigation structures.
        /// </summary>
        /// <param name="newSessions">The new array of Session objects.</param>
        public void Init(Session[] newSessions = null)
        {
            inputSessions = newSessions;
            sessions = null;
            idToIndex = null;
        }

        private void EnsureInitialized()
        {
            if (sessions != null && idToIndex != null)
                return;

            var source = inputSessions ?? FiddlerApplication.UI.GetAllSessions();
            if (source == null || source.Length == 0)
            {
                sessions = new Session[0];
                idToIndex = new Dictionary<int, int>();
                return;
            }

            Array.Sort(source, (p1, p2) => p1.id.CompareTo(p2.id));
            sessions = new Session[source.Length];
            idToIndex = new Dictionary<int, int>();
            for (int i = 0; i < source.Length; i++)
            {
                var s = source[i];
                if (s != null)
                {
                    sessions[i] = s;
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
            EnsureInitialized();
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
            EnsureInitialized();
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
            EnsureInitialized();
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
            EnsureInitialized();
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