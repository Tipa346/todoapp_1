using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using todoapp_1.models;

namespace todoapp_1;

public partial class Form1 : Form
{
    private readonly TodoList _todoList;
    private Label titleLabel = null!;
    private TextBox titleTextBox = null!;
    private Label priorityLabel = null!;
    private NumericUpDown priorityNumeric = null!;
    private Label endDateLabel = null!;
    private DateTimePicker endDatePicker = null!;
    private Button addButton = null!;
    private Label searchLabel = null!;
    private TextBox searchTextBox = null!;
    private Label statusFilterLabel = null!;
    private ComboBox statusFilterCombo = null!;
    private Label dateFilterLabel = null!;
    private ComboBox dateFilterCombo = null!;
    private Button resetButton = null!;
    private Label sortLabel = null!;
    private ComboBox sortCombo = null!;
    private Button completeButton = null!;
    private Button deleteButton = null!;
    private Button renameButton = null!;
    private ListView listView = null!;
    private System.Windows.Forms.Timer refreshTimer = null!;
    public Form1()
    {
        _todoList = new TodoList();
        InitializeComponent();
        InitializeForm();
        InitializeControls();
        _todoList.Load();
        RefreshList();
        refreshTimer =
            new System.Windows.Forms.Timer();
        refreshTimer.Interval = 60000;
        refreshTimer.Tick +=
            (sender, e) =>
            {
                RefreshList();
            };
        refreshTimer.Start();
    }

    private void InitializeForm()
    {
        Text = "Список дел";
        StartPosition =
            FormStartPosition.CenterScreen;
        MinimumSize =
            new Size(1000, 600);
        Size =
            new Size(1200, 700);
        BackColor = Color.White;
        Font =
            new Font(
                "Segoe UI",
                9F
            );
    }

