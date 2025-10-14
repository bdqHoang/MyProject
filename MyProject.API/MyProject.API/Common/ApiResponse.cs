namespace MyProject.API.Common
{
    public class ApiResponse<T>
    {
        public int Code { get; set; }
        public required string Message { get; set; }
        public required T Data { get; set; }
        public bool Success { get; set; }

        /// <summary>
        /// success response
        /// </summary>
        /// <param name="data"> Return data </param>
        /// <param name="message"> Message config </param>
        /// <param name="code"> Code config </param>
        /// <returns></returns>
        public static ApiResponse<T> SuccessResponse(T data, string message = "Success", int code = 200)
        {
            return new ApiResponse<T>
            {
                Code = code,
                Message = message,
                Data = data,
                Success = true
            };
        }

        /// <summary>
        /// Error response
        /// </summary>
        /// <param name="message"> Message config</param>
        /// <param name="code"> Code config</param>
        /// <returns></returns>
        public static ApiResponse<T> ErrorResponse(string message = "Failure", int code = 400)
        {
            return new ApiResponse<T>
            {
                Code = code,
                Message = message,
                Data = default!,
                Success = false
            };
        }

    }
}
