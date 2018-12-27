using System;
using System.Collections.Generic;
using System.Text;

namespace Hass.Client.Models.Components
{

    public enum AlarmControlPanelStateEnum
    {
        Disarmed,
        ArmedHome,
        ArmedAway,
        PendingArmedAway,
        PendingTriggered,
        Triggered,
    }

}
