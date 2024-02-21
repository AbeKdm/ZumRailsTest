
namespace ZumRailsPosts.API.Errors
{
    public class ApiException : ApiResponse
    {
        public ApiException(int statusCode, string message = null, string details = null, Guid transactionId = default(Guid)) : base(statusCode, message)
        {
            TransactionId = transactionId;
            Details = details;
        }
        public Guid TransactionId { get; private set; }
        public string Details { get; set; }
    }
}
