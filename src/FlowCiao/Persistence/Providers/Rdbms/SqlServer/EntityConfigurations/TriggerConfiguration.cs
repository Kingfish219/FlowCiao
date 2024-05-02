using FlowCiao.Models.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace FlowCiao.Persistence.Providers.Rdbms.SqlServer.EntityConfigurations;

internal class TriggerConfiguration : IEntityTypeConfiguration<Trigger>
{
    public void Configure(EntityTypeBuilder<Trigger> builder)
    {
        builder.ToTable("Trigger")
            .Property(x => x.Id)
            .HasValueGenerator<SequentialGuidValueGenerator>();
    }
}