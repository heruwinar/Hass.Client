using System;
using System.Collections.Generic;
using System.Text;

namespace Hass.Client.Core
{
    public interface IViewModel
    {
        string Title { get; }

        bool IsBusy { get; }

        IShellContext ShellContext { get; }

        IViewModelNavigation Navigation { get; set; }

    }

    public interface IModalViewModel : IViewModel
    {
    }

}
