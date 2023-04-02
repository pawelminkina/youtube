namespace Application.Models;

public class AttachmentToAdd
{
    public string Name { get; set; } = string.Empty;
    public Stream Content { get; set; } = null!;
}