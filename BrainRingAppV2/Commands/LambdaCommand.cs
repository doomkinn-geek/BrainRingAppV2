using System;

namespace BrainRingAppV2.Commands
{
    internal class LambdaCommand : BaseCommand
    {
        private readonly Func<object, bool> _canExecute;
        private readonly Action<object> _execute;

        public LambdaCommand(Func<object, bool> canExecute, Action<object> execute)
        {
            _canExecute = canExecute;
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public override bool CanExecute(object parameter = null)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }

        public override void Execute(object parameter = null)
        {
            _execute.Invoke(parameter);
        }
    }
}