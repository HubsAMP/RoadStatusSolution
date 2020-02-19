using System;

namespace RoadStatus.Service.Models
{
    public class ApiErrorResponse
    {
        public string Type { get; set; }
        public DateTime TimestampUtc { get; set; }
        public string ExceptionType { get; set; }
        public int HttpStatusCode { get; set; }
        public string HttpStatus { get; set; }
        public string RelativeUri { get; set; }
        public string Message { get; set; }
    }
}