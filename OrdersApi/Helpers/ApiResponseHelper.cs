using OrdersApi.Dtos;

namespace OrdersApi.Helpers
{
    /// <summary>
    /// Api response helper methods.
    /// </summary>
    public static class ApiResponseHelper
    {
        /// <summary>
        /// Create an error Api Response.
        /// </summary>        
        public static ApiResponse<T> Failure<T>(string message, IEnumerable<string> errors )
        {
            return new ApiResponse<T>() 
            { 
                Success = false, 
                Message = message,
                Errors = errors 
            };            
        }

        /// <summary>
        /// Create a success Api Response.
        /// </summary>
        public static ApiResponse<T> Success<T>(string message, T data)
        {
            return new ApiResponse<T>()
            {
                Success = true,
                Message = message,
                Data = data                
            };
        }
    }
}
