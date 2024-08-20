# ToDo

V této části bude zdokumentována To do aplikace vytvořená v .NET MAUI. 

V To do aplikaci potřebujete vytvářet úkoly, upravovat je, mazat a mít přehled vytvořených úkolů. 
Tato aplikace má dvě stránky jednu na přehled úkolů a druhou na vytváření/upravování úkolů. 

 

UI této aplikace je určeno spíše pro mobilní zařízení ale dá se také použít na PC. 
 

Na hlavní stránce je je pouze ListView a tlačítko pro otevření stránky na vytvoření úkolů. 
 
Nejzásadnější částí je ListView které má upravené zobrazení jednotlivého řádku. 
V ListView lze upravit jak bude vypadat jednotlivý řádek. 


V tomto příkladu je každý řádek AbsoluteLayout který zobrazuje název úkolu a CheckBox který značí jestli byl úkol dokončen. 


<ListView 

    x:Name="TaskLv" 

    Margin="20" 

    AbsoluteLayout.LayoutBounds="0,0,1,0.9" 

    AbsoluteLayout.LayoutFlags="SizeProportional" 

    ItemSelected="TaskLv_ItemSelected" 

    SeparatorVisibility="None"> 

    <ListView.ItemTemplate> 

        <DataTemplate> 

            <ViewCell> 

                <AbsoluteLayout> 

                    <Label 

                        AbsoluteLayout.LayoutBounds="0.01,0,205,45" 

                        AbsoluteLayout.LayoutFlags="PositionProportional" 

                        HorizontalOptions="Start" 

                        Text="{Binding TaskName}" 

                        VerticalOptions="Center" /> 

                    <CheckBox 

                        AbsoluteLayout.LayoutBounds="1,0,65,45" 

                        AbsoluteLayout.LayoutFlags="PositionProportional" 

                        HorizontalOptions="End" 

                        IsChecked="{Binding TaskStatus}" 

                        VerticalOptions="Center" /> 

                </AbsoluteLayout> 

            </ViewCell> 

        </DataTemplate> 

    </ListView.ItemTemplate> 

</ListView> 

 

Každý element má nastavený LayoutFlag na PositionProportional, to znamená že pozice elementu se bude měnit podle šířky obrazovky. Label s názvem úkolu bude vždy v levo, protože má nastavené číslo pro pozici na ose X na 0.01 v LayoutBounds a CheckBox bude vždy v pravo. 

 

Protože ListView bude zobrazovat atributy objektů tak mají hodnoty Labelu a CheckBoxu Binding na jejich atributy, aby to ale fungovalo tak se musí nastavit ItemSource ListView, ten se ale v této aplikaci nastavuje přes C#. 

 

V souboru MainPage.xaml.cs je přepsaná funkce OnAppearing()  

 

    protected override void OnAppearing() 

    { 

        Functions.RefreshListView(TaskLv); 

        base.OnAppearing(); 

    } 

 

Tato funkce se volá vždy když se stránka objeví to znamená při prvním objevení a při příchodu z jiné stránky.  

V těle funkce se volá další funkce která načte z lokálního úložiště vytvořené úkoly a uloží je do statického Listu ve statické třídě ListService 

 

        public static void RefreshListView(ListView lv) 

        { 

            LoadTasks(); 

            lv.ItemsSource = null; 

            lv.ItemsSource = ListService.Tasks; 

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

                ListService.Tasks =                                      (List<TaskToDo>)serializer.Deserialize(reader); 

            } 

        } 

 

//Funkce serializuje List do xml souboru 

public static void SaveTasks() 

{ 

     var serializer = new XmlSerializer(typeof(List<TaskToDo>)); 

     using (var writer = new StreamWriter(path)) 

     { 

         serializer.Serialize(writer, ListService.Tasks); 

     } 

} 

 

Tlačítko na vytváření úkolů otevře novou stránku: 

 

    private void TaskBtn_OnClicked(object? sender, EventArgs e) 

    { 

        Navigation.PushAsync(new EditTask()); 

    } 

Způsobů navigací mezi stránkami je v MAUI více v tomto případě je použita NavigationPage (NavigationPage - .NET MAUI | Microsoft Learn) To znamené že se posouváte mezi stránkami dopředu/dozadu a funguje na principu LIFO(last in first out). 

 

Znázornění vypadá takto:  


 


 

 

Struktura stránky EditTask.xaml je jednoduchá je to pouze VerticalStackLaoyut a v něm potřebné vstupy pro vytvoření/úpravu úkolu 

 

 

    <VerticalStackLayout 

        x:Name="MainLayout" 

        Padding="20" 

        Spacing="20" 

        VerticalOptions="Center"> 

  

        <Label Text="Název úkolu:" /> 

        <Entry x:Name="NameEntry" /> 

        <Label Text="Popis úkolu:" /> 

        <Entry x:Name="DescriptionEntry" /> 

        <Label Text="Datum a čas konce" /> 

        <DatePicker x:Name="TaskDatePicker" /> 

        <TimePicker x:Name="TaskTimePicker" /> 

  

        <Button 

            x:Name="SubmitBtn" 

            Clicked="SubmitBtn_OnClicked" 

            Text="Vytvořit úkol" /> 

  

    </VerticalStackLayout> 

 

Z hodnot těchto vztupů se dá vytvořit objekt typu TaskToDo 

 

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

 

    //Třída musím mít prázdný konstruktor kvůli serializaci 

    public TaskToDo() {} 

} 

 

Tlačítko submit volá funkci SubmitBtn_OnClicked()  

 

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

        else//Vytváření úkolu 

        { 

  

            TaskToDo newTask = new(NameEntry.Text, DescriptionEntry.Text ,TaskDatePicker.Date, TaskTimePicker.Time ,false); 

            ListService.Tasks.Add(newTask); 

        } 

        Functions.SaveTasks(); 

        Navigation.PopAsync(); 

    } 

 

Funkce zkontroluje zdali je zadané jméno a vytvoří úkol, také zkontroluje jestli se jedná o edit podle proměnné isEdit která se nastavuje ve funkci loadTask která se volá v konstruktoru stránky. Pak uloží změny a dá stránku pryč z navigačního stacku. 

 

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

 

Funkce loadTask se volá pokud je konstruktoru předán úkol na upravení, funkce nahraje hodnoty předaného úkolu a vytvoří nové tlačítko pro smazání úkolu které má funkci DeleteBtn_OnClicked.  

 

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

 

Funkce se zeptá uživatele jestli chce smazat úkol a na základě odpovědi ho smaže. 

Funcke také musí bý asynchroní protože když se funkce DisplayAlert nevolá asynchroně tak v případě že je žádaná odpověď od uživatele tak nevrací hodnotu bool ale Task<bool>? . 

 

Pokud uživatel chce upravit úkol tak na něj stačí kliknout a zavolá se Event ListView ItemSelected který zavolá funkci TaskLv_ItemSelected  

 

    private void TaskLv_ItemSelected(object sender, SelectedItemChangedEventArgs args) 

    { 

        var taskToEdit = args.SelectedItem as TaskToDo; 

        Navigation.PushAsync(new EditTask(taskToEdit)); 

    } 

Funkce vezme vybranou položku a předá jí stránce EditTask která provede předchozí probrané funkce. 

 
