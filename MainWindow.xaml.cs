using BT_BO.Views;
using BT_COMMONS.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BT_BO;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        BOParentErrorBox.Visibility = Visibility.Hidden;
        BOViewContainer.ContentChanged += ViewContentChanged;

        BOController controller = App.AppHost.Services.GetRequiredService<BOController>();
        BOParentHeader_Store.Text = "Store# " + controller.StoreNumber;

        BOViewContainer.Content = App.AppHost.Services.GetRequiredService<LoginView>();
    }

    public void HeaderError(string? error = null)
    {
        if (error == null)
        {
            BOParentErrorBox.Visibility = Visibility.Hidden;
            BOParentErrorBoxText.Text = "";
            return;
        }

        BOParentErrorBoxText.Text = error;
        BOParentErrorBox.Visibility = Visibility.Visible;
    }

    private void ViewContentChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        HeaderError();
    }
}
