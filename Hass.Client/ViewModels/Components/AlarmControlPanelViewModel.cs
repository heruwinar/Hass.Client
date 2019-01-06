using System;
using System.Collections.Generic;
using System.Text;
using Hass.Client.Models.Components;
using Hass.Client.Common;

namespace Hass.Client.ViewModels.Components
{
    public class AlarmControlPanelViewModel: EntityViewModel<AlarmControlPanel>
    {

        private string code;

        public AlarmControlPanelViewModel(AlarmControlPanel entity)
            : base(entity)
        {
            ArmAwayCommand = new BindableCommand(OnArmAway) { Text = "ARM AWAY"};
            ArmHomeCommand = new BindableCommand(OnArmHome) { Text = "ARM HOME"};
        }

        public string Code
        {
            get
            {
                return code;
            }
            set
            {
                SetProperty(ref code, value);
            }
        }

        public BindableCommand ArmAwayCommand { get; private set; }

        public BindableCommand ArmHomeCommand { get; private set; }

        private void OnArmAway()
        {

        }

        private void OnArmHome()
        {

        }

    }

}
