using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Hass.Client.Views;
using Hass.Client.Core;


[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Hass.Client
{
    public partial class App : Application, IShellContext, IViewModelNavigation
    {
        private static Dictionary<Type, Type> viewTypes = new Dictionary<Type, Type>();

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }
        
        static App()
        {
            Type iViewTp = typeof(IView);
            Type viewTp = typeof(View);
            Type pgTp = typeof(Page);
            Type[] ctorParams = new Type[] { };

            foreach(Type tp in typeof(App).Assembly
                .GetTypes()
                .Where(t => iViewTp.IsAssignableFrom(t))
                .Where(t => viewTp.IsAssignableFrom(t) || pgTp.IsAssignableFrom(t))
                .Where(t => t.GetConstructor(ctorParams) != null))
            {
                Type iMvvm = tp.GetInterface("IView`1");
                if(iMvvm != null)
                {
                    Type vmTp = iMvvm.GetGenericArguments()[0];
                    viewTypes[vmTp] = tp;
                }
            }
        }

        public new static App Current
        {
            get
            {
                return (App)Application.Current;
            }
        }

        public IViewModelNavigation Navigation
        {
            get
            {
                return this;
            }
        }
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

        void IShellContext.BeginInvokeOnMainThread(Action action)
        {
            Device.BeginInvokeOnMainThread(action);
        }

        async Task IViewModelNavigation.PushAsync(IViewModel viewModel)
        {
            await MainPage.Navigation.PushAsync(ResolvePage(FindViewForViewModel(viewModel)));
        }

        async Task IViewModelNavigation.PushModalAsync(IModalViewModel viewModel)
        {
            await MainPage.Navigation.PushModalAsync(ResolvePage(FindViewForViewModel(viewModel)));
        }

        public static IView FindViewForViewModel(IViewModel viewModel)
        {
            Type vmType = viewModel.GetType();
            Type viewTp;
            if(viewTypes.TryGetValue(vmType, out viewTp))
            {
                var view = (IView)Activator.CreateInstance(viewTp);

                view.ViewModel = viewModel;

                return view;
            }
            throw new ArgumentException($"cannot find view for view model: {vmType}");
        }

        public static Page ResolvePage(IView view)
        {
            Page pg = view as Page;
            if(pg == null)
            {
                pg = new ContentPage
                {
                    Content = (View)view,
                    BindingContext = view.ViewModel
                };
                pg.SetBinding(Page.TitleProperty, new Binding("Title"));
            }
            return pg;
        }
    }
}
