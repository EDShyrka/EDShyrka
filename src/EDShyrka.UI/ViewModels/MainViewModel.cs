using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace EDShyrka.UI.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private int _counter = 0;

    public MainViewModel()
    {

    }

    public string Greeting => "Welcome to Avalonia!";

    [ObservableProperty]
    private string _labelText = "Hello, World!";

    [RelayCommand]
    public void ClickMe()
    {
        LabelText = $"Clicked {++_counter} times !";
    }
}
