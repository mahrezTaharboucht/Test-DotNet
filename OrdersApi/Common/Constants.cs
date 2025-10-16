namespace OrdersApi.Common
{
    public static class Constants
    {
        public const string ConnectionStringKey = "SqlLiteDbConnection";
        public const string ExceptionMiddleware = "Exception middleware";
        public const string GlobalExceptionMessage = "Internal service error.";
        public const string ValidationExceptionMessage = "Validation error.";
        public const string MissingOrderErrorMessage = "The order {0} not found.";
        public const string MissingOrderError = "Order not found.";
        public const string InvalidOrderIdErrorMessage = "The order Id should be greater than 0.";
        public const string ExistingOrderIdErrorMessage = "The order already exist.";
        public const string InvalidItemsErrorMessage = "Order items should contain at least one element.";
        public const string InvalidQuantityErrorMessage = "Item quantity should be greater than 0.";
        public const string InvalidProductTypeErrorMessage = "Unknown product type.";
        public const string EmptyProductTypeErrorMessage = "Product type should be provided.";
        public const string InvalidItemProductTypeErrorMessage = "The given product type value already exists.";
        public const string InvalidWidthErrorMessage = "Width should be greater than 0.";
        public const string InvalidNumberOfItemsInStackErrorMessage = "The number of items in stack should be greater than 0.";
    }
}
