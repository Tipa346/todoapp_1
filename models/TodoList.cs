using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace todoapp_1.models;

public class TodoList
{
    private readonly List<TodoItem> _items = new();
    private const string FileName = "todos.json";
    public IReadOnlyList<TodoItem> Items => _items;
    public void Add(
        string title,
        int priority,
        DateTime endDate)
    {
        TodoItem item = new TodoItem(
            title,
            priority,
            endDate
        );

        _items.Add(item);
    }

    public void Remove(TodoItem item)
    {
        if (item == null)
        {
            return;
        }
        _items.Remove(item);
    }

    public void Complete(TodoItem item)
    {
        if (item == null)
        {
            return;
        }

        item.Complete();
    }

    public void Rename(
        TodoItem item,
        string newTitle)
    {
        if (item == null)
        {
            return;
        }

        item.Rename(newTitle);
    }

    public void SetPriority(
        TodoItem item,
        int priority)
    {
        if (item == null)
        {
            return;
        }

        item.SetPriority(priority);
    }

    public void SetEndDate(
        TodoItem item,
        DateTime endDate)
    {
        if (item == null)
        {
            return;
        }

        item.SetEndDate(endDate);
    }

    public IEnumerable<TodoItem> GetByStatus(
        string filter)
    {
        return filter switch
        {
            "Активные" =>
                _items.Where(item =>
                    !item.IsDone),

            "Выполненные" =>
                _items.Where(item =>
                    item.IsDone),

            "Просроченные" =>
                _items.Where(item =>
                    !item.IsDone &&
                    item.IsOverdue()),

            _ =>
                _items
        };
    }

    public IEnumerable<TodoItem> Search(
        IEnumerable<TodoItem> items,
        string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return items;
        }
        searchText = searchText.Trim();
        return items.Where(item =>
            item.Title.Contains(
                searchText,
                StringComparison.OrdinalIgnoreCase
            ));
    }

    public IEnumerable<TodoItem> SortByUrgency(
        IEnumerable<TodoItem> items)
    {
        return items
            .OrderByDescending(item =>
                item.GetUrgency());
    }

    public IEnumerable<TodoItem> SortByPriority(
        IEnumerable<TodoItem> items)
    {
        return items
            .OrderBy(item =>
                item.Priority);
    }

    public IEnumerable<TodoItem> SortByEndDate(
        IEnumerable<TodoItem> items)
    {
        return items
            .OrderBy(item =>
                item.EndDate);
    }

    public IEnumerable<TodoItem> SortByTitle(
        IEnumerable<TodoItem> items)
    {
        return items
            .OrderBy(item =>
                item.Title);
    }

    public void Save()
    {
        List<TodoItemData> data =
            _items.Select(item =>
                new TodoItemData
                {
                    Title = item.Title,
                    IsDone = item.IsDone,
                    Priority = item.Priority,
                    CreatedAt = item.CreatedAt,
                    EndDate = item.EndDate
                })
            .ToList();

        string json =
            JsonSerializer.Serialize(
                data,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                }
            );

        File.WriteAllText(
            FileName,
            json
        );
    }

    public void Load()
    {
        if (!File.Exists(FileName))
        {
            return;
        }
        try
        {
            string json =
                File.ReadAllText(FileName);
            List<TodoItemData>? data =
                JsonSerializer.Deserialize<
                    List<TodoItemData>
                >(json);

            if (data == null)
            {
                return;
            }
            _items.Clear();
            foreach (TodoItemData item in data)
            {
                try
                {
                    TodoItem todo =
                        new TodoItem(
                            item.Title ?? "Без названия",
                            item.Priority,
                            item.CreatedAt.Date,
                            item.EndDate.Date,
                            item.IsDone
                        );

                    _items.Add(todo);
                }
                catch
                {
                }
            }
        }
        catch
        { }
    }

    private class TodoItemData
    {
        public string? Title { get; set; }
        public bool IsDone { get; set; }
        public int Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime EndDate { get; set; }
    }
}