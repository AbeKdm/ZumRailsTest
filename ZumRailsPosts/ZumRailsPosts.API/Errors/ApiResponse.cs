
namespace ZumRailsPosts.API.Errors
{
    /// <summary>
    /// Represents a uniform API response with a status code and an optional message.
    /// </summary>
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Invalid request: malformed syntax or missing parameters.",
                404 => "Resource not found on server.",
                500 => $"Unexpected server error occurred",
                _ => null
            };
        }
    }
}
