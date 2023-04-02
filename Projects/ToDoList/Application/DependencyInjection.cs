﻿using Application.Services.ToDoItems;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IToDoItemsService, ToDoItemsService>();

        return services;
    }
}