    private void InitializeControls()
    {
        titleLabel = new Label();

        titleLabel.Text = "Название:";
        titleLabel.Location =
            new Point(10, 18);

        titleLabel.AutoSize = true;

        Controls.Add(titleLabel);


        titleTextBox = new TextBox();

        titleTextBox.Location =
            new Point(90, 14);

        titleTextBox.Size =
            new Size(240, 25);

        Controls.Add(titleTextBox);

        priorityLabel = new Label();

        priorityLabel.Text = "Приоритет:";
        priorityLabel.Location =
            new Point(350, 18);

        priorityLabel.AutoSize = true;

        Controls.Add(priorityLabel);


        priorityNumeric =
            new NumericUpDown();

        priorityNumeric.Minimum = 1;
        priorityNumeric.Maximum = 5;
        priorityNumeric.Value = 3;

        priorityNumeric.Location =
            new Point(420, 14);

        priorityNumeric.Size =
            new Size(60, 25);

        Controls.Add(priorityNumeric);

        endDateLabel = new Label();

        endDateLabel.Text = "Конец:";
        endDateLabel.Location =
            new Point(500, 18);

        endDateLabel.AutoSize = true;

        Controls.Add(endDateLabel);


        endDatePicker =
            new DateTimePicker();

        endDatePicker.Format =
            DateTimePickerFormat.Custom;

        endDatePicker.CustomFormat =
            "dd.MM.yyyy";

        endDatePicker.Value =
            DateTime.Today;

        endDatePicker.Location =
            new Point(545, 14);

        endDatePicker.Size =
            new Size(120, 25);

        Controls.Add(endDatePicker);

        addButton = new Button();

        addButton.Text = "Добавить";

        addButton.Location =
            new Point(680, 12);

        addButton.Size =
            new Size(100, 30);

        addButton.Click +=
            AddButton_Click;

        Controls.Add(addButton);

        searchLabel = new Label();

        searchLabel.Text = "Поиск:";

        searchLabel.Location =
            new Point(10, 65);

        searchLabel.AutoSize = true;

        Controls.Add(searchLabel);


        searchTextBox = new TextBox();

        searchTextBox.Location =
            new Point(60, 61);

        searchTextBox.Size =
            new Size(220, 25);

        searchTextBox.TextChanged +=
            (sender, e) =>
            {
                RefreshList();
            };

        Controls.Add(searchTextBox);

        statusFilterLabel =
            new Label();

        statusFilterLabel.Text =
            "Фильтр:";

        statusFilterLabel.Location =
            new Point(300, 65);

        statusFilterLabel.AutoSize = true;

        Controls.Add(statusFilterLabel);


        statusFilterCombo =
            new ComboBox();

        statusFilterCombo.DropDownStyle =
            ComboBoxStyle.DropDownList;

        statusFilterCombo.Items.Add("Все");
        statusFilterCombo.Items.Add("Активные");
        statusFilterCombo.Items.Add("Выполненные");
        statusFilterCombo.Items.Add("Просроченные");

        statusFilterCombo.SelectedIndex = 0;

        statusFilterCombo.Location =
            new Point(355, 61);

        statusFilterCombo.Size =
            new Size(140, 25);

        statusFilterCombo.SelectedIndexChanged +=
            (sender, e) =>
            {
                RefreshList();
            };

        Controls.Add(statusFilterCombo);

        dateFilterLabel =
            new Label();

        dateFilterLabel.Text =
            "Дата конца:";

        dateFilterLabel.Location =
            new Point(510, 65);

        dateFilterLabel.AutoSize = true;

        Controls.Add(dateFilterLabel);


        dateFilterCombo =
            new ComboBox();

        dateFilterCombo.DropDownStyle =
            ComboBoxStyle.DropDownList;

        dateFilterCombo.Items.Add("Все даты");
        dateFilterCombo.Items.Add("Сегодня");
        dateFilterCombo.Items.Add("Завтра");
        dateFilterCombo.Items.Add("Следующие 7 дней");
        dateFilterCombo.Items.Add("Просроченные");

        dateFilterCombo.SelectedIndex = 0;

        dateFilterCombo.Location =
            new Point(590, 61);

        dateFilterCombo.Size =
            new Size(170, 25);

        dateFilterCombo.SelectedIndexChanged +=
            (sender, e) =>
            {
                RefreshList();
            };

        Controls.Add(dateFilterCombo);

        resetButton = new Button();

        resetButton.Text =
            "Сбросить";

        resetButton.Location =
            new Point(775, 59);

        resetButton.Size =
            new Size(100, 30);

        resetButton.Click +=
            (sender, e) =>
            {
                searchTextBox.Clear();

                statusFilterCombo.SelectedIndex = 0;

                dateFilterCombo.SelectedIndex = 0;

                sortCombo.SelectedIndex = 0;
            };

        Controls.Add(resetButton);

        sortLabel = new Label();

        sortLabel.Text =
            "Сортировка:";

        sortLabel.Location =
            new Point(10, 108);

        sortLabel.AutoSize = true;

        Controls.Add(sortLabel);


        sortCombo = new ComboBox();

        sortCombo.DropDownStyle =
            ComboBoxStyle.DropDownList;

        sortCombo.Items.Add("По срочности");
        sortCombo.Items.Add("По приоритету");
        sortCombo.Items.Add("По дате конца");
        sortCombo.Items.Add("По названию");

        sortCombo.SelectedIndex = 0;

        sortCombo.Location =
            new Point(95, 104);

        sortCombo.Size =
            new Size(160, 25);

        sortCombo.SelectedIndexChanged +=
            (sender, e) =>
            {
                RefreshList();
            };

        Controls.Add(sortCombo);

        completeButton = new Button();

        completeButton.Text =
            "Выполнить";

        completeButton.Location =
            new Point(275, 102);

        completeButton.Size =
            new Size(100, 30);

        completeButton.Click +=
            CompleteButton_Click;

        Controls.Add(completeButton);

        deleteButton = new Button();

        deleteButton.Text =
            "Удалить";

        deleteButton.Location =
            new Point(385, 102);

        deleteButton.Size =
            new Size(100, 30);

        deleteButton.Click +=
            DeleteButton_Click;

        Controls.Add(deleteButton);

        renameButton = new Button();

        renameButton.Text =
            "Переименовать";

        renameButton.Location =
            new Point(495, 102);

        renameButton.Size =
            new Size(120, 30);

        renameButton.Click +=
            RenameButton_Click;

        Controls.Add(renameButton);

        listView = new ListView();

        listView.View =
            View.Details;

        listView.FullRowSelect =
            true;

        listView.GridLines =
            true;

        listView.MultiSelect =
            false;

        listView.HideSelection =
            false;

        listView.Location =
            new Point(5, 145);

        listView.Size =
            new Size(
                ClientSize.Width - 10,
                ClientSize.Height - 150
            );

        listView.Anchor =
            AnchorStyles.Top |
            AnchorStyles.Bottom |
            AnchorStyles.Left |
            AnchorStyles.Right;

        listView.Columns.Add(
            "Название",
            300
        );

        listView.Columns.Add(
            "Статус",
            130
        );

        listView.Columns.Add(
            "Приоритет",
            100
        );

        listView.Columns.Add(
            "Дата конца",
            130
        );

        listView.Columns.Add(
            "Осталось",
            160
        );

        listView.Columns.Add(
            "Срочность",
            100
        );
        listView.DoubleClick +=
            (sender, e) =>
            {
                RenameButton_Click(
                    sender,
                    e
                );
            };

        Controls.Add(listView);
    }

