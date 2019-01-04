using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;
using Hass.Client.Models.Components;

namespace Hass.Client.Views.Converters
{

    public class AlarmControlPanelStateTextConverter : BaseConverter<AlarmControlPanelStateEnum>
    {

        public static AlarmControlPanelStateTextConverter Default = new AlarmControlPanelStateTextConverter();

        protected override object Convert(AlarmControlPanelStateEnum value,  object parameter)
        {
            switch(value)
            {
                case AlarmControlPanelStateEnum.ArmedAway:
                case AlarmControlPanelStateEnum.ArmedHome:
                    return "ARMED";
                case AlarmControlPanelStateEnum.PendingTriggered:
                case AlarmControlPanelStateEnum.PendingArmedAway:
                    return "PENDING";
                case AlarmControlPanelStateEnum.Triggered:
                    return "TRIGGERED";
                default:
                    return "DISARMED";
            }
        }

    }

}
