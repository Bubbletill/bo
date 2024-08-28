using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BT_BO.Splash;

/// <summary>
/// Interaction logic for BOSplashScreen.xaml
/// </summary>
public partial class BOSplashScreen : Window
{
    public BOSplashScreen()
    {
        InitializeComponent();
        VersionText.Text = "Back Office Version " + FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
    }
}
