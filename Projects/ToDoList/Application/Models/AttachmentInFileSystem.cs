using MimeTypes;

namespace Application.Models;

public class AttachmentInFileSystem
{
    public string MimeType => MimeTypeMap.GetMimeType(Path.GetExtension(Name));
    public string Name { get; set; } = string.Empty;
    public Stream Content { get; set; } = null!;
}