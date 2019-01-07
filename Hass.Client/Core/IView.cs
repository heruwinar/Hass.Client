using System;
using System.Collections.Generic;
using System.Text;

namespace Hass.Client.Core
{
    public interface IView
    {
        IViewModel ViewModel { get; set; }
    }

    public interface IView<TViewModel>: IView
        where TViewModel: IViewModel
    {
    }

}
