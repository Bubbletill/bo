using BT_BO.Buttons;
using BT_BO.Buttons.Menu;
using BT_BO.Views.Reports;
using BT_COMMONS.Operators;
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

namespace BT_BO.Views;

/// <summary>
/// Interaction logic for HomeView.xaml
/// </summary>
public partial class HomeView : UserControl
{
    private readonly MainWindow _mainWindow;
    private readonly BOController _controller;

    private readonly Style _buttonStyle;

    public HomeView(MainWindow mainWindow, BOController controller)
    {
        _mainWindow = mainWindow;
        _controller = controller;

        InitializeComponent();
        _buttonStyle = FindResource("BTVerticleButton") as Style;

        LoadButtons(App.HomeButtons);
        Button button = new Button();
        button.Style = _buttonStyle;
        button.Content = "Sign-out";
        button.Click += (s, e) =>
        {
            _controller.Logout();
        };
        ButtonStackPanel.Children.Add(button);
    }

    public void LoadButtons(List<HomeButton> buttons)
    {
        ButtonStackPanel.Children.Clear();
        buttons.ForEach(type =>
        {
            IButtonData buttonData = GetButtonFunction(type);
            if (!_controller.CurrentOperator!.HasBoolPermission(buttonData.Permission))
                return;

            ButtonStackPanel.Children.Add(App.CreateButton(GetButtonFunction(type), _buttonStyle));
        });
        ButtonStackPanel.InvalidateVisual();
        ButtonStackPanel.UpdateLayout();
    }

    public IButtonData GetButtonFunction(HomeButton button)
    {
        switch (button)
        {
            case HomeButton.REPORTS:
                {
                    return new ButtonData
                    {
                        Name = "Reports",
                        Permission = OperatorBoolPermission.BO_Reports_Access,
                        OnClick = w =>
                        {
                            _mainWindow.BOViewContainer.Content = App.AppHost.Services.GetRequiredService<ReportSelectionView>();
                            return;
                        }
                    };
                }

            case HomeButton.CASH_MANAGEMENT:
                {
                    return new ButtonData
                    {
                        Name = "Cash Management",
                        Permission = OperatorBoolPermission.BO_CashManagement_Access,
                        OnClick = w =>
                        {
                            return;
                        }
                    };
                }

            case HomeButton.EMPLOYEE_MANAGEMENT:
                {
                    return new ButtonData
                    {
                        Name = "Employee Management",
                        Permission = OperatorBoolPermission.BO_EmployeeManagement_Access,
                        OnClick = w =>
                        {
                            return;
                        }
                    };
                }

            case HomeButton.STORE_MANAGEMENT:
                {
                    return new ButtonData
                    {
                        Name = "Store Configuration",
                        Permission = OperatorBoolPermission.BO_StoreManagement_Access,
                        OnClick = w =>
                        {
                            return;
                        }
                    };
                }

            case HomeButton.SD_OPERATIONS:
                {
                    return new ButtonData
                    {
                        Name = "Sales Operations",
                        Permission = OperatorBoolPermission.BO_SDOperations_Access,
                        OnClick = w =>
                        {
                            return;
                        }
                    };
                }

            default:
                {
                    return null;
                }
        }
    }
}
