using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Reflection;

namespace Hass.Client.Views.Controls
{

    [ContentProperty("Templates")]
    public class ByTypeDataTemplateSelector : IDataTemplateSelector
    {

        [DefaultMember("Item")]
        public class TemplateEntryList : KeyedCollection<Type, TemplateEntry>
        {
            protected override Type GetKeyForItem(TemplateEntry item)
            {
                if(item == null)
                {
                    throw new ArgumentNullException("item");
                }
                if(item.Type == null)
                {
                    throw new ArgumentNullException("item.Type");
                }
                return item.Type;
            }

            public DataTemplate FindTemplate(Type key)
            {
                TemplateEntry entry;
                Dictionary.TryGetValue(key, out entry);
                return entry?.DataTemplate;
            }

        }

        public ByTypeDataTemplateSelector()
        {
            Templates = new TemplateEntryList();
        }

        public TemplateEntryList Templates { get; private set; }

        public DataTemplate DefaultTemplate { get; set; }

        public DataTemplate SelectTemplate(object item, BindableObject container)
        {
            return Templates.FindTemplate(item.GetType()) ?? DefaultTemplate;
        }

    }


    [ContentProperty("DataTemplate")]
    public class TemplateEntry
    {
        public Type Type { get; set; }

        public DataTemplate DataTemplate { get; set; }
    }

}
