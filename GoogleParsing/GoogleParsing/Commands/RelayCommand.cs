using System;
using System.Threading.Tasks;

namespace GoogleParsing.Commands
{
    public class RelayCommand : CommandBase
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;
        public RelayCommand() { }
        public RelayCommand(Func<Task> execute) : this(execute, null)
        {
            _execute = execute;
        }
        public RelayCommand(Func<Task> execute, Func<bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }
        public override bool CanExecute(object parameter)
            => _canExecute == null || _canExecute();

        public override void Execute(object parameter) => _execute();
    }
}
