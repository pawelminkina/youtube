﻿using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Models;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Attachments;

public class AttachmentService : IAttachmentsService
{
    private readonly IFileAttachmentService _fileAttachmentService;
    private readonly IApplicationDbContext _dbContext;

    public AttachmentService(IFileAttachmentService fileAttachmentService, IApplicationDbContext dbContext)
    {
        _fileAttachmentService = fileAttachmentService;
        _dbContext = dbContext;
    }
    public async Task<AttachmentInFileSystem> GetAttachmentContentAsync(Guid id, CancellationToken ct)
    {
        var attachmentFromDb = await GetAttachmentFromDbAsync(id, ct);

        return await _fileAttachmentService.GetContent(attachmentFromDb.Path, ct);
    }

    public async Task RemoveAttachmentAsync(Guid id, CancellationToken ct)
    {
        var attachmentFromDb = await GetAttachmentFromDbAsync(id, ct);

        _dbContext.ToDoAttachments.Remove(attachmentFromDb);
        await _dbContext.SaveChangesAsync(ct);

        await _fileAttachmentService.RemoveAttachmentAsync(attachmentFromDb.Path, ct);
    }

    private async Task<ToDoAttachment> GetAttachmentFromDbAsync(Guid id, CancellationToken ct)
    {
        var attachmentFromDb = await _dbContext.ToDoAttachments.FirstOrDefaultAsync(s=>s.Id == id, ct);

        if (attachmentFromDb == null)
        {
            throw new NotFoundException(nameof(ToDoAttachment), id);
        }

        return attachmentFromDb;
    }
}