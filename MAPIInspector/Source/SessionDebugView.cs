using System.Diagnostics;
using Fiddler;

[assembly: DebuggerTypeProxy(typeof(MapiInspector.SessionDebugView), Target = typeof(Session))]
[assembly: DebuggerTypeProxy(typeof(MapiInspector.SessionArrayDebugView), Target = typeof(Session[]))]

namespace MapiInspector
{
    /// <summary>
    /// Debugger proxy for Fiddler.Session to improve Watch window display.
    /// </summary>
    internal class SessionDebugView
    {
        private readonly Session _session;
        public SessionDebugView(Session session)
        {
            _session = session;
        }
        public int Id => _session.id;
        public string RequestPath => _session.RequestHeaders?.RequestPath;
        public string LocalProcess => _session.LocalProcess;
        public string ClientInfo => _session.RequestHeaders?["X-ClientInfo"];
        public override string ToString() => $"Session #{Id.ToString()} ({LocalProcess})";
    }

    /// <summary>
    /// Debugger proxy for arrays of Fiddler.Session to improve Watch window display.
    /// </summary>
    internal class SessionArrayDebugView
    {
        private readonly Session[] _sessions;
        public SessionArrayDebugView(Session[] sessions)
        {
            _sessions = sessions;
        }
        public object[] Items
        {
            get
            {
                if (_sessions == null) return null;
                var items = new object[_sessions.Length];
                for (int i = 0; i < _sessions.Length; i++)
                {
                    var s = _sessions[i];
                    items[i] = s == null ? null : new SessionDebugView(s);
                }
                return items;
            }
        }
    }
}
