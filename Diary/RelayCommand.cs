using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Diary
{
    public  class RelayCommand : ICommand
    {
        #region Fields

        readonly Action<object> execute;
        readonly Func<object, bool> canExecute;

        #endregion // Fields

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }


        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
