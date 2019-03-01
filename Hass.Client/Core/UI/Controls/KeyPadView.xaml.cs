using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Hass.Client.Util;
using Hass.Client.Core;

namespace Hass.Client.Views.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class KeyPadView : ContentView
    {

        public static BindableProperty HeaderTextProperty = BindableProperty.Create(
            "HeaderText",
            typeof(string),
            typeof(KeyPadView),
            "Code");

        public static BindableProperty CodeProperty = BindableProperty.Create(
            "Code",
            typeof(string),
            typeof(KeyPadView),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (s, o, n) => ((KeyPadView)s).OnCodeChanged());

        public static BindableProperty CodeLengthProperty = BindableProperty.Create(
            "CodeLength",
            typeof(int),
            typeof(KeyPadView),
            6);

        public static BindableProperty Command1Property = BindableProperty.Create(
            "Command1",
            typeof(BindableCommand),
            typeof(KeyPadView));

        public static BindableProperty Command2Property = BindableProperty.Create(
            "Command2",
            typeof(BindableCommand),
            typeof(KeyPadView));


        private double lblCodeLabelFontSize;

        public KeyPadView()
        {
            InitializeComponent();
            OnCodeChanged();
            lblCodeLabelFontSize = lblCodeLabel.FontSize;
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

        public string HeaderText
        {
            get
            {
                return (string)GetValue(HeaderTextProperty);
            }
            set
            {
                SetValue(HeaderTextProperty, value);
            }
        }

        public BindableCommand Command1
        {
            get
            {
                return (BindableCommand)GetValue(Command1Property);
            }
            set
            {
                SetValue(Command1Property, value);
            }
        }

        public BindableCommand Command2
        {
            get
            {
                return (BindableCommand)GetValue(Command2Property);
            }
            set
            {
                SetValue(Command2Property, value);
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
                lblCodeLabel.AnimateFontSize(animName, lblCodeLabelFontSize, () => { });
            }
            else if (newCode.Length == 1)
            {
                lblCodeLabel.AnimateFontSize(animName, lblCodeLabelFontSize - 8, setCode);
            }
            else
            {
                setCode();
            }
        }

        private void OnCodeChanged()
        {
            lblCodeValue.Text = " " + new string('*', (Code?.Length).GetValueOrDefault());
        }


    }
}