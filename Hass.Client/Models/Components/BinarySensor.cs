using System;
using System.Collections.Generic;
using System.Text;
using Hass.Client.HassApi;
using Hass.Client.Models;

namespace Hass.Client.Models.Components
{
    public class BinarySensor : Entity<bool>
    {
        private string deviceClass;

        public BinarySensor(string id)
            : base(id, PlatformTypeEnum.Sensor)
        {
        }

        public string DeviceClass
        {
            get
            {
                return deviceClass;
            }
            set
            {
                SetProperty(ref deviceClass, value);
            }
        }

        protected override void InitializeValues(StateResult state)
        {
            base.InitializeValues(state);

            string v = state.State;

            State = StringComparer.InvariantCultureIgnoreCase.Equals(v, "on")
                || StringComparer.InvariantCultureIgnoreCase.Equals(v, "true");

            DeviceClass = state.Attributes.GetValue<string>("unit_of_measurement");
        }

    }

}
