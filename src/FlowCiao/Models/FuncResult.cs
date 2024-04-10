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

        public bool Success { get; }
        public int Code { get; }
        public string Title { get; }
        public string Message { get; }
        public int StatusCode { get; }
    }

    public class FuncResult<T>
    {
        public FuncResult(bool success, string message = default, int code = default, string title = default,
            int statusCode = default, T data = default)
        {
            Success = success;
            Code = code;
            Title = title;
            Message = message;
            StatusCode = statusCode;
            Data = data;
        }
        
        public bool Success { get; }
        public int Code { get; }
        public string Title { get; }
        public string Message { get; }
        public int StatusCode { get; }
        public T Data { get; }
    }
}