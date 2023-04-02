using Application.Services.Attachments;
using Application.Services.ToDoItems;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IToDoItemsService, ToDoItemsService>();
        services.AddScoped<IAttachmentsService, AttachmentService>();

        return services;
    }
}