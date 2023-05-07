using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configuration;

public class SecondSampleEntityConfiguration : IEntityTypeConfiguration<SecondSample>
{
    public void Configure(EntityTypeBuilder<SecondSample> builder)
    {
        builder.HasKey(s => s.Id).IsClustered(false);
    }
}