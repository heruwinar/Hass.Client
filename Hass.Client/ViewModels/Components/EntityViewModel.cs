using System;
using System.Collections.Generic;
using System.Text;
using Hass.Client.Core;
using Hass.Client.Models;

namespace Hass.Client.ViewModels.Components
{
    public class EntityViewModel<TEntity>: ViewModelBase
        where TEntity: ModelBase, IComponent
    {

        public EntityViewModel(TEntity entity)
        {
            Entity = entity;
        }

        public TEntity Entity { get; private set; }
    }
}
