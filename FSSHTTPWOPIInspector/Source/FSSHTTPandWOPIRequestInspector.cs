using Fiddler;

namespace FSSHTTPandWOPIInspector
{

    /// <summary>
    /// FSSHTTPandWOPI Request inspector derived from FSSHTTPandWOPIInspector and implemented Fiddler.IResponseInspector2
    /// </summary>
    public class FSSHTTPandWOPIRequestInspector : FSSHTTPandWOPIInspector, IRequestInspector2
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