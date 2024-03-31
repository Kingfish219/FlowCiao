using FlowCiao.Models.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowCiao.Persistence.Providers.Rdbms.SqlServer.EntityConfigurations;

internal class StateActivityConfiguration : IEntityTypeConfiguration<StateActivity>
{
    public void Configure(EntityTypeBuilder<StateActivity> builder)
    {
        builder.HasKey(x => new { x.StateId, x.ActivityId });
    }
}