    private void AddButton_Click(
        object? sender,
        EventArgs e)
    {
        string title =
            titleTextBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(title))
        {
            MessageBox.Show(
                "Введите название задачи.",
                "Ошибка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );

            titleTextBox.Focus();
            return;
        }


        int priority =
            (int)priorityNumeric.Value;


        DateTime endDate =
            endDatePicker.Value.Date;


        try
        {
            _todoList.Add(
                title,
                priority,
                endDate
            );

            _todoList.Save();

            titleTextBox.Clear();

            priorityNumeric.Value = 3;

            endDatePicker.Value =
                DateTime.Today;

            RefreshList();

            titleTextBox.Focus();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Ошибка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }

    private void CompleteButton_Click(
        object? sender,
        EventArgs e)
    {
        TodoItem? item =
            GetSelectedItem();

        if (item == null)
        {
            MessageBox.Show(
                "Выберите задачу.",
                "Выполнение",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            return;
        }
        _todoList.Complete(item);
        _todoList.Save();
        RefreshList();
    }

    private void DeleteButton_Click(
        object? sender,
        EventArgs e)
    {
        TodoItem? item =
            GetSelectedItem();

        if (item == null)
        {
            MessageBox.Show(
                "Выберите задачу.",
                "Удаление",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            return;
        }
        DialogResult result =
            MessageBox.Show(
                $"Удалить задачу \"{item.Title}\"?",
                "Удаление",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );
        if (result != DialogResult.Yes)
        {
            return;
        }
        _todoList.Remove(item);

        _todoList.Save();

        RefreshList();
    }

    private void RenameButton_Click(
        object? sender,
        EventArgs e)
    {
        TodoItem? item =
            GetSelectedItem();

        if (item == null)
        {
            MessageBox.Show(
                "Выберите задачу.",
                "Переименование",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            return;
        }


        string newTitle =
            ShowInputDialog(
                "Переименовать задачу:",
                item.Title
            );


        if (string.IsNullOrWhiteSpace(newTitle))
        {
            return;
        }


        try
        {
            _todoList.Rename(
                item,
                newTitle
            );

            _todoList.Save();

            RefreshList();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Ошибка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }

    private TodoItem? GetSelectedItem()
    {
        if (listView.SelectedItems.Count == 0)
        {
            return null;
        }

        return listView
            .SelectedItems[0]
            .Tag as TodoItem;
    }

    private void RefreshList()
    {
        if (listView == null)
        {
            return;
        }


        listView.BeginUpdate();

        try
        {
            listView.Items.Clear();

            string statusFilter =
                statusFilterCombo
                    .SelectedItem?
                    .ToString()
                ?? "Все";
            IEnumerable<TodoItem> items =
                _todoList.GetByStatus(
                    statusFilter
                );

            string dateFilter =
                dateFilterCombo
                    .SelectedItem?
                    .ToString()
                ?? "Все даты";

            items =
                FilterByDate(
                    items,
                    dateFilter
                );

            items =
                _todoList.Search(
                    items,
                    searchTextBox.Text
                );

            string sort =
                sortCombo
                    .SelectedItem?
                    .ToString()
                ?? "По срочности";


            items = sort switch
            {
                "По приоритету" =>
                    _todoList.SortByPriority(
                        items
                    ),

                "По дате конца" =>
                    _todoList.SortByEndDate(
                        items
                    ),

                "По названию" =>
                    _todoList.SortByTitle(
                        items
                    ),

                _ =>
                    _todoList.SortByUrgency(
                        items
                    )
            };

            foreach (TodoItem item in items)
            {
                AddItemToList(item);
            }
        }
        finally
        {
            listView.EndUpdate();
        }
    }

    private IEnumerable<TodoItem> FilterByDate(
        IEnumerable<TodoItem> items,
        string filter)
    {
        DateTime today =
            DateTime.Today;


        return filter switch
        {
            "Сегодня" =>
                items.Where(item =>
                    item.EndDate.Date ==
                    today
                ),

            "Завтра" =>
                items.Where(item =>
                    item.EndDate.Date ==
                    today.AddDays(1)
                ),

            "Следующие 7 дней" =>
                items.Where(item =>
                    item.EndDate.Date >= today &&
                    item.EndDate.Date <=
                    today.AddDays(7)
                ),

            "Просроченные" =>
                items.Where(item =>
                    !item.IsDone &&
                    item.EndDate.Date < today
                ),

            _ =>
                items
        };
    }

    private void AddItemToList(
        TodoItem item)
    {
        ListViewItem row =
            new ListViewItem(
                item.Title
            );

        string status;

        if (item.IsDone)
        {
            status = "Выполнена";
        }
        else if (item.IsOverdue())
        {
            status = "Просрочена";
        }
        else
        {
            status = "Активна";
        }

        row.SubItems.Add(
            status
        );

        row.SubItems.Add(
            item.Priority.ToString()
        );

        row.SubItems.Add(
            item.EndDate.ToString(
                "dd.MM.yyyy"
            )
        );

        row.SubItems.Add(
            item.GetDaysLeftText()
        );

        row.SubItems.Add(
            $"{item.GetUrgency() * 100:F0}%"
        );
        row.Tag = item;
        Color taskColor =
            CalculateTaskColor(item);
        row.BackColor =
            taskColor;
        if (item.IsDone)
        {
            row.ForeColor =
                Color.Gray;

            row.Font =
                new Font(
                    listView.Font,
                    FontStyle.Strikeout
                );
        }
        else
        {
            row.ForeColor =
                Color.Black;

            row.Font =
                new Font(
                    listView.Font,
                    FontStyle.Regular
                );
        }
        listView.Items.Add(row);
    }

    private Color CalculateTaskColor(
        TodoItem item)
    {
        if (item.IsDone)
        {
            return Color.FromArgb(
                225,
                225,
                225
            );
        }

        double urgency =
            item.GetUrgency();
        urgency =
            Math.Clamp(
                urgency,
                0.0,
                1.0
            );

        Color green =
            Color.FromArgb(
                170,
                235,
                150
            );

        Color yellow =
            Color.FromArgb(
                255,
                235,
                130
            );

        Color red =
            Color.FromArgb(
                255,
                130,
                130
            );

        if (urgency <= 0.5)
        {
            double t =
                urgency / 0.5;


            return InterpolateColor(
                green,
                yellow,
                t
            );
        }

        double redT =
            (urgency - 0.5) / 0.5;


        return InterpolateColor(
            yellow,
            red,
            redT
        );
    }

    private Color InterpolateColor(
        Color first,
        Color second,
        double t)
    {
        t =
            Math.Clamp(
                t,
                0.0,
                1.0
            );


        int r =
            (int)(
                first.R +
                (second.R - first.R) * t
            );


        int g =
            (int)(
                first.G +
                (second.G - first.G) * t
            );


        int b =
            (int)(
                first.B +
                (second.B - first.B) * t
            );


        return Color.FromArgb(
            r,
            g,
            b
        );
    }

    private string ShowInputDialog(
        string text,
        string defaultValue)
    {
        using Form form =
            new Form();


        form.Text =
            "Переименовать";


        form.Size =
            new Size(
                400,
                160
            );


        form.StartPosition =
            FormStartPosition.CenterParent;


        form.FormBorderStyle =
            FormBorderStyle.FixedDialog;


        form.MinimizeBox = false;
        form.MaximizeBox = false;


        Label label =
            new Label();

        label.Text = text;

        label.Location =
            new Point(
                15,
                15
            );

        label.AutoSize = true;

        form.Controls.Add(label);


        TextBox textBox =
            new TextBox();

        textBox.Text =
            defaultValue;

        textBox.Location =
            new Point(
                15,
                45
            );

        textBox.Width = 350;

        form.Controls.Add(textBox);


        Button okButton =
            new Button();

        okButton.Text =
            "OK";

        okButton.DialogResult =
            DialogResult.OK;

        okButton.Location =
            new Point(
                200,
                80
            );

        form.Controls.Add(okButton);


        Button cancelButton =
            new Button();

        cancelButton.Text =
            "Отмена";

        cancelButton.DialogResult =
            DialogResult.Cancel;

        cancelButton.Location =
            new Point(
                285,
                80
            );

        form.Controls.Add(cancelButton);


        form.AcceptButton =
            okButton;

        form.CancelButton =
            cancelButton;


        textBox.SelectAll();

        textBox.Focus();


        if (form.ShowDialog(this) ==
            DialogResult.OK)
        {
            return textBox.Text.Trim();
        }


        return "";
    }

    protected override void OnFormClosing(
        FormClosingEventArgs e)
    {
        try
        {
            _todoList.Save();
        }
        catch
        {
        }
        if (refreshTimer != null)
        {
            refreshTimer.Stop();
            refreshTimer.Dispose();
        }
        base.OnFormClosing(e);
    }
}