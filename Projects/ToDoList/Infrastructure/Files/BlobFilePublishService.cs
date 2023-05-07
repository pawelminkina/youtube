using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Common;
using Application.Common.Interfaces;
using Azure.Storage.Blobs;
using Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Files;

public class BlobFilePublishService : IPublishToDoService
{
    private readonly BlobContainerClient _blobContainerClient;
    public BlobFilePublishService(IConfiguration configuration)
    {
        _blobContainerClient =
            new BlobContainerClient(
                configuration.GetConnectionString(AppConfig.AttachmentsFileStorageConnectionStringName),
                configuration.GetValue<string>(AppConfig.PublishToDoFileContainerName));

        _blobContainerClient.DeleteIfExists();
        _blobContainerClient.CreateIfNotExists();
    }
    public async Task PublishToDoAsync(ToDoItem item)
    {
        var jsonOptions = new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = true
        };
        using var stream = new MemoryStream(JsonSerializer.SerializeToUtf8Bytes(item, jsonOptions));
        await _blobContainerClient.UploadBlobAsync($"{item.Id}.json", stream);
    }
}