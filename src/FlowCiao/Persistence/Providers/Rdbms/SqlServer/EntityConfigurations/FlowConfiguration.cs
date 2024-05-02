using FlowCiao.Models.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace FlowCiao.Persistence.Providers.Rdbms.SqlServer.EntityConfigurations;

internal class FlowConfiguration : IEntityTypeConfiguration<Flow>
{
    public void Configure(EntityTypeBuilder<Flow> builder)
    {
        builder.ToTable("Flow")
            .Property(x => x.Id)
            .HasValueGenerator<SequentialGuidValueGenerator>();
    }
}