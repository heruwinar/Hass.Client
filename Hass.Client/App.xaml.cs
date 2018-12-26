using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Hass.Client.Views;


[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Hass.Client
{
    public partial class App : Application
    {
        CancellationToken cts = new CancellationToken();

        HassApi.WsAPI wsClient;

        HassApi.StateResult[] states;

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
            ConnectToServerAsync();
        }

        private async void ConnectToServerAsync()
        {
            wsClient = new HassApi.WsAPI(new Uri("ws://lockng.duckdns.org/api/websocket"), "l159456753");

            await wsClient.ConnectAsync();

            states = await wsClient.ListStatesAsync();

            wsClient.StateChanged += OnWsClientStateChanged;
        }

        private void OnWsClientStateChanged(object sender, HassApi.StateChangedEventArgs e)
        {

        }

        //private void OnWsClientStateChanged(object sender, HassApi.StateChangedEventArgs e)
        //{
        //    HassApi.StateResult state = e.Event.Data.NewState;
        //    if (e.Event.Data.EntityId == "alarm_control_panel.home_alarm")
        //    {
        //        if (state.State == "pending")
        //        {
        //            string postState = state.Attributes["post_pending_state"].GetString();
        //            if (postState == "armed_away")
        //            {
        //                PlaySSML("Alarm is being armed for away! You might exit now");
        //            }
        //            else if (postState == "triggered")
        //            {
        //                PlaySSML("Please enter code to disarm or siren will sound<break time='3s' />", 10);
        //            }
        //        }
        //        else if (state.State == "armed_home")
        //        {
        //            PlaySSML("Alarm has been armed for staying!");
        //        }
        //        if (state.State == "armed_away")
        //        {
        //            PlaySSML("Alarm has been armed for away!");
        //        }
        //        else if (state.State == "disarmed")
        //        {
        //            PlaySSML("Alarm has been disarmed");
        //        }
        //        else if (state.State == "triggered")
        //        {
        //            PlaySSML("Alarm is triggered. Please enter code to disarm <break time='3s' />", 10);
        //        }
        //        else
        //        {
        //            PlaySSML(null, 0);
        //        }
        //    }
        //}

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
