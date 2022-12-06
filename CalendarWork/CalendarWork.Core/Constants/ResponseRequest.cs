using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CalendarWork.Core.Constants
{
    public class ResponseRequest
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}
