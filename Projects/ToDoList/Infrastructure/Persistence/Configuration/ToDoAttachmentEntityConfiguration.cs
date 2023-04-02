using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class ToDoAttachmentEntityConfiguration : IEntityTypeConfiguration<ToDoAttachment>
{
    public void Configure(EntityTypeBuilder<ToDoAttachment> builder)
    {
        builder.HasKey(s => s.Id).IsClustered(false);
    }
}