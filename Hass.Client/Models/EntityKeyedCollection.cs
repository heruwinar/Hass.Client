using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Hass.Client.Models
{
    public class EntityKeyedCollection 
        : KeyedCollection<string, IComponent>, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public EntityKeyedCollection(IEnumerable<IComponent> items = null)
        {
            if(items != null)
            {
                foreach(IComponent item in items)
                {
                    Add(item);
                }
            }
        }

        public IComponent FindByIdOrDefault(string entityid)
        {
            if(Dictionary == null)
            {
                return null;
            }
            IComponent res;
            Dictionary.TryGetValue(entityid, out res);
            return res;
        }

        protected override string GetKeyForItem(IComponent item)
        {
            return item.EntityId;
        }

        protected override void RemoveItem(int index)
        {
            IComponent item = this[index];
            base.RemoveItem(index);
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, item, index);
        }

        protected override void InsertItem(int index, IComponent item)
        {
            base.InsertItem(index, item);
            OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
        }

        protected override void SetItem(int index, IComponent item)
        {
            base.SetItem(index, item);
            OnCollectionChanged(NotifyCollectionChangedAction.Replace, item, index);
        }

        protected override void ClearItems()
        {
            base.ClearItems();
            OnCollectionChanged(NotifyCollectionChangedAction.Reset);
        }

        private void OnCollectionChanged(
            NotifyCollectionChangedAction action, 
            IComponent item = null, 
            int index = -1)
        {
            if(CollectionChanged != null)
            {
                if (item == null)
                {
                    CollectionChanged(this, new NotifyCollectionChangedEventArgs(action));
                }
                else
                {
                    CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, item, index));
                }
            }
        }

    }
}
