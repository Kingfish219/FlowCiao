namespace FlowCiao.Models
{
    public class FuncResult
    {
        public FuncResult(bool success, string message = default, int code = default)
        {
            Success = success;
            Code = code;
            Message = message;
        }

        public bool Success { get; }
        public int Code { get; }
        public string Message { get; }
    }

    public class FuncResult<T> : FuncResult
    {
        public FuncResult(bool success, string message = default, int code = default, T data = default)
            : base(success, message, code)
        {
            Data = data;
        }

        public T Data { get; }
    }
}