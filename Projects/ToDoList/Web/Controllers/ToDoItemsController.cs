using Application.Models;
using Application.Services.ToDoItems;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("todoitems")]
public class ToDoItemsController : ControllerBase
{
    private readonly IToDoItemsService _toDoItemsService;

    public ToDoItemsController(IToDoItemsService toDoItemsService)
    {
        _toDoItemsService = toDoItemsService;
    }

    [HttpGet]
    public IAsyncEnumerable<ToDoItemDto> GetAll(CancellationToken ct)
    {
        return _toDoItemsService.GetAllAsync(ct);
    }


    [HttpGet("{id:guid}")]
    [ActionName(nameof(GetOne))]
    public async Task<ActionResult<ToDoItemDto>> GetOne(Guid id, CancellationToken ct)
    {
        return await _toDoItemsService.GetAsync(id, ct);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromForm] ToDoItemToAdd toDoItem, [FromForm] List<IFormFile> attachments, CancellationToken ct)
    {
        var attachmentsDto = attachments.Select(f => new AttachmentToAdd()
        {
            Content = f.OpenReadStream(),
            Name = f.FileName
        });

        var id = await _toDoItemsService.AddAsync(toDoItem, attachmentsDto, ct);

        return CreatedAtAction(nameof(GetOne), new { id }, id);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _toDoItemsService.DeleteAsync(id, ct);

        return NoContent();
    }

    [HttpPut("{id:guid}/name")]
    public async Task<IActionResult> UpdateName(Guid id, [FromQuery] string name, CancellationToken ct)
    {
        await _toDoItemsService.ChangeNameAsync(id, name, ct);
        return NoContent();
    }
}