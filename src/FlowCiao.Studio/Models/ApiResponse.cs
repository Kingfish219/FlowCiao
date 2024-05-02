using FlowCiao.Models;

namespace FlowCiao.Studio.Models;

internal class ApiResponse
{
    public ApiResponse(ApiResponseStatus status = ApiResponseStatus.Success, object data = null, string message = null, object error = null)
    {
        Data = data;
        Status = status == ApiResponseStatus.Success ? "success" : "error";
        Message = message;
        Error = error;
    }
    
    public ApiResponse(object data)
    {
        Data = data;
        Status = "success";
    }
    
    public ApiResponse(FuncResult funcResult, object data = null)
    {
        Status = GenerateStatus(funcResult.Success ? ApiResponseStatus.Success : ApiResponseStatus.Error);
        Message = funcResult.Message;
        Data = data;
    }

    public string Status { get; }
    public object Data { get; }
    public string Message { get; }
    public object Error { get; }

    private static string GenerateStatus(ApiResponseStatus status)
    {
        return status == ApiResponseStatus.Success ? "success" : "error";
    }
    
    public enum ApiResponseStatus
    {
        Success,
        Error
    }
}
