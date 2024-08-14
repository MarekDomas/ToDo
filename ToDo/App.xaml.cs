using System.Xml.Serialization;
using ToDo.Classes;

namespace ToDo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            

            MainPage = new AppShell();
        }

    }
}
