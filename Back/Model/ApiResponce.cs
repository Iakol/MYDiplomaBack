using System.Net;

namespace Back.Model
{
    public class ApiResponce
    {
        public ApiResponce() {
            ErrorsMessage = new List<string>();
        }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> ErrorsMessage { get; set; }
        public object Result { get; set; }
    }
}
