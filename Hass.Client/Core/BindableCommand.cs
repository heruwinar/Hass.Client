using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Hass.Client.Core
{
    public class BindableCommand<T> : ModelBase, ICommand
    {
        public event EventHandler CanExecuteChanged;

        private bool isExecutable = true;
        private bool isApplicable = true;
        private string text = "Command";
        private Action<T> action;

        public BindableCommand(Action<T> action)
        {
            this.action = action;
        }

        public bool IsExecutable
        {
            get
            {
                return isExecutable;
            }
            set
            {
                if (SetProperty(ref isExecutable, value))
                {
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public bool IsApplicable
        {
            get
            {
                return isApplicable;
            }
            set
            {
                SetProperty(ref isApplicable, value);
            }
        }

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                SetProperty(ref text, value);
            }
        }

        public virtual bool CanExecute(object parameter)
        {
            return IsExecutable && parameter is T;
        }

        public void Execute(T parameter)
        {
            action(parameter);
        }

        void ICommand.Execute(object parameter)
        {
            Execute((T)parameter);
        }
    }

    public class BindableCommand : BindableCommand<object>
    {
        public BindableCommand(Action action)
            :base(p => action())
        {
        }

        public override bool CanExecute(object parameter)
        {
            return IsExecutable;
        }

    }
}
