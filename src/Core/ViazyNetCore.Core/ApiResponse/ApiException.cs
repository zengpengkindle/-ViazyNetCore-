using static Microsoft.AspNetCore.Http.StatusCodes;

namespace ViazyNetCore
{
    public class ApiException : System.Exception
    {
        public int StatusCode { get; set; }

        public ApiException(string message, int statusCode = Status400BadRequest) : base(message)
        {
            StatusCode = statusCode;
        }

        public ApiException(int statusCode = Status400BadRequest)
        {
            StatusCode = statusCode;
        }

        public ApiException(System.Exception ex, int statusCode = Status500InternalServerError) : base(ex.Message)
        {
            StatusCode = statusCode;
        }
    }
}
