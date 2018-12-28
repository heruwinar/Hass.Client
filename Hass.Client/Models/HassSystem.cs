using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hass.Client.HassApi;
using Hass.Client.Models.Components;
using Xamarin.Forms;

namespace Hass.Client.Models
{

    public class HassSystem : ModelBase
    {

        private EntityKeyedCollection allEntities = new EntityKeyedCollection();


        public HassSystem()
        {
            SetHassAPI(WsAPI.Instance);
        }

        private void OnHassApiStateChanged(object sender, StateChangedEventArgs e)
        {
            IComponent component = BuildEntitiesFromStateResults(new[] { e.Event.Data.NewState }).First();

            if(!allEntities.Contains(component.EntityId))
            {
                allEntities.Add(component);
            }
        }

        public IHassAPI HassApi
        {
            get;
            private set;
        }

        public IEnumerable<IComponent> AllEntities
        {
            get
            {
                return allEntities;
            }
        }

        private void SetHassAPI(IHassAPI newValue)
        {
            Action<IHassAPI, bool> installEvtHandlers = (h, install) =>
            {
                if(h == null)
                {
                    return;
                }
                if(install)
                {
                    h.StateChanged += OnHassApiStateChanged;
                }
                else
                {
                    h.StateChanged -= OnHassApiStateChanged;
                }
            };

            if(newValue != HassApi)
            {
                installEvtHandlers(HassApi, false);
                HassApi = newValue;
                installEvtHandlers(HassApi, true);
            }
        }

        public async Task Initialize()
        {
            if(HassApi == null)
            {
                SetHassAPI(new WsAPI(new Uri("ws://lockng.duckdns.org/api/websocket"), "l159456753"));
            }

            await HassApi.ConnectAsync();

            StateResult[] states = await HassApi.ListStatesAsync();

            allEntities = new EntityKeyedCollection(BuildEntitiesFromStateResults(states));

            Device.BeginInvokeOnMainThread(() => OnPropertyChanged(nameof(AllEntities)));
        }

        private IEnumerable<IComponent> BuildEntitiesFromStateResults(StateResult[] states)
        {
            IEnumerable<IGrouping<string, StateResult>> grps =
                states.GroupBy(st => st.EntityId.Substring(0, st.EntityId.IndexOf(".")));

            foreach (IGrouping<string, StateResult> grp in grps.OrderBy(g => g.Key))
            {
                PlatformTypeEnum platform;
                Enum.TryParse(grp.Key.Replace("_", ""), true, out platform);

                string entityType = $"{typeof(HassSystem).Namespace}.Components.{platform}";

                Type tp = typeof(HassSystem).Assembly.GetType(entityType, false, true) ?? typeof(UnkownEntity);

                foreach (StateResult st in grp)
                {
                    IComponent component = allEntities.FindByIdOrDefault(st.EntityId);
                    if (component == null)
                    {
                        component = (IComponent)Activator.CreateInstance(tp, new object[] { st.EntityId });
                    }

                    component.Initialize(st);

                    yield return component;
                }
            }
        }

    }

}
