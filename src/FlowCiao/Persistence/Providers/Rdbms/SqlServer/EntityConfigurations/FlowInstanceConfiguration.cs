using FlowCiao.Models.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace FlowCiao.Persistence.Providers.Rdbms.SqlServer.EntityConfigurations;

internal class FlowInstanceConfiguration : IEntityTypeConfiguration<FlowInstance>
{
    public void Configure(EntityTypeBuilder<FlowInstance> builder)
    {
        builder.ToTable("FlowInstance")
            .Property(x => x.Id)
            .HasValueGenerator<SequentialGuidValueGenerator>();
    }
}