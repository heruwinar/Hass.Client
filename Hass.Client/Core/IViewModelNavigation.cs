using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hass.Client.Core
{
    public interface IViewModelNavigation
    {
        Task PushAsync(IViewModel viewModel);

        Task PushModalAsync(IModalViewModel viewModel);

    }

}
