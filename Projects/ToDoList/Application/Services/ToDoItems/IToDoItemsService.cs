using Application.Models;

namespace Application.Services.ToDoItems;

public interface IToDoItemsService
{
    public Task<ToDoItemDto> GetAsync(Guid id, CancellationToken ct);
    public IAsyncEnumerable<ToDoItemDto> GetAllAsync(CancellationToken ct);
    public Task<Guid> AddAsync(ToDoItemToAdd toDoItem, IEnumerable<AttachmentToAdd> attachments, CancellationToken ct);
    public Task DeleteAsync(Guid id, CancellationToken ct);
    public Task ChangeNameAsync(Guid id, string newName, CancellationToken ct);
}