#region copyright

/*****************************************************************************************
*                                     ______________________________________________     *
*                              o O   |                                              |    *
*                     (((((  o      <                  SettingsKit                  |    *
*                    ( o o )         |______________________________________________|    *
* ------------oOOO-----(_)-----OOOo----------------------------------------------------- *
*             Project: WpfDemo                                                           *
*            Filename: RelayCommand.cs                                                   *
*              Author: Stanley Omoregie                                                  *
*        Created Date: 09.02.2026                                                        *
*       Modified Date: 09.02.2026                                                        *
*          Created By: Stanley Omoregie                                                  *
*    Last Modified By: Stanley Omoregie                                                  *
*           CopyRight: copyright © 2026 Omotech Digital Solutions                        *
*                  .oooO  Oooo.                                                          *
*                  (   )  (   )                                                          *
* ------------------\ (----) /---------------------------------------------------------- *
*                    \_)  (_/                                                            *
*****************************************************************************************/

#endregion copyright

using System.Windows.Input;

namespace WpfDemo.Commands;

/// <summary>
/// A generic relay command implementation that implements the ICommand interface.
/// This command allows for action-based command execution with optional parameter support.
/// </summary>
public class RelayCommand : ICommand
{
    private readonly Action<object?> _execute;
    private readonly Predicate<object?>? _canExecute;

    /// <summary>
    /// Occurs when changes occur that affect whether the command can be executed.
    /// </summary>
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    /// <summary>
    /// Initializes a new instance of the RelayCommand class.
    /// </summary>
    /// <param name="execute">The action to execute when the command is invoked.</param>
    public RelayCommand(Action<object?> execute)
        : this(execute, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the RelayCommand class.
    /// </summary>
    /// <param name="execute">The action to execute when the command is invoked.</param>
    /// <param name="canExecute">The predicate to determine if the command can be executed.</param>
    private RelayCommand(Action<object?> execute, Predicate<object?>? canExecute)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    /// <summary>
    /// Determines whether the command can be executed in its current state.
    /// </summary>
    /// <param name="parameter">The command parameter.</param>
    /// <returns>True if the command can be executed; otherwise, false.</returns>
    public bool CanExecute(object? parameter)
    {
        return _canExecute is null || _canExecute(parameter);
    }

    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="parameter">The command parameter.</param>
    public void Execute(object? parameter)
    {
        _execute(parameter);
    }
}

/// <summary>
/// A generic relay command implementation that implements the ICommand interface with a generic parameter type.
/// </summary>
/// <typeparam name="T">The type of the command parameter.</typeparam>
public class RelayCommand<T> : ICommand
{
    private readonly Action<T?> _execute;
    private readonly Predicate<T?>? _canExecute;

    /// <summary>
    /// Occurs when changes occur that affect whether the command can be executed.
    /// </summary>
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    /// <summary>
    /// Initializes a new instance of the RelayCommand&lt;T&gt; class.
    /// </summary>
    /// <param name="execute">The action to execute when the command is invoked.</param>
    /// <param name="canExecute">The predicate to determine if the command can be executed.</param>
    public RelayCommand(Action<T?> execute, Predicate<T?>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    /// <summary>
    /// Determines whether the command can be executed in its current state.
    /// </summary>
    /// <param name="parameter">The command parameter.</param>
    /// <returns>True if the command can be executed; otherwise, false.</returns>
    public bool CanExecute(object? parameter)
    {
        try
        {
            return _canExecute is null || _canExecute((T?)parameter);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="parameter">The command parameter.</param>
    public void Execute(object? parameter)
    {
        _execute((T?)parameter);
    }
}

