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

    override protected void OnDisappearing()
    {
        Functions.SaveTasks();
        base.OnDisappearing();
    }

    private void TaskBtn_OnClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new EditTask());
    }

    private void TaskLv_ItemSelected(object sender, SelectedItemChangedEventArgs args)
    {
        TaskToDo taskToEdit = args.SelectedItem as TaskToDo;
        Navigation.PushAsync(new EditTask(taskToEdit));
    }
}