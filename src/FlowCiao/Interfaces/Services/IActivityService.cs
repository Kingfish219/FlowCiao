using FlowCiao.Models;
using FlowCiao.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowCiao.Interfaces.Services
{
    public interface IActivityService
    {
        public Task<List<Activity>> Get(bool fetchActorContent = false);

        public Task<Activity> GetByKey(Guid id = default, string actorName = default);

        public Task<Guid> Modify(Activity activity);

        public Task<FuncResult> RegisterActivity(string actorName, byte[] actorContent);

        public Task<IFlowActivity> LoadActivity(string activityFileName);
    }
}
