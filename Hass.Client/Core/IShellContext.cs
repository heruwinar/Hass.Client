using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hass.Client.Core
{
    public interface IShellContext
    {
        void BeginInvokeOnMainThread(Action action);

        IViewModelNavigation Navigation { get; }

    }


}
