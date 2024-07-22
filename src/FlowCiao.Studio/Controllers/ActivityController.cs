using FlowCiao.Interfaces.Services;
using FlowCiao.Services;
using FlowCiao.Studio.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlowCiao.Studio.Controllers;

[Route("flowciao/api/activity")]
public class ActivityController : FlowCiaoControllerBase
{
    private readonly IActivityService _activityService;

    public ActivityController(IActivityService activityService)
    {
        _activityService = activityService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Get()
    {
        var result = await _activityService.Get();

        return Ok(new ApiResponse(result));
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterActivityAssembly(IFormFile file)
    {
        if (file is null)
        {
            return BadRequest();
        }

        if (file.Length <= 0)
        {
            return BadRequest();
        }

        await using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        var fileBytes = ms.ToArray();
        var result = await _activityService.RegisterActivity(file.FileName, fileBytes);

        return Ok(new ApiResponse(result));
    }
}