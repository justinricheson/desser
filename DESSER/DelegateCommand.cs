using System;
using System.Windows.Input;

namespace DESSER
{
    public class DelegateCommand : ICommand
    {
        #region Private Variables
        private Action _action;
        #endregion

        #region Constructor
        public DelegateCommand(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            _action = action;
        }
        #endregion

        #region Public
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            _action();
        }
        #endregion
    }
}
