using FlowCiao.Models.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowCiao.Persistence.Providers.Rdbms.SqlServer.EntityConfigurations;

internal class TransitionTriggerConfiguration : IEntityTypeConfiguration<TransitionTrigger>
{
    public void Configure(EntityTypeBuilder<TransitionTrigger> builder)
    {
        builder.HasKey(x => new { x.TransitionId, x.TriggerId });
    }
}
