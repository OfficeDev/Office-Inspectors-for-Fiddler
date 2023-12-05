namespace MapiInspector
{
    using Fiddler;

    /// <summary>
    /// MAPI Request inspector derived from MAPIInspector and implemented Fiddler.IResponseInspector2
    /// </summary>
    public class MAPIRequestInspector : MAPIInspector, IRequestInspector2
    {
        /// <summary>
        /// Gets or sets the request headers from the frame
        /// </summary>
        public HTTPRequestHeaders headers
        {
            get
            {
                return MAPIParser.BaseHeaders as HTTPRequestHeaders;
            }

            set
            {
                MAPIParser.BaseHeaders = value;
            }
        }
    }
}