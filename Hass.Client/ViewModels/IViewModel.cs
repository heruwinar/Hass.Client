using System;
using System.Collections.Generic;
using System.Text;

namespace Hass.Client.ViewModels
{
    public interface IViewModel
    {
        string Title { get; }

        bool IsBusy { get; }

        IShellContext ShellContext { get; }

    }

    public interface IModalViewModel : IViewModel
    {
    }

}
