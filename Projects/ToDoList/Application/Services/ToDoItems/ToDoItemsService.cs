﻿using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Models;
using ByteSizeLib;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services.ToDoItems;

public class ToDoItemsService : IToDoItemsService
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IPublishToDoService _publishToDo;
    private readonly ILogger<ToDoItemsService> _logger;
    private readonly IFileAttachmentService _attachmentService;

    public ToDoItemsService(IFileAttachmentService attachmentService, IApplicationDbContext dbContext, IPublishToDoService publishToDo, ILogger<ToDoItemsService> logger)
    {
        _dbContext = dbContext;
        _publishToDo = publishToDo;
        _logger = logger;
        _attachmentService = attachmentService;
    }

    public async Task<ToDoItemDto> GetAsync(Guid id, CancellationToken ct)
    {
        var query = _dbContext.ToDoItems.Include(s=>s.Attachments);
        var str = query.ToQueryString();
        var itemFromDb = query.FirstOrDefault(f=>f.Id == id);

        if (itemFromDb == null)
        {
            throw new NotFoundException(nameof(ToDoItem), id);
        }

        return new ToDoItemDto()
        {
            Description = itemFromDb.Description,
            Id = itemFromDb.Id,
            IsCompleted = itemFromDb.IsCompleted,
            Name = itemFromDb.Name,
            Attachments = await GetAttachmentReferencesForFileAsync(itemFromDb, ct).ToListAsync(ct)
        };
    }

    public async IAsyncEnumerable<ToDoItemDto> GetAllAsync([EnumeratorCancellation] CancellationToken ct)
    {
        var toDoItems = _dbContext.ToDoItems.Include(s => s.Attachments);

        foreach (var toDoItemDto in toDoItems)
        {
            yield return new ToDoItemDto()
            {
                Description = toDoItemDto.Description,
                Id = toDoItemDto.Id,
                IsCompleted = toDoItemDto.IsCompleted,
                Name = toDoItemDto.Name,
                Attachments = await GetAttachmentReferencesForFileAsync(toDoItemDto, ct).ToListAsync(ct)
            };
        }
    }

    public async Task<Guid> AddAsync(ToDoItemToAdd toDoItem, IEnumerable<AttachmentInFileSystem> attachments, CancellationToken ct)
    {
        try
        {
            var tasks = attachments.Select(s => Task.Run(async () => await _attachmentService.AddAttachmentAsync(s, ct), ct));
            
            var attToAdd = await Task.WhenAll(tasks);
            
            var itemToAdd = new ToDoItem()
            {
                Description = toDoItem.Description!,
                Name = toDoItem.Name,
                Attachments = attToAdd.Select(f=> new ToDoAttachment()
                {
                    Path = f
                }).ToList()
            };

            _dbContext.ToDoItems.Add(itemToAdd);

            await _dbContext.SaveChangesAsync(ct);

            return itemToAdd.Id;
        }
        catch (Exception)
        {
            var removeAddedAttachmentTasks = attachments.Select(s => Task.Run(async () => await _attachmentService.RemoveAttachmentIfExistAsync(s.Name, ct), ct));
            await Task.WhenAll(removeAddedAttachmentTasks);
            throw;
        }
        
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var itemFromDb = await _dbContext.ToDoItems.FindAsync(id, ct);

        if (itemFromDb == null)
        {
            throw new NotFoundException(nameof(ToDoItem), id);
        }

        _dbContext.ToDoItems.Remove(itemFromDb);

        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task ChangeStatusAsync(Guid id, CancellationToken ct)
    {
        var itemFromDb = await _dbContext.ToDoItems.FindAsync(id, ct);

        if (itemFromDb == null)
        {
            throw new NotFoundException(nameof(ToDoItem), id);
        }

        itemFromDb.IsCompleted = !itemFromDb.IsCompleted;

        await _dbContext.SaveChangesAsync(ct);
    }

    private async IAsyncEnumerable<AttachmentDto> GetAttachmentReferencesForFileAsync(ToDoItem item, [EnumeratorCancellation] CancellationToken ct)
    {
        foreach (var attachment in item.Attachments)
        {
            var attachmentFromFiles = await _attachmentService.GetAttachmentReferenceAsync(attachment.Path, ct);

            yield return new AttachmentDto()
            {
                Id = attachment.Id.ToString(),
                Name = Path.GetFileName(attachmentFromFiles.Name),
                SizeInMb = ByteSize.FromBytes(attachmentFromFiles.SizeInBytes).MegaBytes.ToString(CultureInfo.InvariantCulture)
            };
        }
    }

    public async Task PublishAllToDoItems(CancellationToken ct)
    {
        var usagesInKb = new StringBuilder();
        usagesInKb.AppendLine();
        usagesInKb.AppendLine($"Before: {ByteSize.FromBytes(GC.GetTotalMemory(true)).KiloBytes.ToString(CultureInfo.InvariantCulture)}");

        var items = _dbContext.ToDoItems.Include(s => s.Samples).ThenInclude(s => s.SecondSamples).AsEnumerable();

        foreach (var toDoItem in items)
        {
            await _publishToDo.PublishToDoAsync(toDoItem);
            usagesInKb.AppendLine($"In progress: {ByteSize.FromBytes(GC.GetTotalMemory(true)).KiloBytes.ToString(CultureInfo.InvariantCulture)}");
            ct.ThrowIfCancellationRequested();
        }

        usagesInKb.AppendLine();
        usagesInKb.Append($"After: {ByteSize.FromBytes(GC.GetTotalMemory(true)).KiloBytes.ToString(CultureInfo.InvariantCulture)}");

        _logger.LogInformation(usagesInKb.ToString());
    }
}