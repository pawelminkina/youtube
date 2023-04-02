namespace Application.Models;

public class ToDoItemDto
{
    public ToDoItemDto()
    {
        Attachments = new List<AttachmentDto>();
    }
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public IEnumerable<AttachmentDto> Attachments { get; set; } 
}