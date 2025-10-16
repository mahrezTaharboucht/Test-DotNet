namespace OrdersApi.Common
{
    public static class Constants
    {
        public const string ConnectionStringKey = "SqlLiteDbConnection";
        public const string ExceptionMiddleware = "Exception middleware";
        public const string GlobalExceptionMessage = "Internal service error.";
        public const string ValidationExceptionMessage = "Validation error.";

        public const string MissingOrderErrorMessage = "The order {orderId} not found.";
        public const string MissingOrderError = "Order not found.";
    }
}
