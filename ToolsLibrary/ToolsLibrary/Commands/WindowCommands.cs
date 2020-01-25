using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FrameLessWindow.Commands
{
    public class WindowCommands : ICommand
    {
        public Action<object> Actor { get; set; }
        public Func<object,bool> CheckFunc { get; set; }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            if (CheckFunc == null) return true;
            return CheckFunc.Invoke(parameter);
            
        }

        public void Execute(object parameter)
        {
            if(Actor!=null && parameter !=null)
                Actor.Invoke(parameter);
        }
    }
}
