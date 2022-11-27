using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eshop_pbl6.Helpers.Common
{
    public class CommonReponse
    {
        public class ResponseClient
        {
            public string code { get; set; } = string.Empty;
            public string message { get; set; } = string.Empty;
            public object result { get; set; }
            public string timestamp { get; set; } = DateTime.Now.ToString();
            public ResponseClient() { }
            public ResponseClient(ResponseCodes responseCode, string message, object result)
            {
                this.code = ((int)responseCode).ToString();
                this.message = message;
                this.result = result;
            }

        }
        public static ResponseClient CreateResponse(ResponseCodes responseCode, string message, object result)
        {
            ResponseClient responseClient = new ResponseClient();
            responseClient.code = responseCode.ToString().ToLower();
            responseClient.message = message;
            responseClient.result = result;
            return responseClient;
        }
    }

}