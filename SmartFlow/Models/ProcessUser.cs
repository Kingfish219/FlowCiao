using System;
using Dapper.FluentMap.Mapping;

namespace SmartFlow.Models
{
    public class ProcessUser
    {
        public const string CRMProcessUser = "CRMProcessUser";

        public Guid Id { get; set; }
        public string Name { get; set; }

        public static ProcessUser GetCrmProcessUser()
        {
            return new ProcessUser
            {
                Id = Guid.NewGuid(),
                Name = CRMProcessUser
            };
        }
    }

    internal class UserMap : EntityMap<ProcessUser>
    {
        internal UserMap()
        {
            Map(x => x.Id).ToColumn("UserId");
        }
    }
}
