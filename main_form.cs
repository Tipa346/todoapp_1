using todoapp_1.Models;

namespace todoapp_1;

public partial class Form1 : Form
{
    private readonly TodoList _todoList = new();

    private TextBox _titleBox = null!;
    private NumericUpDown _priorityBox = null!;
    private Button _addButton = null!;
    private Button _completeButton = null!;
    private Button _renameButton = null!;
    private Button _deleteButton = null!;
    private Button _sortButton = null!;
    private ComboBox _filterBox = null!;
    private TextBox _searchBox = null!;
    private ListView _listView = null!;

    public Form1()
    {
        InitializeComponent();

        _todoList.Load();

        CreateInterface();
        RefreshList();
    }

    private void CreateInterface()
    {
        Text = "Список дел";
        Width = 900;
        Height = 600;
        StartPosition = FormStartPosition.CenterScreen;

        // Панель сверху
        Panel topPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 110
        };

        // Название задачи
        Label titleLabel = new Label
        {
            Text = "Название:",
            Left = 10,
            Top = 15,
            Width = 80
        };

        _titleBox = new TextBox
        {
            Left = 95,
            Top = 10,
            Width = 250
        };

        // Приоритет
        Label priorityLabel = new Label
        {
            Text = "Приоритет:",
            Left = 360,
            Top = 15,
            Width = 75
        };

        _priorityBox = new NumericUpDown
        {
            Left = 440,
            Top = 10,
            Width = 60,
            Minimum = 1,
            Maximum = 5,
            Value = 3
        };

        // Кнопка добавления
        _addButton = new Button
        {
            Text = "Добавить",
            Left = 515,
            Top = 8,
            Width = 100
        };

        // Выполнить
        _completeButton = new Button
        {
            Text = "Выполнить",
            Left = 625,
            Top = 8,
            Width = 100
        };

        // Удалить
        _deleteButton = new Button
        {
            Text = "Удалить",
            Left = 735,
            Top = 8,
            Width = 100
        };

        // Поиск
        Label searchLabel = new Label
        {
            Text = "Поиск:",
            Left = 10,
            Top = 60,
            Width = 50
        };

        _searchBox = new TextBox
        {
            Left = 65,
            Top = 55,
            Width = 220
        };

        // Фильтр
        Label filterLabel = new Label
        {
            Text = "Фильтр:",
            Left = 300,
            Top = 60,
            Width = 55
        };

        _filterBox = new ComboBox
        {
            Left = 360,
            Top = 55,
            Width = 140,
            DropDownStyle = ComboBoxStyle.DropDownList
        };

        _filterBox.Items.AddRange(new[]
        {
            "Все",
            "Активные",
            "Выполненные"
        });

        _filterBox.SelectedIndex = 0;

        // Сортировка
        _sortButton = new Button
        {
            Text = "Сортировать",
            Left = 515,
            Top = 53,
            Width = 100
        };

        // Переименование
        _renameButton = new Button
        {
            Text = "Переименовать",
            Left = 625,
            Top = 53,
            Width = 100
        };

        // Список задач
        _listView = new ListView
        {
            Dock = DockStyle.Fill,
            View = View.Details,
            FullRowSelect = true,
            GridLines = true,
            MultiSelect = false
        };

        _listView.Columns.Add("Название", 400);
        _listView.Columns.Add("Статус", 150);
        _listView.Columns.Add("Приоритет", 100);

        // Добавляем элементы на форму
        topPanel.Controls.Add(titleLabel);
        topPanel.Controls.Add(_titleBox);
        topPanel.Controls.Add(priorityLabel);
        topPanel.Controls.Add(_priorityBox);
        topPanel.Controls.Add(_addButton);
        topPanel.Controls.Add(_completeButton);
        topPanel.Controls.Add(_deleteButton);
        topPanel.Controls.Add(searchLabel);
        topPanel.Controls.Add(_searchBox);
        topPanel.Controls.Add(filterLabel);
        topPanel.Controls.Add(_filterBox);
        topPanel.Controls.Add(_sortButton);
        topPanel.Controls.Add(_renameButton);

        Controls.Add(_listView);
        Controls.Add(topPanel);

        // Обработчики событий
        _addButton.Click += AddButton_Click;
        _completeButton.Click += CompleteButton_Click;
        _deleteButton.Click += DeleteButton_Click;
        _renameButton.Click += RenameButton_Click;
        _sortButton.Click += SortButton_Click;

