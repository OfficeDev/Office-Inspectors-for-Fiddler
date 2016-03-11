using Fiddler;

namespace MapiInspector
{

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
                return this.BaseHeaders as HTTPRequestHeaders;
            }

            set
            {
                this.BaseHeaders = value;
            }
        }
    }
}