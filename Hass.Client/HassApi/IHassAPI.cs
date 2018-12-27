using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hass.Client.HassApi
{
    public interface IHassAPI
    {
        event EventHandler Authenticated;

        event EventHandler<ResultReceivedEventArgs> ResultReceived;

        event EventHandler<StateChangedEventArgs> StateChanged;

        Task<bool> ConnectAsync();

        Task<StateResult[]> ListStatesAsync();
    }

}
