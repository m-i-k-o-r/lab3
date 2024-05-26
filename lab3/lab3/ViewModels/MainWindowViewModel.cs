using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;


public class MainWindowViewModel : BindableBase
{
    private object? _classFromLab2;
    private String _route = "";
    public List<string> NameClass { get; set; }
    public string Type { get; set; } = "";
    private string _data = "животное не выбрано";
    
    public MainWindowViewModel()
    {
        NameClass = new List<string>();
        Data = _data;
    }
    public String Route
    {
        get => _route;
        set
        {
            _route = value;
            GetListClass();
        }
    }

    public void GetListClass()
    {
        NameClass = new List<string>();

        string assemblyPath = _route;

        Assembly assembly = Assembly.LoadFrom(assemblyPath);

        Type[] types = assembly.GetTypes();

        var modelsTypes = types.Where(t => t.Namespace == "lab2_6");
        foreach (Type type in types) 
        {
            NameClass.Add(type.FullName);
        }
    }

    public void CreateClass(String className)
    {
        Assembly assembly = Assembly.LoadFrom(_route);

        Type? classType = assembly.GetType(className);

        var listType = typeof(List<>);

        Type = classType.FullName;

        if (classType != null)
        {

            switch (classType.FullName)
            {
                case "lab2_6.Dog":
                    _classFromLab2 = Activator.CreateInstance(type: classType);
                    Data = $"Выбрана Собака. \nУ нее максимальная скорость равна {MethodFromClass("GetMaxSpeed")}. \nНе может лазить по деревьям и Может подать голос";
                    break;
                case "lab2_6.Panther":
                    _classFromLab2 = Activator.CreateInstance(type: classType);
                    Data = $"Выбрана Пантера. \nУ нее максимальная скорость равна {MethodFromClass("GetMaxSpeed")}. \nМожет лазить по деревьям и Может подать голос";
                    break;
                case "lab2_6.Turtle":
                    _classFromLab2 = Activator.CreateInstance(type: classType);
                    Data = $"Выбрана Черепаха. \nУ нее максимальная скорость равна {MethodFromClass("GetMaxSpeed")}. \nНе может лазить по деревьям и Не может подать голос";
                    break;
            }
            Console.WriteLine("Успешно");
        }
        else
        {
            Console.WriteLine("Класс не найден");
        }

    }

    public object MethodFromClass(string nameMethod)
    {
        try
        {
            MethodInfo method = _classFromLab2.GetType().GetMethod(nameMethod);

            if (method == null)
            {
                throw new MissingMethodException("Метод не найден.");
            }
            else
            {
                ParameterInfo[] methodParameters = method.GetParameters();
                object returnValue = method.Invoke(_classFromLab2, null);

                if (method.ReturnType != typeof(void))
                {
                    Console.WriteLine($"Результат выполнения метода: {method} - {returnValue}");
                }

                return returnValue;

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка выполнения метода: {ex.Message}");
            return null;
        }
    }

    public void Run()
    {
        Data = (string) MethodFromClass("Run");
    }

    public void Stop()
    {
        Data = (string)MethodFromClass("Stop");
    }

    public void Voice()
    {
        Data = (string)MethodFromClass("GetVoice");
    }

    public void Up()
    {
        Data = (string)MethodFromClass("GoUp");
    }

    public void Down()
    {
        Data = (string)MethodFromClass("GoDown");
    }

    public string Data
    {
        get => _data;
        set => SetProperty(ref _data, value);
    }
}