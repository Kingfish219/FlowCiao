using FlowCiao.Models.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowCiao.Persistence.Providers.Rdbms.SqlServer.EntityConfigurations;

internal class TransitionActivityConfiguration : IEntityTypeConfiguration<TransitionActivity>
{
    public void Configure(EntityTypeBuilder<TransitionActivity> builder)
    {
        builder.HasKey(x => new { x.TransitionId, x.ActivityId });
    }
}
