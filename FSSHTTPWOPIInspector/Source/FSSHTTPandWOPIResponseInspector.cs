using Fiddler;

namespace FSSHTTPandWOPIInspector
{
    /// <summary>
    /// FSSHTTPandWOPI Response inspector derived from FSSHTTPandWOPIInspector and implemented Fiddler.IResponseInspector2
    /// </summary>
    public class FSSHTTPandWOPIResponseInspector : FSSHTTPandWOPIInspector, IResponseInspector2
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