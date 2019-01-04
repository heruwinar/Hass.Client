using System;
using System.Collections.Generic;
using System.Text;
using Hass.Client.HassApi;

namespace Hass.Client.Models.Components
{

    public class AlarmPanelCodeManager: Entity<string>
    {
        public AlarmPanelCodeManager(string id)
            : base(id, PlatformTypeEnum.AlarmPanelCodeManager)
        {
        }

        protected override void InitializeValues(StateResult state)
        {
            base.InitializeValues(state);
        }

    }

}
