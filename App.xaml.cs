using BT_BO.Buttons;
using BT_BO.Buttons.Menu;
using BT_BO.Buttons.Reports;
using BT_BO.RepositoryImpl;
using BT_BO.Splash;
using BT_BO.Views;
using BT_BO.Views.Reports;
using BT_COMMONS;
using BT_COMMONS.App;
using BT_COMMONS.DataRepositories;
using BT_COMMONS.Helpers;
using BT_COMMONS.Operators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BT_BO;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IHost? AppHost { get; private set; }

    public static List<HomeButton> HomeButtons;
    public static List<ReportSelectionButton> ReportSelectionButtons;

    public App()
    {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                IConfigurationBuilder builder = new ConfigurationBuilder();
                builder.AddJsonFile("AppSettings.json");

                IConfiguration config = builder.Build();

                services.AddSingleton<IConfiguration>(provider => config);

                services.AddSingleton<DatabaseAccess>(x => new DatabaseAccess(config["LocalConnectionString"], config["ControllerConnectionString"]));
                services.AddSingleton<IOperatorRepository, OperatorRepository>();
                services.AddSingleton<IButtonRepository, ButtonRepository>();

                services.AddSingleton<MainWindow>();
                services.AddViewFactory<LoginView>();
                services.AddViewFactory<HomeView>();

                services.AddViewFactory<ReportSelectionView>();

                services.AddSingleton<BOController>();
            }).Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        try
        {
            BOSplashScreen splash = new BOSplashScreen();
            splash.Show();

            splash.StatusText.Text = "Starting AppHost";
            await AppHost!.StartAsync();

            var controller = AppHost.Services.GetRequiredService<BOController>();

            splash.StatusText.Text = "Loading data";
            // Load data.json
            try
            {
                using (StreamReader r = new StreamReader("C:\\bubbletill\\data.json"))
                {
                    string json = r.ReadToEnd();
                    AppConfig config = JsonConvert.DeserializeObject<AppConfig>(json);

                    if (config == null || config.Register == null || config.Store == null || config.RegisterOpen == null)
                    {
                        throw new Exception();
                    }

                    controller.StoreNumber = (int)config.Store;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bubbletill failed to launch:\nFailed to load data.json", "Bubbletill Back Office", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None);
                Shutdown();
                return;
            }

            // Load groups
            splash.StatusText.Text = "Setting up operator groups";
            var operRepo = AppHost.Services.GetRequiredService<IOperatorRepository>();
            var operGroups = await operRepo.GetOperatorGroups();
            foreach (var group in operGroups)
            {
                group.Parse();
                controller.OperatorGroups.Add(group.Id, group);
            }

            // Load buttons
            splash.StatusText.Text = "Setting up configured buttons";
            var btnRepo = AppHost.Services.GetRequiredService<IButtonRepository>();

            HomeButtons = await btnRepo.GetHomeButtons();
            ReportSelectionButtons = await btnRepo.GetReportSelectionButtons();

            // Start BO
            splash.StatusText.Text = "Starting Back Office...";
            var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
            splash.Close();

            base.OnStartup(e);
        } catch (Exception ex)
        {
            MessageBox.Show("Back Office failed to launch:\n" + ex, "Bubbletill Back Office", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None);
            Shutdown();
            return;
        }
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();

        base.OnExit(e);
    }

    public static Button CreateButton(IButtonData buttonData, Style buttonStyle)
    {
        var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
        var controller = AppHost.Services.GetRequiredService<BOController>();
        Button button = new Button();
        button.Style = buttonStyle;
        button.Content = buttonData.Name;
        button.Click += (s, e) =>
        {
            if (controller.CurrentOperator.HasBoolPermission(buttonData.Permission))
            {
                buttonData.OnClick(mainWindow);
            }
            else
            {
                controller.HeaderError("Insufficient permission.");
            }
        };

        return button;
    }
}
