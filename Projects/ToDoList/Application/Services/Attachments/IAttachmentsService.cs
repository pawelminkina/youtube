using Application.Models;

namespace Application.Services.Attachments;

public interface IAttachmentsService
{
    Task<AttachmentInFileSystem> GetAttachmentContentAsync(Guid id, CancellationToken ct);
    Task RemoveAttachmentAsync(Guid id, CancellationToken ct);
}