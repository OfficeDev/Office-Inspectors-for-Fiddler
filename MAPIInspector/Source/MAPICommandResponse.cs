using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using VisualSync;

namespace VisualSync
{
    class MAPICommandResponse
    {
        private byte[] httpBytes = null;
        private string payloadString = null;
        
        public string PayloadString
        {
            get
            {
                return payloadString;
            }
        }
        public MAPICommandResponse(byte[] httpPayload)
        {
            httpBytes = httpPayload;
            payloadString = httpBytes.ToString();
        }
    }
}
