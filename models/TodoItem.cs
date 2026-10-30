namespace todoapp_1.Models;

public class TodoItem
{
    private string _title = string.Empty;
    private bool _isDone;
    private int _priority;

    public string Title => _title;
    public bool IsDone => _isDone;
    public int Priority => _priority;

    public TodoItem(string title, int priority)
    {
        Rename(title);
        SetPriority(priority);
        _isDone = false;
    }

    public void Complete()
    {
        _isDone = true;
    }

    public void Rename(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException(
                "Название задачи не может быть пустым.");
        }

        _title = title.Trim();
    }

    public void SetPriority(int priority)
    {
        if (priority < 1 || priority > 5)
        {
            throw new ArgumentException(
                "Приоритет должен быть от 1 до 5.");
        }

        _priority = priority;
    }

    public void RestoreState(bool isDone)
    {
        _isDone = isDone;
    }
}
