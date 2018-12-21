using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms.Platform.UWP;
using Windows.UI.Xaml.Media;

//[assembly: ExportRenderer(typeof(Xamarin.Forms.TabbedPage), typeof(Hass.Client.UWP.BottomTabbedPageRender))]

namespace Hass.Client.UWP
{
    public class BottomTabbedPageRender : TabbedPageRenderer
    {


        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            var pivot = (FormsPivot)Control;
            pivot.Style = (Style)Application.Current.Resources["FormsPivotLeftHeaderStyle"];
            //Control.Template = (ControlTemplate)Application.Current.Resources["LeftFormsPivotTemplate"];
            //Control.ApplyTemplate();
            //Control.SelectionChanged += Control_SelectionChanged;
            //SelectTab(Control.SelectedIndex);
        }

        
        private void Control_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectTab(Control.SelectedIndex);


        }

        private void SelectTab(int index)
        {
            var container = (Grid)Control.FindName("PivotItemsContainer");

            if (container == null)
            {
                return;
            }
            var selected = container.Children[Control.SelectedIndex];

            foreach (UIElement el in container.Children)
            {
                el.Visibility = el == selected ? Visibility.Visible : Visibility.Collapsed;
            }
            var navView = FindChild<PageControl>(selected, (t)=> true);
            if(navView != null)
            {
                
            }
            SystemNavigationManager nav = SystemNavigationManager.GetForCurrentView();
            nav.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        private static T FindChild<T>(DependencyObject startNode, Predicate<T> found) where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(startNode);
            for (int i = 0; i < count; i++)
            {
                DependencyObject current = VisualTreeHelper.GetChild(startNode, i);
                T asType = null;
                if ((current.GetType()).Equals(typeof(T)) || (current.GetType().GetTypeInfo().IsSubclassOf(typeof(T))))
                {
                    asType = (T)current;
                    if(found(asType))
                    {
                        return asType; 
                    }
                }
                asType = FindChild<T>(current, found);
                if(asType != null)
                {
                    return asType;
                }
            }
            return null;
        }
    }

}
