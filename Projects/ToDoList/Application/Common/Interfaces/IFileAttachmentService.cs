using Application.Models;

namespace Application.Common.Interfaces;

public interface IFileAttachmentService
{
    Task<AttachmentFileDto> GetAttachmentReferenceAsync(string path, CancellationToken ct);
    Task<string> AddAttachmentAsync(AttachmentInFileSystem attachment, CancellationToken ct);
    Task RemoveAttachmentIfExistAsync(string path, CancellationToken ct);
    Task RemoveAttachmentAsync(string path, CancellationToken ct);
    Task<AttachmentInFileSystem> GetContent(string path, CancellationToken ct);
}