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
        
        builder.HasMany(e => e.Triggers)
            .WithMany(e => e.Transitions)
            .UsingEntity<TransitionTrigger>(
                l => l.HasOne(e => e.Trigger).WithMany(e => e.TransitionTriggers),
                r => r.HasOne(e => e.Transition).WithMany(e => e.TransitionTriggers));

        builder.Navigation(e => e.TransitionTriggers).AutoInclude();
    }
}