using System;
using System.Windows.Input;

namespace GoogleParsing.Commands
{
    public abstract class CommandBase : ICommand
    {
        public abstract bool CanExecute(object parameter);
        public abstract void Execute(object parameter);
        public event EventHandler CanExecuteChanged;
    }
}
