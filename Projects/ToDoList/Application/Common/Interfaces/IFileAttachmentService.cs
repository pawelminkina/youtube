using Application.Models;

namespace Application.Common.Interfaces;

public interface IFileAttachmentService
{
    public Task<AttachmentFileDto> GetAttachmentReferenceAsync(string path, CancellationToken ct);
    public Task<string> AddAttachmentAsync(AttachmentToAdd attachment, CancellationToken ct);
    public Task RemoveAttachmentIfExistAsync(string name, CancellationToken ct);
}