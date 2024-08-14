using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Classes;

public class TaskToDo
{
    public string TaskName { get; set; }
    public string TaskDescription { get; set; }
    public DateTime TaskDate { get; set; }
    public TimeSpan TaskTime { get; set; }
    public bool TaskStatus { get; set; } 
    public TaskToDo(string taskName, string taskDescription, DateTime taskDate, TimeSpan taskTime, bool taskStatus)
    {
        TaskName = taskName;
        TaskDescription = taskDescription;
        TaskDate = taskDate;
        TaskStatus = taskStatus;
        TaskTime = taskTime;
    }
    public TaskToDo() { }
}