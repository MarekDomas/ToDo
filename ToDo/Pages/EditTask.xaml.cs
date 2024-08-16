using ToDo.Classes;

namespace ToDo.Pages;

public partial class EditTask : ContentPage
{
    TaskToDo taskToEdit;
    bool isEdit;

    //Konstruktor má výchozí parametr taskToEdit, který je null, když není znamená to, že se edituje existující úkol
	public EditTask(TaskToDo? taskToEdit = null)
	{
		InitializeComponent();

        if (taskToEdit is null)
        {
            return;
        }
        loadTask(taskToEdit);
	}

    private void SubmitBtn_OnClicked(object? sender, EventArgs e)
    {
        if(string.IsNullOrWhiteSpace(NameEntry.Text))
        {
            DisplayAlert("Error", "Zadejte název úkolu", "Ok");
            return;
        }

        if (isEdit)//Editace úkolu
        {
            taskToEdit.TaskName = NameEntry.Text;
            taskToEdit.TaskDescription = DescriptionEntry.Text;
            taskToEdit.TaskDate = TaskDatePicker.Date;
            taskToEdit.TaskTime = TaskTimePicker.Time;
        }
        else//Vytváøení úkolu
        {

            TaskToDo newTask = new(NameEntry.Text, DescriptionEntry.Text ,TaskDatePicker.Date, TaskTimePicker.Time ,false);
            ListService.Tasks.Add(newTask);
        }
        Functions.SaveTasks();
        Navigation.PopAsync();
    }

    private async void DeleteBtn_OnClicked(object? sender, EventArgs e)
    {
        var answer = await DisplayAlert("Smazat", "Chcete smazat úkol", "Ano", "Ne");

        if (!answer)
        {
            return;
        }

        ListService.Tasks.Remove(taskToEdit);
        Functions.SaveTasks();
        Navigation.PopAsync();
    }

    private void loadTask(TaskToDo task)
    {
        isEdit = true;
        taskToEdit = task;
        SubmitBtn.Text = "Upravit úkol";

        NameEntry.Text = taskToEdit.TaskName;
        DescriptionEntry.Text = taskToEdit.TaskDescription;
        TaskDatePicker.Date = taskToEdit.TaskDate;
        TaskTimePicker.Time = taskToEdit.TaskTime;

        var deleteBtn = new Button
        {
            Text = "Smazat úkol",
            BackgroundColor = Colors.Red,
        };

        deleteBtn.Clicked += DeleteBtn_OnClicked;

        MainLayout.Children.Add(deleteBtn);
    }
}