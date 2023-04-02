using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ToDoItem> ToDoItems { get; }
    DbSet<ToDoAttachment> ToDoAttachments { get; }
    public Task<int> SaveChangesAsync(CancellationToken ct = default);
}