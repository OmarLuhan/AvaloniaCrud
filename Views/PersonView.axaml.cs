using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Crud.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Crud.Views;

public partial class PersonView : UserControl
{
    public PersonView()
    {
        InitializeComponent();
        DataContext = App.ServiceProvider.GetRequiredService<PersonViewModel>();
    }
}