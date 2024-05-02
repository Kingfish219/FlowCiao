using FlowCiao.Models.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace FlowCiao.Persistence.Providers.Rdbms.SqlServer.EntityConfigurations;

internal class TransitionConfiguration : IEntityTypeConfiguration<Transition>
{
    public void Configure(EntityTypeBuilder<Transition> builder)
    {
        builder.ToTable("Transition")
            .Property(x => x.Id)
            .HasValueGenerator<SequentialGuidValueGenerator>();
    }
}