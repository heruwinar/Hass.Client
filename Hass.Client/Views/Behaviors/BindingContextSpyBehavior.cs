using System;
using System.Linq;
using Xamarin.Forms;
using Hass.Client.Views.Common;
using Hass.Client.ViewModels;

namespace Hass.Client.Views.Behaviors
{
    public class BindingContextSpyBehavior: Behavior<Page>
    {

        private void OnPageBindingContextChanged(object sender, EventArgs e)
        {
            var pg = (Page)sender;

            foreach (BindingContextSpy ctxSpy in pg.Resources
                            .Select(kvp => kvp.Value)
                            .Where(r => r is BindingContextSpy))
            {
                ctxSpy.BindingContext = pg.BindingContext;
            }
        }

        protected override void OnAttachedTo(Page page)
        {
            base.OnAttachedTo(page);
            page.BindingContextChanged += OnPageBindingContextChanged;
            page.Appearing += OnPageAppearing;
            page.Disappearing += OnPageDisappearing;

            OnPageBindingContextChanged(page, EventArgs.Empty);
        }

        private void OnPageDisappearing(object sender, EventArgs e)
        {
            //App.Current.Navigation = null;
        }

        private void OnPageAppearing(object sender, EventArgs e)
        {
            App.Current.Navigation = ((Page)sender).Navigation;
        }

        protected override void OnDetachingFrom(Page page)
        {
            base.OnDetachingFrom(page);
            page.BindingContextChanged -= OnPageBindingContextChanged;
            page.Appearing -= OnPageAppearing;
            page.Disappearing -= OnPageDisappearing;
        }

    }
}
