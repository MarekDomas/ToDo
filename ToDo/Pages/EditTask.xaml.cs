

using System.Diagnostics;
using System.Xml.Serialization;
using ToDo.Classes;

namespace ToDo.Pages;

public partial class EditTask : ContentPage
{
    TaskToDo taskToEdit = null;
    bool isEdit = false;
	public EditTask(TaskToDo taskToEdit = null)
	{
		InitializeComponent();
        if(taskToEdit is not null)
        {
            isEdit = true;
            this.taskToEdit = taskToEdit;
            NameEntry.Text = taskToEdit.TaskName;
            DescriptionEntry.Text = taskToEdit.TaskDescription;
            TaskDatePicker.Date = taskToEdit.TaskDate ;
            TaskTimePicker.Time = taskToEdit.TaskTime;
        }
	}

    private void SubmitBtn_OnClicked(object? sender, EventArgs e)
    {
        if (isEdit)
        {
            taskToEdit.TaskName = NameEntry.Text;
            taskToEdit.TaskDescription = DescriptionEntry.Text;
            taskToEdit.TaskDate = TaskDatePicker.Date;
            taskToEdit.TaskTime = TaskTimePicker.Time;
        }
        else
        {
            TaskToDo newTask = new(NameEntry.Text, DescriptionEntry.Text ,TaskDatePicker.Date, TaskTimePicker.Time ,false);
            ListService.Tasks.Add(newTask);
        }
        Functions.SaveTasks();
        Navigation.PopAsync();
    }
}