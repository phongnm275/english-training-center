namespace EnglishTrainingCenter.Common.Utilities;

/// <summary>
/// Pagination result wrapper
/// </summary>
/// <typeparam name="T">Data type</typeparam>
public class PagedResult<T>
{
    /// <summary>
    /// Items in current page
    /// </summary>
    public IEnumerable<T> Items { get; set; } = new List<T>();

    /// <summary>
    /// Total count of items
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Current page number
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Page size
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total pages
    /// </summary>
    public int TotalPages => (TotalCount + PageSize - 1) / PageSize;

    /// <summary>
    /// Has previous page
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>
    /// Has next page
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;
}

/// <summary>
/// API response wrapper
/// </summary>
/// <typeparam name="T">Data type</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Success flag
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// HTTP status code
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Response message
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Response data
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Error details
    /// </summary>
    public IEnumerable<string> Errors { get; set; } = new List<string>();

    public static ApiResponse<T> SuccessResponse(T data, string message = "Operation successful", int statusCode = 200)
    {
        return new ApiResponse<T>
        {
            Success = true,
            StatusCode = statusCode,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> ErrorResponse(string message, int statusCode = 400, IEnumerable<string>? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            StatusCode = statusCode,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }
}

/// <summary>
/// Simple API response (non-generic)
/// </summary>
public class ApiResponse
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public IEnumerable<string> Errors { get; set; } = new List<string>();

    public static ApiResponse SuccessResponse(string message = "Operation successful", int statusCode = 200)
    {
        return new ApiResponse
        {
            Success = true,
            StatusCode = statusCode,
            Message = message
        };
    }

    public static ApiResponse ErrorResponse(string message, int statusCode = 400, IEnumerable<string>? errors = null)
    {
        return new ApiResponse
        {
            Success = false,
            StatusCode = statusCode,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }
}
