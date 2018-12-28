using System;
using System.Collections.Generic;
using System.Text;
using Hass.Client.HassApi;
using Hass.Client.Models;

namespace Hass.Client.Models.Components
{
    public class Sensor : Entity<string>
    {

        private string unitOfMeasurement;
        private string deviceClass;

        public Sensor(string id)
            : base(id, PlatformTypeEnum.Sensor)
        {
        }

        public string UnitOfMeasurement
        {
            get
            {
                return unitOfMeasurement;
            }
            set
            {
                SetProperty(ref unitOfMeasurement, value);
            }
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

            UnitOfMeasurement = state.Attributes.GetValue<string>("device_class");
            DeviceClass = state.Attributes.GetValue<string>("unit_of_measurement");
        }

    }

}
