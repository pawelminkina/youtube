using Application.Common;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Models;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Infrastructure.Files;

public class BlobFileAttachmentService : IFileAttachmentService
{
    private readonly BlobContainerClient _blobContainerClient;
    public BlobFileAttachmentService(IConfiguration configuration)
    {
        _blobContainerClient =
            new BlobContainerClient(
                configuration.GetConnectionString(AppConfig.AttachmentsFileStorageConnectionStringName),
                configuration.GetValue<string>(AppConfig.AttachmentsFileContainerName));
    }

    public async Task<AttachmentFileDto> GetAttachmentReferenceAsync(string path, CancellationToken ct)
    {
        var fileClient = _blobContainerClient.GetBlobClient(path);
        if (!await fileClient.ExistsAsync(ct))
        {
            throw new NotFoundException($"Attachment with given path: {path} was not found in blob storage");
        }

        var props = await fileClient.GetPropertiesAsync(cancellationToken: ct);
        return new AttachmentFileDto() {Name = fileClient.Name, SizeInBytes = props.Value.ContentLength};
    }

    public async Task<string> AddAttachmentAsync(AttachmentToAdd attachment, CancellationToken ct)
    {
        var newName = $"{Guid.NewGuid()}_{attachment.Name}";

        var uploadResult = await _blobContainerClient.UploadBlobAsync(newName, attachment.Content, ct);

        var response = uploadResult.GetRawResponse();
        if (response.IsError)
        {
            throw new InvalidOperationException($"There was issue with uploading attachment with name: {newName}, status: {response.Status}");
        }

        return newName;
    }

    public async Task RemoveAttachmentIfExistAsync(string name, CancellationToken ct)
    {
        var fileClient = _blobContainerClient.GetBlobClient(name);
        if (await fileClient.ExistsAsync(ct))
        {
            await fileClient.DeleteAsync(cancellationToken:ct);
        }
    }
}