using System;
using System.Collections.Generic;
using System.Text;

namespace Hass.Client.Core
{

    public interface IAppModule
    {
        string Title { get; set; }

        IEnumerable<IAppModuleSection> Items { get; }

        IAppModuleSection SelectedItem { get; set; }
    }

    public interface IAppModuleSection
    {
        string Title { get; set; }

        IEnumerable<IAppModuleItem> Items { get; }

        IAppModuleItem SelectedItem { get; set; }
    }

    public interface IAppModuleItem
    {
        string Title { get; set; }

    }

}
