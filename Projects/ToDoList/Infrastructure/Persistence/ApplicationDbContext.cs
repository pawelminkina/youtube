using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<ToDoItem> ToDoItems => Set<ToDoItem>();
    public DbSet<ToDoAttachment> ToDoAttachments => Set<ToDoAttachment>();
}