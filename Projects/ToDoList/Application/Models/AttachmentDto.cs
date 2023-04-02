namespace Application.Models;

public class AttachmentDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string SizeInMb { get; set; } = string.Empty;
}