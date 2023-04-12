using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Services.Attachments;
using Domain.Entities;
using FluentAssertions;
using MockQueryable.NSubstitute;
using NSubstitute;

namespace Unit.Application.Services.Attachments;

public class AttachmentServiceTests
{
    [Test]
    public async Task GetAttachmentContentAsync_AttachmentDoesNotExist_NotFoundExceptionThrown()
    {
        //Arrange
        var dbContext = Substitute.For<IApplicationDbContext>();
        var mock = new List<ToDoAttachment>().AsQueryable().BuildMockDbSet();
        dbContext.ToDoAttachments.Returns(mock);
        var sut = CreateSut(dbContext: dbContext);
        var idOfAttachment = Guid.NewGuid();

        //Act
        var action = async () => await sut.GetAttachmentContentAsync(idOfAttachment, CancellationToken.None);

        //Assert
        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task GetAttachmentContentAsync_ItemFound_CallsFileServiceWithCorrectPath()
    {
        //Arrange
        var dbContext = Substitute.For<IApplicationDbContext>();
        var fileService = Substitute.For<IFileAttachmentService>();
        var idOfAttachment = Guid.NewGuid();
        var expectedPath = Guid.NewGuid().ToString();

        var mock = new[]
        {
            new ToDoAttachment {Id = idOfAttachment, Path = expectedPath}
        }.AsQueryable().BuildMockDbSet();

        dbContext.ToDoAttachments.Returns(mock);
        var sut = CreateSut(fileAttachmentService: fileService, dbContext: dbContext);

        //Act
        await sut.GetAttachmentContentAsync(idOfAttachment, CancellationToken.None);

        //Assert
        await fileService.Received().GetContent(Arg.Is(expectedPath), Arg.Is(CancellationToken.None));
    }

    private static AttachmentService CreateSut(IFileAttachmentService? fileAttachmentService = null, IApplicationDbContext? dbContext = null)
    {
        fileAttachmentService ??= Substitute.For<IFileAttachmentService>();
        dbContext ??= Substitute.For<IApplicationDbContext>();

        return new AttachmentService(fileAttachmentService, dbContext);
    }
}