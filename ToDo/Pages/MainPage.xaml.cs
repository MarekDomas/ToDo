using System.Diagnostics;
using ToDo.Classes;
using ToDo.Pages;

namespace ToDo;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();     
    }

    protected override void OnAppearing()
    {
        Functions.RefreshListView(TaskLv);
        base.OnAppearing();
    }

    private void TaskBtn_OnClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new EditTask());
    }

    private async void dialog(TaskToDo task)
    {

        var answer = await DisplayAlert("Delete?", "Do you want to delete the task", "Yes", "No");

        if (answer)
        {
            Debug.Assert( ListService.Tasks.Remove(task));
            Functions.RefreshListView(TaskLv);
        }
        Functions.SaveTasks();
    }

    private bool _isUserInteracting;
    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (!_isUserInteracting)
        {
            return;
        }

        var taskStatusBox = sender as CheckBox; 
        
        if (taskStatusBox.IsChecked)
        {
            dialog(taskStatusBox.BindingContext as TaskToDo);
        }
        
        Functions.SaveTasks();
    }

    private void TaskLv_ItemSelected(object sender, SelectedItemChangedEventArgs args)
    {
        TaskToDo taskToEdit = args.SelectedItem as TaskToDo;
        Navigation.PushAsync(new EditTask(taskToEdit));
    }


    private void CheckBox_Focused(object sender, FocusEventArgs e)
    {
        _isUserInteracting = true; 
    }

    private void CheckBox_Unfocused(object sender, FocusEventArgs e)
    {
        _isUserInteracting = false; 
    }
}