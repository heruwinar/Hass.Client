using System;
using System.Collections.Generic;
using System.Text;
using Hass.Client.HassApi;
using Hass.Client.Models;

namespace Hass.Client.Models.Components
{
    public class AlarmControlPanel : Entity<AlarmControlPanelStateEnum>
    {

        public AlarmControlPanel(string id)
            : base(id, PlatformTypeEnum.AlarmControlPanel)
        {
        }

        protected override void InitializeValues(StateResult state)
        {
            base.InitializeValues(state);

            AlarmControlPanelStateEnum alarmState = AlarmControlPanelStateEnum.Disarmed;

            if (state.State == "armed_away")
            {
                alarmState = AlarmControlPanelStateEnum.ArmedAway;
            }
            else if (state.State == "armed_home")
            {
                alarmState = AlarmControlPanelStateEnum.ArmedHome;
            }
            else if (state.State == "triggered")
            {
                alarmState = AlarmControlPanelStateEnum.Triggered;
            }
            else if (state.State == "pending")
            {
                if ("triggered".Equals(state.Attributes.GetValue<string>("post_pending_state")))
                {
                    alarmState = AlarmControlPanelStateEnum.PendingTriggered;
                }
                else
                {
                    alarmState = AlarmControlPanelStateEnum.PendingArmedAway;
                }
            }

            State = alarmState;
        }

    }

}
