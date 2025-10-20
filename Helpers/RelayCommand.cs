using System;
using System.Windows.Input;

namespace Crud.Helpers;

public class RelayCommand(Action execute, Func<bool>? canExecute = null):ICommand
{
    public bool CanExecute(object? parameter)
    {
        return canExecute?.Invoke() ?? true;
    }

    public void Execute(object? parameter)
    {
        execute();
    }

    public event EventHandler? CanExecuteChanged;
    
}