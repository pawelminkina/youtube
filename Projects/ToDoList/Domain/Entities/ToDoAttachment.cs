namespace Domain.Entities;

public class ToDoAttachment
{
    public ToDoAttachment()
    {
        Path = string.Empty;
    }
    public Guid Id { get; set; }
    public string Path { get; set; }
    public ToDoItem ToDoItem { get; set; } = null!;
}