using System;
using System.Collections.Generic;
using System.Text;
using Hass.Client.HassApi;

namespace Hass.Client.Models.Components
{

    public class UnkownEntity : Entity<string>
    {
        public UnkownEntity(string entityId)
            : base(entityId, PlatformTypeEnum.Unknown)
        {

        }

        protected override void InitializeValues(StateResult state)
        {
            base.InitializeValues(state);
            State = state.State;
        }

    }

}
