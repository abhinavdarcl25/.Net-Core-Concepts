using System.Net;

namespace CollegeApp_API.Models
{
    public class CommonApiResponse
    {
        public bool Success { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public dynamic Data { get; set; }
        public List<string> Errors { get; set; }
    }
}
