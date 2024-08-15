using System.Diagnostics;
using ToDo.Classes;
using ToDo.Pages;
using Plugin.LocalNotification;

namespace ToDo;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        //var request = new NotificationRequest
        //{
        //    NotificationId = 1337,
        //    Title = "MEDIUM",
        //    Subtitle = "Hello! I'm Erdal",
        //    Description = "Local Push Notification",
        //    BadgeNumber = 1,

        //    Schedule = new NotificationRequestSchedule
        //    {
        //        NotifyTime = DateTime.Now.AddSeconds(3),
        //    }
        //};

        //LocalNotificationCenter.Current.Show(request);
    }

    protected override void OnAppearing()
    {
        Functions.RefreshListView(TaskLv);
        base.OnAppearing();
    }

    protected override void OnDisappearing()
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