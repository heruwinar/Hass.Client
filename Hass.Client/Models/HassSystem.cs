using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hass.Client.HassApi;
using Hass.Client.Models.Components;

namespace Hass.Client.Models
{

    public class HassSystem : ModelBase
    {

        private EntityKeyedCollection allEntities;

        public HassSystem(IHassAPI hassAPI = null)
        {
            SetHassAPI(hassAPI);
        }

        private void OnHassApiStateChanged(object sender, StateChangedEventArgs e)
        {
            string entityId = e.Event.Data.EntityId;

            IComponent component = allEntities.FindByIdOrDefault(entityId);
            if (component != null)
            {
                component.Initialize(e.Event.Data.NewState);
            }
            else
            {
                component = BuildEntitiesFromStateResults(new[] { e.Event.Data.NewState }).First();
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

            OnPropertyChanged(nameof(AllEntities));
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
                    var entity = (IComponent)Activator.CreateInstance(tp, new object[] { st.EntityId });

                    entity.Initialize(st);

                    yield return entity;
                }
            }
        }

    }

}
