using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UntitledGame.EntityManagement
{
    abstract class SystemBase
    {
        public EntityManager EntityManager { get; private set; }

        // An array of components that an entity must have for this system to care about it.
        protected Type[] _componentFilter;

        private List<Entity> _entities;

        protected SystemBase(params Type[] componentFilter)
        {
            _componentFilter = componentFilter;
            _entities = new List<Entity>();
        }

        public void Bind(EntityManager manager)
        {
            // If this system already has an owner, the user is doing something stupid.
            if (EntityManager != null)
            {
                throw new InvalidOperationException("System already bound to an EntityManager.");
            }

            EntityManager = manager;

            // Allow sub-classes to do any kind of init work they made need to do.
            OnManagerBound();

            // Subscribe to a few events the manager will throw at us during its lifetime.
            EntityManager.MessageDispatcher.Subscribe(EntityManager.MESSAGE_ENTITY_ALTERED, OnEntityAltered);
            EntityManager.MessageDispatcher.Subscribe(EntityManager.MESSAGE_ENTITY_ADDED, OnEntityAdded);
            EntityManager.MessageDispatcher.Subscribe(EntityManager.MESSAGE_ENTITY_REMOVED, OnEntityRemoved);
        }

        public void Unbind()
        {
            // If the user hasn't bound this entity yet, we've probably hit an unexpected state.
            if (EntityManager == null)
            {
                throw new InvalidOperationException("System not bound to an EntityManager.");
            }

            // Unsubscribe from the events we were previously listening for.
            EntityManager.MessageDispatcher.Unsubscribe(EntityManager.MESSAGE_ENTITY_ALTERED, OnEntityAltered);
            EntityManager.MessageDispatcher.Unsubscribe(EntityManager.MESSAGE_ENTITY_ADDED, OnEntityAdded);
            EntityManager.MessageDispatcher.Unsubscribe(EntityManager.MESSAGE_ENTITY_REMOVED, OnEntityRemoved);

            // Allow sub-classes to clean up their state.
            OnManagerUnbound();

            _entities.Clear();
            EntityManager = null;
        }


        protected virtual void OnManagerBound()
        {
        }

        protected virtual void OnManagerUnbound()
        {
        }


        // This is called from the MessageDispatcher when the owning EntityManager gets a new entity.
        private void OnEntityAdded(Message message)
        {
            // Check to see if the entity's composition is in line with our component filter.
            CheckEntity(message.DataAs<Entity>());
        }

        // This is called from the MessageDispatcher when an entity's composition is altered.
        private void OnEntityAltered(Message message)
        {
            // Check to see if the entity's composition is now in line with our component filter.
            CheckEntity(message.DataAs<Entity>());
        }


        // This is called from the MessageDispatcher when an entity is removed from our owning EntityManager.
        private void OnEntityRemoved(Message message)
        {
            var entity = message.DataAs<Entity>();
            if(_entities.Contains(entity))
            {
                _entities.Remove(entity);
            }
        }

        private void CheckEntity(Entity entity)
        {
            // Perform the logic to add/remove the entity based upon a change in its composition.
            bool wasValid = _entities.Contains(entity);
            bool currentValidity = IsValid(entity);

            // If we have already added the entity, and it's no longer something this system cares about, remove it.
            // Otherwise, if the entity hasn't yet been added but its composition is now interesting to us, add it.
            if(wasValid && !currentValidity)
            {
                _entities.Remove(entity);
            }
            else if(!wasValid && currentValidity)
            {
                _entities.Add(entity);
            }
        }

        private bool IsValid(Entity entity)
        {
            // Check the entity's composition against our component filter.
            return _componentFilter.Any(t => !entity.ContainsComponent(t));
        }
    }
}
