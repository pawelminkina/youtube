using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class ToDoItemEntityConfiguration : IEntityTypeConfiguration<ToDoItem>
{
    public void Configure(EntityTypeBuilder<ToDoItem> builder)
    {
        builder.HasKey(s => s.Id).IsClustered(false);

        builder.Property(s => s.Name).HasMaxLength(1000).IsRequired();

        builder.HasMany(s => s.Attachments).WithOne(f => f.ToDoItem);

        builder.HasMany(s => s.Samples).WithOne(f => f.ToDo);
    }
}