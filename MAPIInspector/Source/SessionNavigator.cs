using System.Collections;
using System.Collections.Generic;
using Fiddler;

namespace MapiInspector
{
    /// <summary>
    /// Provides fast navigation of Session objects by their "Number" property.
    /// Allows retrieval of previous, next, and range of sessions based on their number.
    /// </summary>
    public class SessionNavigator : IEnumerable<Session>
    {
        /// <summary>
        /// Array of sessions sorted by their "Number" property.
        /// </summary>
        private readonly Session[] sortedSessions;

        /// <summary>
        /// Maps session number to its index in the sortedSessions array.
        /// </summary>
        private readonly Dictionary<int, int> numberToIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionNavigator"/> class.
        /// Sorts the provided sessions by their "Number" property and builds navigation structures.
        /// </summary>
        /// <param name="sessions">Array of Session objects to navigate.</param>
        public SessionNavigator(Session[] sessions)
        {
            if (sessions.Length > 0 && sessions[sessions.Length - 1]["Number"] == null)
            {
                for (int i = 0; i < sessions.Length; i++)
                {
                    sessions[i]["Number"] = i.ToString();
                }
            }

            var tempDict = new SortedDictionary<int, Session>();
            numberToIndex = new Dictionary<int, int>();

            if (sessions == null || sessions.Length == 0)
            {
                sortedSessions = new Session[0];
                return;
            }

            // Collect valid sessions and their numbers into a sorted dictionary
            for (int i = 0; i < sessions.Length; i++)
            {
                var s = sessions[i];
                if (s != null)
                {
                    var numberValue = s["Number"];
                    if (numberValue != null)
                    {
                        if (int.TryParse(numberValue, out int num))
                        {
                            // Only add if not already present (avoid duplicate numbers)
                            if (!tempDict.ContainsKey(num))
                                tempDict[num] = s;
                        }
                    }
                }
            }

            // Build sorted array and number-to-index map from sorted dictionary
            sortedSessions = new Session[tempDict.Count];
            int idx = 0;
            foreach (var kvp in tempDict)
            {
                sortedSessions[idx] = kvp.Value;
                numberToIndex[kvp.Key] = idx;
                idx++;
            }
        }

        /// <summary>
        /// Gets the previous session in the sorted order relative to the specified session.
        /// </summary>
        /// <param name="current">The current session.</param>
        /// <returns>The previous session, or null if not found.</returns>
        public Session Previous(Session current)
        {
            if (current == null || current["Number"] == null)
                return null;
            if (!int.TryParse(current["Number"], out int num))
                return null;
            if (!numberToIndex.TryGetValue(num, out int idx))
                return null;
            if (idx > 0)
                return sortedSessions[idx - 1];
            return null;
        }

        /// <summary>
        /// Gets the next session in the sorted order relative to the specified session.
        /// </summary>
        /// <param name="current">The current session.</param>
        /// <returns>The next session, or null if not found.</returns>
        public Session Next(Session current)
        {
            if (current == null || current["Number"] == null)
                return null;
            if (!int.TryParse(current["Number"], out int num))
                return null;
            if (!numberToIndex.TryGetValue(num, out int idx))
                return null;
            if (idx < sortedSessions.Length - 1)
                return sortedSessions[idx + 1];
            return null;
        }

        /// <summary>
        /// Retrieves the first session from the sorted list of sessions.
        /// </summary>
        /// <returns>The first <see cref="Session"/> object in the sorted list, or <see langword="null"/> if the list is empty.</returns>
        public Session First()
        {
            if (sortedSessions.Length > 0)
            {
                return sortedSessions[0];
            }
            return null;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the sorted sessions.
        /// </summary>
        public IEnumerator<Session> GetEnumerator()
        {
            for (int i = 0; i < sortedSessions.Length; i++)
            {
                yield return sortedSessions[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}