using System.Reflection;
using FlowCiao.Exceptions;
using FlowCiao.IntegrationTests.Fixtures;
using FlowCiao.Interfaces.Persistence;
using FlowCiao.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FlowCiao.IntegrationTests.Services;

[Collection("Sequential")]
public class ActivityServiceTests : IClassFixture<ServiceProviderFixture>
{
    private readonly IActivityService _activityService;

    public ActivityServiceTests(ServiceProviderFixture serviceProviderFixture)
    {
        _activityService = serviceProviderFixture.ServiceProvider.GetService<IActivityService>();
    }

    [Fact]
    public async Task RegisterActivity_ShouldThrowException_WhenActorNameIsInvalid()
    {
        var actorName = "invalidFile.txt";
        var actorContent = Array.Empty<byte>();

        await Assert.ThrowsAsync<FlowCiaoException>(() => _activityService.RegisterActivity(actorName, actorContent));
    }

    [Fact]
    public async Task RegisterActivity_ShouldThrowException_WhenNoValidActivitiesFound()
    {
        var actorName = "valid.dll";
        var actorContent = Array.Empty<byte>();
        await Assert.ThrowsAsync<FlowCiaoException>(() => _activityService.RegisterActivity(actorName, actorContent));
    }

    [Fact]
    public async Task RegisterActivity_ShouldRegisterActivitiesSuccessfully()
    {
        var actorName = "FlowCiao.Samples.ActivityHub.dll";
        var actorContent = await File.ReadAllBytesAsync("TestUtils/Resource/FlowCiao.Samples.ActivityHub.dll");

        var result = await _activityService.RegisterActivity(actorName, actorContent);

        Assert.True(result.Success);
        Assert.Equal("Activities registered successfully", result.Message);
    }
}