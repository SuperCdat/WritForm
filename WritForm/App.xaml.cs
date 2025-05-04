using System.Configuration;
using System.Data;
using System.Windows;

namespace WritForm;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static void CloseApp()
    {
        Application.Current.Shutdown();
    }
}

