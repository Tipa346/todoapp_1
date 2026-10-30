using System;

namespace todoapp_1.models;

public class TodoItem
{
    private string _title = string.Empty;
    private bool _isDone;
    private int _priority;
    private DateTime _createdAt;
    private DateTime _endDate;
    public string Title => _title;
    public bool IsDone => _isDone;
    public int Priority => _priority;
    public DateTime CreatedAt => _createdAt;
    public DateTime EndDate => _endDate;

    public TodoItem(
        string title,
        int priority,
        DateTime endDate)
    {
        _createdAt = DateTime.Today;

        Rename(title);
        SetPriority(priority);
        SetEndDate(endDate);

        _isDone = false;
    }

    public TodoItem(
        string title,
        int priority,
        DateTime createdAt,
        DateTime endDate,
        bool isDone)
    {
        Rename(title);
        SetPriority(priority);
        _createdAt = createdAt.Date;
        _endDate = endDate.Date;
        _isDone = isDone;
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
                "Название задачи не может быть пустым."
            );
        }

        _title = title.Trim();
    }

    public void SetPriority(int priority)
    {
        if (priority < 1 || priority > 5)
        {
            throw new ArgumentException(
                "Приоритет должен быть от 1 до 5."
            );
        }

        _priority = priority;
    }

    public void SetEndDate(DateTime endDate)
    {
        _endDate = endDate.Date;
    }

    public int GetDaysLeft()
    {
        return (_endDate.Date - DateTime.Today).Days;
    }

    public bool IsOverdue()
    {
        return !_isDone &&
               _endDate.Date < DateTime.Today;
    }

    public double GetUrgency()
    {
        int daysLeft = GetDaysLeft();

        if (daysLeft <= 0)
        {
            return 1.0;
        }

        if (daysLeft >= 7)
        {
            return 0.0;
        }

        return 1.0 - daysLeft / 7.0;
    }

    public string GetDaysLeftText()
    {
        if (_isDone)
        {
            return "—";
        }

        int days = GetDaysLeft();

        if (days < 0)
        {
            int overdueDays = Math.Abs(days);

            if (overdueDays == 1)
            {
                return "Просрочена на 1 день";
            }

            return $"Просрочена на {overdueDays} дн.";
        }

        if (days == 0)
        {
            return "Сегодня";
        }

        if (days == 1)
        {
            return "1 день";
        }

        return $"{days} дн.";
    }
}