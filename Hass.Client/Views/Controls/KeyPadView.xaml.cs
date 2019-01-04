using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Hass.Client.Util;

namespace Hass.Client.Views.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class KeyPadView : ContentView
    {

        public BindableProperty CodeProperty = BindableProperty.Create(
            "Code",
            typeof(string),
            typeof(KeyPadView),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (s, o, n) => ((KeyPadView)s).OnCodeChanged());

        public BindableProperty CodeLengthProperty = BindableProperty.Create(
            "CodeLength",
            typeof(int),
            typeof(KeyPadView),
            6);

        public KeyPadView()
        {
            InitializeComponent();
        }

        public string Code
        {
            get
            {
                return (string)GetValue(CodeProperty);
            }
            set
            {
                SetValue(CodeProperty, value);
            }
        }

        public int CodeLength
        {
            get
            {
                return (int)GetValue(CodeLengthProperty);
            }
            set
            {
                SetValue(CodeLengthProperty, value);
            }
        }


        private void OnButtonKeyClicked(object sender, EventArgs e)
        {

            char key = ((Button)sender).Text.First();
            if(char.IsDigit(key))
            {
                SetCode(Code + key);
            }
            else if(key == 'C')
            {
                SetCode("");
            }

        }

        private void SetCode(string newCode)
        {
            string code = Code ?? "";
            newCode = newCode ?? "";

            if(newCode == code)
            {
                return;
            }

            Action setCode = () =>
            {
                Code = newCode.Length > CodeLength 
                ? newCode.Substring(newCode.Length - CodeLength)
                : newCode;
            };

            string animName = "lblCodeAnimation";

            lblCodeLabel.AbortAnimation(animName);

            if (newCode.Length == 0)
            {
                setCode();
                lblCodeLabel.AnimateFontSize(animName, 20, () => { });
            }
            else if (newCode.Length == 1)
            {
                lblCodeLabel.AnimateFontSize(animName, 12, setCode);
            }
            else
            {
                setCode();
            }
        }

        private void OnCodeChanged()
        {
            lblCodeValue.Text = new string('*', (Code?.Length).GetValueOrDefault());
        }

    }
}