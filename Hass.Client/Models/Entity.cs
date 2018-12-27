using System;
using System.Collections.Generic;
using System.Linq;
using Hass.Client.HassApi;

namespace Hass.Client.Models
{
    public abstract class Entity<TState> : ModelBase, IComponent
    {
        private TState state;
        protected Entity(string id, PlatformTypeEnum platform, TState state = default(TState))
        {
            Id = id;
            Platform = platform;
            State = state;
        }

        public string Id { get; private set; }

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

        void IComponent.Initialize(StateResult state)
        {
            InitializeValues(state);
        }

        protected virtual void InitializeValues(StateResult state)
        {

        }
    }

}
