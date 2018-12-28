using System;
using System.Collections.Generic;
using System.Text;
using Hass.Client.HassApi;

namespace Hass.Client.Models
{

    public interface IComponent
    {
        string EntityId { get; }

        string ContextId { get; }

        string FriendlyName { get; }

        PlatformTypeEnum Platform { get; }

        object State { get; }

        void Initialize(StateResult state);

    }

}
