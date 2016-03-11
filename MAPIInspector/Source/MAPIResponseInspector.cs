using Fiddler;

namespace MapiInspector
{
    /// <summary>
    /// MAPI Response inspector derived from MAPIInspector and implemented Fiddler.IResponseInspector2
    /// </summary>
    public class MAPIResponseInspector : MAPIInspector, IResponseInspector2
    {        
        /// <summary>
        ///  Gets or sets the response headers from the frame
        /// </summary>
        public HTTPResponseHeaders headers
        {
            get
            {
                return this.BaseHeaders as HTTPResponseHeaders;
            }

            set
            {
                this.BaseHeaders = value;
            }
        }
    }
}