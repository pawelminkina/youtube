using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IPublishToDoService
{
    Task PublishToDoAsync(ToDoItem item);
}