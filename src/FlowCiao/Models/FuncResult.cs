
namespace FlowCiao.Models
{
    public class FuncResult
    {
        public bool Success { get; set; } = true;
        public int Code { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
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
