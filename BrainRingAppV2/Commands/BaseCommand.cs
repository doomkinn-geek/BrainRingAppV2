using System;
using System.Windows.Input;

namespace BrainRingAppV2.Commands
{
    internal abstract class BaseCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public abstract bool CanExecute(object parameter = null);

        public abstract void Execute(object parameter = null);
    }
}
