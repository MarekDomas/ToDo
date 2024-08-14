using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ToDo.Classes
{
    public static class Functions
    {
        private static string path = Path.Combine(FileSystem.AppDataDirectory, "tasks.xml");

        public static void SaveTasks()
        {
            var serializer = new XmlSerializer(typeof(List<TaskToDo>));
            using (var writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, ListService.Tasks);
            }
        }
        public static void LoadTasks()
        {
            if (!File.Exists(path))
            {
                return;
            }

            var serializer = new XmlSerializer(typeof(List<TaskToDo>));
            using (var reader = new StreamReader(path))
            {
                ListService.Tasks = (List<TaskToDo>)serializer.Deserialize(reader);
            }
        }

        public static void RefreshListView(ListView lv)
        {
            LoadTasks();
            lv.ItemsSource = null;
            lv.ItemsSource = ListService.Tasks;
        }
    }
}
