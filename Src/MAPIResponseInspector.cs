using Fiddler;

namespace MapiInspector
{
    public class MAPIResponseInspector : MAPIInspector, IResponseInspector2
    {        
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