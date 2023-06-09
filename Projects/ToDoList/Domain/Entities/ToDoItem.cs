﻿namespace Domain.Entities;

public class ToDoItem
{
    public ToDoItem()
    {
        Attachments = new List<ToDoAttachment>();
    }
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public ICollection<ToDoAttachment> Attachments { get; set; }
}