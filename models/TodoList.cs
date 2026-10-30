using System.Text.Json;

namespace todoapp_1.Models;

public class TodoList
{
    private readonly List<TodoItem> _items = new();

    private const string FileName = "todos.json";

    public IReadOnlyList<TodoItem> Items => _items;

    public void Add(string title, int priority)
    {
        TodoItem item = new TodoItem(title, priority);
        _items.Add(item);
    }

    public void Remove(TodoItem item)
    {
        if (!_items.Remove(item))
        {
            throw new ArgumentException("Задача не найдена.");
        }
    }

    public void Complete(TodoItem item)
    {
        if (!_items.Contains(item))
        {
            throw new ArgumentException("Задача не найдена.");
        }

        item.Complete();
    }

    public void Rename(TodoItem item, string newTitle)
    {
        if (!_items.Contains(item))
        {
            throw new ArgumentException("Задача не найдена.");
        }

        item.Rename(newTitle);
    }

    public void SetPriority(TodoItem item, int priority)
    {
        if (!_items.Contains(item))
        {
            throw new ArgumentException("Задача не найдена.");
        }

        item.SetPriority(priority);
    }

    public IEnumerable<TodoItem> GetFiltered(string filter)
    {
        return filter switch
        {
            "Активные" => _items.Where(item => !item.IsDone),
            "Выполненные" => _items.Where(item => item.IsDone),
            _ => _items
        };
    }

    public IEnumerable<TodoItem> Search(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return _items;
        }

        return _items.Where(item =>
            item.Title.Contains(
                text,
                StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<TodoItem> SortByPriority(
        IEnumerable<TodoItem> items)
    {
        return items.OrderByDescending(item => item.Priority);
    }

    public void Save()
    {
        List<TodoItemData> data = _items
            .Select(item => new TodoItemData
            {
                Title = item.Title,
                IsDone = item.IsDone,
                Priority = item.Priority
            })
            .ToList();

        JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };

        string json = JsonSerializer.Serialize(data, options);

        File.WriteAllText(FileName, json);
    }

    public void Load()
    {
        if (!File.Exists(FileName))
        {
            return;
        }

        string json = File.ReadAllText(FileName);

        List<TodoItemData>? data =
            JsonSerializer.Deserialize<List<TodoItemData>>(json);

        if (data == null)
        {
            return;
        }

        _items.Clear();

        foreach (TodoItemData item in data)
        {
            TodoItem todo = new TodoItem(
                item.Title,
                item.Priority);

            todo.RestoreState(item.IsDone);

            _items.Add(todo);
        }
    }

    private class TodoItemData
    {
        public string Title { get; set; } = string.Empty;
        public bool IsDone { get; set; }
        public int Priority { get; set; }
    }
}