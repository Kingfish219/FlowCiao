using FlowCiao.Models.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace FlowCiao.Persistence.Providers.Rdbms.SqlServer.EntityConfigurations;

internal class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.ToTable("Activity")
            .Property(x => x.Id)
            .HasValueGenerator<SequentialGuidValueGenerator>();

        builder.HasIndex(a => a.ActorName).IsUnique();

        builder.HasMany(e => e.Transitions)
            .WithMany(e => e.Activities)
            .UsingEntity<TransitionActivity>(
                l => l.HasOne(e => e.Transition).WithMany(e => e.TransitionActivities),
                r => r.HasOne(e => e.Activity).WithMany(e => e.TransitionActivities));
    }
}