using FlowCiao.Models.Core;
using FlowCiao.Models.Dto;
using FlowCiao.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlowCiao.Studio.Controllers;

[Route("flowciao/api/activity")]
public class ActivityController : FlowCiaoApiControllerBase
{
    private readonly ActivityService _activityService;

    public ActivityController(ActivityService activityService)
    {
        _activityService = activityService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Get()
    {
        var result = await _activityService.Get();

        return Ok(result);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterActivityAssembly(IFormFile? file)
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
        var result = await _activityService.RegisterActivity(new ActivityAssembly(file.FileName, fileBytes));

        return Ok(result);
    }
}