using System;
using System.Collections.Generic;
using System.Linq;
using Hass.Client.HassApi;

namespace Hass.Client.Models
{
    public abstract class Entity<TState> : ModelBase, IComponent
    {
        private string contextId;
        private TState state;
        private string friendlyName;
        private DateTime lastChanged;
        private DateTime lastUpdated;

        protected Entity(string id, PlatformTypeEnum platform, TState state = default(TState))
        {
            Id = id;
            Platform = platform;
            State = state;
        }

        public string Id { get; private set; }


        public string ContextId
        {
            get
            {
                return contextId;
            }
            protected set
            {
                SetProperty(ref contextId, value);
            }
        }

        
        public PlatformTypeEnum Platform
        {
            get;
            private set;
        }

        public TState State
        {
            get
            {
                return state;
            }
            protected set
            {
                SetProperty(ref state, value);
            }
        }

        string IComponent.EntityId
        {
            get
            {
                return Id;
            }
        }

        object IComponent.State
        {
            get
            {
                return State;
            }
        }

        public string FriendlyName
        {
            get
            {
                return friendlyName;
            }
            set
            {
                SetProperty(ref friendlyName, value);
            }
        }

        public DateTime LastChanged
        {
            get
            {
                return lastChanged;
            }
            set
            {
                SetProperty(ref lastChanged, value);
            }
        }

        public DateTime LastUpdated
        {
            get
            {
                return lastUpdated;
            }
            set
            {
                SetProperty(ref lastUpdated, value);
            }
        }

        void IComponent.Initialize(StateResult state)
        {
            InitializeValues(state);
        }

        protected virtual void InitializeValues(StateResult state)
        {
            ContextId = state.Context.GetValue<string>("id");
            FriendlyName = state.Attributes.GetValue<string>("friendly_name");
            LastChanged = state.LastChanged;
            LastUpdated = state.LastUpdated;
        }

    }

}
