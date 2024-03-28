using FlowCiao.Models.Flow;
using FlowCiao.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlowCiao.Studio.Controllers;

[Route("flowciao/api/activity")]
public class ActivityController : FlowCiaoApiControllerBase
{
    private readonly ILogger<ActivityController> _logger;
    private readonly IActivityRepository _activityRepository;

    public ActivityController(ILogger<ActivityController> logger, IActivityRepository activityRepository)
    {
        _logger = logger;
        _activityRepository = activityRepository;
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
        await _activityRepository.RegisterActivity(new ActivityAssembly(file.FileName, fileBytes));

        return Ok(new
        {
            Id = Guid.NewGuid()
        });
    }
}