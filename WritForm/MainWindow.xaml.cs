using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WritForm;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public ObservableCollection<string> MyItems { get; set; }

    public MainWindow()
    {
        InitializeComponent();

        // Initialize data
        MyItems = new ObservableCollection<string>
            {
                "Item 1", "Item 2", "Item 3",
                "Item 4", "Item 5", "Item 6",
                "Item 7", "Item 8", "Item 9"
            };

        // Bind data to ListBox
        DataContext = this;
    }

    // Method to add a new item
    private void AddItem_Click(object sender, RoutedEventArgs e)
    {
        MyItems.Add("New Item " + (MyItems.Count + 1));
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        MaximizeWithoutCoveringTaskbar();
    }

    private void MaximizeWithoutCoveringTaskbar()
    {
        //var workingArea = SystemParameters.WorkArea;
        //this.Left = workingArea.Left;
        //this.Top = workingArea.Top;
        //this.Width = workingArea.Width;
        //this.Height = workingArea.Height;


        this.Left = 433.0;
        this.Top = 134.0;
        this.Width = 500.0;
        this.Height = 500.0;

    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        var storyboard = (Storyboard)FindResource("CloseStoryboard");
        storyboard.Completed += (s, a) => App.CloseApp();
        storyboard.Begin(this);
    }

    private void MinimizeButton_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        Keyboard.ClearFocus();
    }

    private async void checkupdate_Click(object sender, RoutedEventArgs e)
    {
        string availableVersionFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "available.version");

        bool shouldDownload = true;

        if (File.Exists(availableVersionFilePath))
        {
            DateTime lastModified = File.GetLastWriteTime(availableVersionFilePath);
            if (lastModified > DateTime.Now.AddDays(-1))
            {
                shouldDownload = false;
            }
        }

        if (shouldDownload)
        {
            await Task.Run(() =>
            {
                Process.Start(AppDomain.CurrentDomain.BaseDirectory + "installer.exe",
                    "\"https://github.com/SuperCdat/variables/blob/main/available.version?raw=true\" \"available.version\"");
            });

            // Wait for the download to complete
            await Task.Delay(2000); // Adjust the delay as needed
        }

        try
        {
            string currentVersion = File.ReadAllLines(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "current.version"))[0];
            string availableVersion = File.ReadAllLines(availableVersionFilePath)[0];

            if (currentVersion.Equals(availableVersion))
            {
                MessageBox.Show("Ứng Dụng Là Bản Mới Nhất: " + currentVersion, "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                
                if (MessageBox.Show("Đã Có Bản Cập Nhật: " + availableVersion + "\nCài Đặt Ngay?", "", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
                {

                }
            }
        }
        catch
        {
            MessageBox.Show("Không thể tải số phiên bản.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
        }



    }
}
