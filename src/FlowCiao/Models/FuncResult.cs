namespace FlowCiao.Models
{
    public class FuncResult
    {
        public FuncResult(bool success, string message = default, int code = default, string title = default,
            int statusCode = default)
        {
            Success = success;
            Code = code;
            Title = title;
            Message = message;
            StatusCode = statusCode;
        }

        public bool Success { get; init; }
        public int Code { get; init; }
        public string Title { get; init; }
        public string Message { get; init; }
        public int StatusCode { get; init; }
    }

    public class FuncResult<T>
    {
        public bool Success { get; set; } = true;
        public int Code { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public T Data { get; set; }
    }
}