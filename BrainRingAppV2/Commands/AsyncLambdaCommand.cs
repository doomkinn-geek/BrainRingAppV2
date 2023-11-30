using System;
using System.Threading.Tasks;

namespace BrainRingAppV2.Commands
{
    internal class AsyncLambdaCommand : BaseCommand
    {
        private readonly Func<object, bool> _canExecute;
        private readonly Func<object, Task> _execute;

        public AsyncLambdaCommand(Func<object, bool> canExecute, Func<object, Task> execute)
        {
            _canExecute = canExecute;
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public override bool CanExecute(object parameter = null)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }

        public override async void Execute(object parameter = null)
        {
            await _execute(parameter);
        }
    }
}
