﻿namespace MapiInspector
{
    using Fiddler;

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
                return MAPIParser.BaseHeaders as HTTPResponseHeaders;
            }

            set
            {
                MAPIParser.BaseHeaders = value;
            }
        }
    }
}