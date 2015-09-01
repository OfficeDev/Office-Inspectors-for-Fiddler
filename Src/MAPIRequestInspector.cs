using Fiddler;

namespace MapiInspector
{
    public class MAPIRequestInspector : MAPIInspector, IRequestInspector2
    {
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