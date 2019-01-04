using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;
using Hass.Client.Models.Components;

namespace Hass.Client.Views.Converters
{
    public class AlarmControlPanelStateSvgConverter : BaseConverter<AlarmControlPanelStateEnum>
    {

        public static AlarmControlPanelStateSvgConverter Default = new AlarmControlPanelStateSvgConverter();

        protected override object Convert(AlarmControlPanelStateEnum value, object parameter)
        {
            switch (value)
            {
                case AlarmControlPanelStateEnum.ArmedAway:
                    return "alarm-armed-away";
                case AlarmControlPanelStateEnum.ArmedHome:
                    return "alarm-armed-home";
                case AlarmControlPanelStateEnum.PendingTriggered:
                    return "alarm-pending-triggered";
                case AlarmControlPanelStateEnum.PendingArmedAway:
                    return "alarm-pending-armed-away";
                case AlarmControlPanelStateEnum.Triggered:
                    return "alarm-triggered";
                default:
                    return "bell";
            }
        }

    }

}
