using FlowCiao.IntegrationTests.Fixtures;
using FlowCiao.IntegrationTests.TestUtils.Models;
using FlowCiao.Interfaces.Persistence;
using FlowCiao.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowCiao.IntegrationTests.Services
{
    public class StateServiceTests : IClassFixture<ServiceProviderFixture>
    {
        private readonly IStateRepository _stateRepository;
        private readonly IActivityService _activityService;

        public StateServiceTests(ServiceProviderFixture serviceProviderFixture)
        {
            _stateRepository = serviceProviderFixture.ServiceProvider.GetService<IStateRepository>();
            _activityService = serviceProviderFixture.ServiceProvider.GetService<IActivityService>();
        }

        [Fact]
        public async Task ModifyAsync_ShouldWork()
        {
            var state = States.Idle;
            if (state.Activities is null || state.Activities.Count == 0)
            {
                foreach (var activity in state.Activities)
                {
                    var activityResult = await _activityService.Modify(activity);
                    if (activityResult == default)
                    {
                        Assert.Fail("Modifying Activity failed");
                    }
                }
            }

            var result = await _stateRepository.Modify(state);

            Assert.NotEqual(result, default);

        }

    }
}

