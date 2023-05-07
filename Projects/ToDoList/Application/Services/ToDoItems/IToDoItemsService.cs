using Application.Models;

namespace Application.Services.ToDoItems;

public interface IToDoItemsService
{
    public Task<ToDoItemDto> GetAsync(Guid id, CancellationToken ct);
    public IAsyncEnumerable<ToDoItemDto> GetAllAsync(CancellationToken ct);
    public Task<Guid> AddAsync(ToDoItemToAdd toDoItem, IEnumerable<AttachmentInFileSystem> attachments, CancellationToken ct);
    public Task DeleteAsync(Guid id, CancellationToken ct);
    public Task ChangeStatusAsync(Guid id, CancellationToken ct);
    public Task PublishAllToDoItems(CancellationToken ct);
}