        _filterBox.SelectedIndexChanged += FilterChanged;
        _searchBox.TextChanged += SearchChanged;
    }

    private TodoItem? GetSelectedItem()
    {
        if (_listView.SelectedItems.Count == 0)
        {
            return null;
        }

        return _listView.SelectedItems[0].Tag as TodoItem;
    }

    private void AddButton_Click(
        object? sender,
        EventArgs e)
    {
        try
        {
            _todoList.Add(
                _titleBox.Text,
                (int)_priorityBox.Value);

            _titleBox.Clear();
            _priorityBox.Value = 3;

            RefreshList();
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(
                ex.Message,
                "Ошибка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
    }

    private void CompleteButton_Click(
        object? sender,
        EventArgs e)
    {
        try
        {
            TodoItem? item = GetSelectedItem();

            if (item == null)
            {
                throw new ArgumentException(
                    "Сначала выберите задачу.");
            }

            _todoList.Complete(item);

            RefreshList();
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(
                ex.Message,
                "Ошибка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
    }

    private void DeleteButton_Click(
        object? sender,
        EventArgs e)
    {
        try
        {
            TodoItem? item = GetSelectedItem();

            if (item == null)
            {
                throw new ArgumentException(
                    "Сначала выберите задачу.");
            }

            DialogResult result = MessageBox.Show(
                "Удалить выбранную задачу?",
                "Подтверждение",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                return;
            }

            _todoList.Remove(item);

            RefreshList();
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(
                ex.Message,
                "Ошибка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
    }

    private void RenameButton_Click(
        object? sender,
        EventArgs e)
    {
        try
        {
            TodoItem? item = GetSelectedItem();

            if (item == null)
            {
                throw new ArgumentException(
                    "Сначала выберите задачу.");
            }

            string? newTitle = ShowInputDialog(
                "Введите новое название:",
                item.Title);

            if (newTitle == null)
            {
                return;
            }

            _todoList.Rename(item, newTitle);

            RefreshList();
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(
                ex.Message,
                "Ошибка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
    }

    private void SortButton_Click(
        object? sender,
        EventArgs e)
    {
        RefreshList(true);
    }

    private void FilterChanged(
        object? sender,
        EventArgs e)
    {
        RefreshList();
    }

    private void SearchChanged(
        object? sender,
        EventArgs e)
    {
        RefreshList();
    }

    private void RefreshList(bool sort = false)
    {
        _listView.Items.Clear();

        string filter =
            _filterBox?.SelectedItem?.ToString()
            ?? "Все";

        string search =
            _searchBox?.Text
            ?? "";

        IEnumerable<TodoItem> items =
            _todoList.GetFiltered(filter);

        if (!string.IsNullOrWhiteSpace(search))
        {
            items = items.Where(item =>
                item.Title.Contains(
                    search,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (sort)
        {
            items = _todoList.SortByPriority(items);
        }

        foreach (TodoItem item in items)
        {
            ListViewItem row =
                new ListViewItem(item.Title);

            row.SubItems.Add(
                item.IsDone
                    ? "Выполнена"
                    : "Активна");

            row.SubItems.Add(
                item.Priority.ToString());

            row.Tag = item;

            if (item.IsDone)
            {
                row.Font = new Font(
                    row.Font,
                    FontStyle.Strikeout);
            }

            if (item.Priority >= 4 && !item.IsDone)
            {
                row.BackColor = Color.LightYellow;
            }

            _listView.Items.Add(row);
        }
    }

    private static string? ShowInputDialog(
        string message,
        string defaultValue)
    {
        using Form form = new Form
        {
            Width = 450,
            Height = 160,
            Text = "Переименование",
            StartPosition =
                FormStartPosition.CenterParent
        };

        Label label = new Label
        {
            Left = 10,
            Top = 10,
            Width = 410,
            Text = message
        };

        TextBox textBox = new TextBox
        {
            Left = 10,
            Top = 35,
            Width = 410,
            Text = defaultValue
        };

        Button okButton = new Button
        {
            Text = "OK",
            Left = 245,
            Top = 70,
            Width = 80,
            DialogResult = DialogResult.OK
        };

        Button cancelButton = new Button
        {
            Text = "Отмена",
            Left = 335,
            Top = 70,
            Width = 85,
            DialogResult = DialogResult.Cancel
        };

        form.Controls.Add(label);
        form.Controls.Add(textBox);
        form.Controls.Add(okButton);
        form.Controls.Add(cancelButton);

        form.AcceptButton = okButton;
        form.CancelButton = cancelButton;

        return form.ShowDialog() == DialogResult.OK
            ? textBox.Text
            : null;
    }

    protected override void OnFormClosing(
        FormClosingEventArgs e)
    {
        try
        {
            _todoList.Save();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Не удалось сохранить задачи:\n{ex.Message}",
                "Ошибка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        base.OnFormClosing(e);
    }
}