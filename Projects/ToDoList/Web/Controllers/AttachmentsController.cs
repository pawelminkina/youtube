using Application.Common.Interfaces;
using Application.Services.Attachments;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("attachments")]
public class AttachmentsController : ControllerBase
{
    private readonly IAttachmentsService _attachmentsService;
    public AttachmentsController(IAttachmentsService attachmentsService)
    {
        _attachmentsService = attachmentsService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetContent(Guid id, CancellationToken ct)
    {
        var attachment = await _attachmentsService.GetAttachmentContentAsync(id, ct);

        return File(attachment.Content, attachment.MimeType);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _attachmentsService.RemoveAttachmentAsync(id, ct);

        return NoContent();
    }
}