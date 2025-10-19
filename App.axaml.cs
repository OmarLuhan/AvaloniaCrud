using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Crud.Data;
using Crud.Models;
using Crud.Services;
using Crud.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Crud;

public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; } = null!;
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        ConfigureServices();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
            desktop.Exit += OnExit;
        }

        base.OnFrameworkInitializationCompleted();
    }
    private void OnExit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        (ServiceProvider as IDisposable)?.Dispose();
    }
    private static void ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddDbContext<CrudDbContext>();
        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddTransient<IPersonService,PersonService>(); 
        services.AddTransient<PersonViewModel>();
        ServiceProvider = services.BuildServiceProvider();
    }
}