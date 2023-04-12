using Application.Common.Interfaces;
using Application.Models;
using Application.Services.ToDoItems;
using AutoFixture;
using Domain.Entities;
using NSubstitute;
using System.Net.Mail;
using FluentAssertions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Integration.Application.Services.ToDoItems;

public class ToDoItemsServiceTests
{
    private static Guid _databaseName = Guid.Empty;
    private readonly Fixture _fixture = new();

    public ToDoItemsServiceTests()
    {
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }
    [SetUp]
    public void SetUp()
    {
        _databaseName = Guid.NewGuid();
    }

    [TearDown]
    public void TearDown()
    {
        _databaseName = Guid.Empty;
    }

    [Test]
    public async Task GetAllAsync_ReturnsAllItems()
    {
        //GIVEN to do items in available in database
        var toDoItems = _fixture.CreateMany<ToDoItem>().ToList();

        //AND attachments available in storage account
        var fileAttachmentService = Substitute.For<IFileAttachmentService>();
        SetUpFileService(fileAttachmentService, toDoItems);

        //WHEN service is called and data is retrieved
        var sut = CreateSut(fileAttachmentService, toDoItems);
        var res = await sut.GetAllAsync(CancellationToken.None).ToListAsync(CancellationToken.None);

        //THEN check whether returned data is expected
        var expected = GetResult(toDoItems);

        res.Should().BeEquivalentTo(expected);
    }

    private static ToDoItemsService CreateSut(IFileAttachmentService? fileAttachmentService, IEnumerable<ToDoItem> items)
    {
        fileAttachmentService ??= Substitute.For<IFileAttachmentService>();
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(_databaseName.ToString());
        var dbContext = new ApplicationDbContext(options.Options);
        
        dbContext.AddRange(items);
        dbContext.SaveChanges();

        return new ToDoItemsService(fileAttachmentService, dbContext);
    }

    private static IEnumerable<ToDoItemDto> GetResult(IEnumerable<ToDoItem> dto)
    {
        return dto.Select(f => new ToDoItemDto()
        {
            Description = f.Description,
            Id = f.Id,
            IsCompleted = f.IsCompleted,
            Name = f.Name,
            Attachments = f.Attachments.Select(s => new AttachmentDto()
            {
                Id = s.Id.ToString(),
                Name = Path.GetFileName(s.Path),
                SizeInMb = "1"
            })
        });
    }

    private static void SetUpFileService(IFileAttachmentService service, IEnumerable<ToDoItem> toDoItems)
    {
        foreach (var toDoItem in toDoItems)
        {
            foreach (var attachment in toDoItem.Attachments)
            {
                service.GetAttachmentReferenceAsync(Arg.Is(attachment.Path), CancellationToken.None)
                    .Returns(new AttachmentFileDto()
                    {
                        Name = attachment.Path,
                        SizeInBytes = 1000000
                    });
            }
        }
    }
}