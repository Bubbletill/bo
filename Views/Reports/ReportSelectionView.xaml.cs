using BT_BO.Buttons;
using BT_BO.Buttons.Menu;
using BT_BO.Buttons.Reports;
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

namespace BT_BO.Views.Reports;

/// <summary>
/// Interaction logic for ReportSelectionView.xaml
/// </summary>
public partial class ReportSelectionView : UserControl
{
    private readonly MainWindow _mainWindow;
    private readonly BOController _controller;

    private readonly Style _buttonStyle;

    public ReportSelectionView(MainWindow mainWindow, BOController controller)
    {
        _mainWindow = mainWindow;
        _controller = controller;

        InitializeComponent();
        _buttonStyle = FindResource("BTVerticleButton") as Style;

        LoadButtons(App.ReportSelectionButtons);
        Button button = new Button();
        button.Style = _buttonStyle;
        button.Content = "Back";
        button.Click += (s, e) =>
        {
            _mainWindow.BOViewContainer.Content = App.AppHost.Services.GetRequiredService<HomeView>();
        };
        ButtonStackPanel.Children.Add(button);
    }

    public void LoadButtons(List<ReportSelectionButton> buttons)
    {
        ButtonStackPanel.Children.Clear();
        buttons.ForEach(type =>
        {
            ButtonStackPanel.Children.Add(App.CreateButton(GetButtonFunction(type), _buttonStyle));
        });
        ButtonStackPanel.InvalidateVisual();
        ButtonStackPanel.UpdateLayout();
    }

    public IButtonData GetButtonFunction(ReportSelectionButton button)
    {
        switch (button)
        {
            case ReportSelectionButton.EJOURNAL:
                {
                    return new ButtonData
                    {
                        Name = "E-Journal",
                        Permission = null,
                        OnClick = w =>
                        {
                            return;
                        }
                    };
                }

            case ReportSelectionButton.REGISTERSALES:
                {
                    return new ButtonData
                    {
                        Name = "Register Sales",
                        Permission = null,
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
