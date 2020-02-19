using RoadStatus.Service.Models;
using System;

namespace RoadStatus.Service.Exceptions
{
    [Serializable]
    public class ApiException : Exception
    {
        public int StatusCode { get; set; }
        public ApiErrorResponse Error { get; set; }
    }
}