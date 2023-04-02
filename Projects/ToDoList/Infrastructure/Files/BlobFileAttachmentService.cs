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
        return new AttachmentFileDto() { Name = fileClient.Name, SizeInBytes = props.Value.ContentLength };
    }

    public async Task<string> AddAttachmentAsync(AttachmentInFileSystem attachment, CancellationToken ct)
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

    public async Task<AttachmentInFileSystem> GetContent(string path, CancellationToken ct)
    {
        var fileClient = _blobContainerClient.GetBlobClient(path);
        if (!await fileClient.ExistsAsync(ct))
        {
            throw new NotFoundException($"Attachment with path: {path} was not found is blob storage");
        }

        var downloadResult = await fileClient.DownloadStreamingAsync(cancellationToken: ct);

        var response = downloadResult.GetRawResponse();
        if (response.IsError)
        {
            throw new InvalidOperationException($"There was issue with downloading attachment in path: {path}, status: {response.Status}");
        }

        return new AttachmentInFileSystem() {Content = downloadResult.Value.Content, Name = path};

    }

    public async Task RemoveAttachmentIfExistAsync(string path, CancellationToken ct)
    {
        var fileClient = _blobContainerClient.GetBlobClient(path);
        if (await fileClient.ExistsAsync(ct))
        {
            await fileClient.DeleteAsync(cancellationToken: ct);
        }
    }

    public async Task RemoveAttachmentAsync(string path, CancellationToken ct)
    {
        var fileClient = _blobContainerClient.GetBlobClient(path);

        await fileClient.DeleteAsync(cancellationToken: ct);
    }
}