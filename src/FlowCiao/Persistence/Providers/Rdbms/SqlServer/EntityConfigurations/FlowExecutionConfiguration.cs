using FlowCiao.Models.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace FlowCiao.Persistence.Providers.Rdbms.SqlServer.EntityConfigurations;

internal class FlowExecutionConfiguration : IEntityTypeConfiguration<FlowExecution>
{
    public void Configure(EntityTypeBuilder<FlowExecution> builder)
    {
        builder.ToTable("FlowExecution")
            .Property(x => x.Id)
            .HasValueGenerator<SequentialGuidValueGenerator>();
    }
}