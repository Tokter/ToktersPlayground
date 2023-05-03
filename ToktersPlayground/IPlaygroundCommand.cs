using Avalonia.Platform.Storage;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToktersPlayground.ViewModels;

namespace ToktersPlayground
{
    /// <summary>
    /// Playground command that can be placed somewhere in the user interface
    /// </summary>
    public interface IPlaygroundCommand : ICommand
    {
        IStorageProvider? StorageProvider { get; set; }
        Task ExecuteAsync(object? paramter);
        void RaiseCanExecuteChanged();
    }

    public enum PlaygroundCommandLocation
    {
        MainMenu,
        ComponentsMenu,
        ComponentsTools,
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class PlaygroundCommandAttribute : Attribute
    {
        public string Name { get; set; } = "Command Name";
        public PlaygroundCommandLocation Location { get; set;}
        public int Order { get; set; } = 100;
        public string? Icon { get; set; }
        public Type? ComponentType { get; set; } = null;

        public PlaygroundCommandAttribute(string name, PlaygroundCommandLocation location, int order = 100, string? icon = null)
        {
            Name = name;
            Location = location;
            Order= order;
            Icon = icon;
        }
    }

    public class PlaygroundCommand : IPlaygroundCommand
    {
        public IStorageProvider? StorageProvider { get; set; }

        public event EventHandler? CanExecuteChanged;

        public virtual bool CanExecute(object? parameter)
        {
            throw new NotImplementedException();
        }

        public virtual Task ExecuteAsync(object? parameter)
        {
            throw new NotImplementedException();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #region Explicit implementations
        bool ICommand.CanExecute(object? parameter)
        {
            return CanExecute(parameter);
        }

        void ICommand.Execute(object? parameter)
        {
            ExecuteAsync(parameter);
        }
        #endregion
    }

    public class RelayCommand : ICommand
    {
        private Func<object?, bool> _canExecute;
        private Action<object?> _execute;

        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action<object?> execute, Func<object?, bool> canExecute)
        {
            _canExecute = canExecute;
            _execute = execute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            _execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
