using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UntitledGame.EntityManagement
{
    abstract class SystemBase
    {
        public EntityManager EntityManager { get; private set; }

        protected Type[] _componentFilter;
        private List<Entity> _entities;

        protected SystemBase(params Type[] componentFilter)
        {
            _componentFilter = componentFilter;
            _entities = new List<Entity>();
        }

        public void Bind(EntityManager manager)
        {
            if (EntityManager != null)
            {
                throw new InvalidOperationException("System already bound to an EntityManager.");
            }
            EntityManager = manager;
            OnManagerBound();

            EntityManager.MessageDispatcher.Subscribe(EntityManager.MESSAGE_ENTITY_ALTERED, OnEntityAltered);
            EntityManager.MessageDispatcher.Subscribe(EntityManager.MESSAGE_ENTITY_ADDED, OnEntityAdded);
            EntityManager.MessageDispatcher.Subscribe(EntityManager.MESSAGE_ENTITY_REMOVED, OnEntityRemoved);
        }

        public void Unbind()
        {
            if (EntityManager == null)
            {
                throw new InvalidOperationException("System not bound to an EntityManager.");
            }

            EntityManager.MessageDispatcher.Unsubscribe(EntityManager.MESSAGE_ENTITY_ALTERED, OnEntityAltered);
            EntityManager.MessageDispatcher.Unsubscribe(EntityManager.MESSAGE_ENTITY_ADDED, OnEntityAdded);
            EntityManager.MessageDispatcher.Unsubscribe(EntityManager.MESSAGE_ENTITY_REMOVED, OnEntityRemoved);
            _entities.Clear();
            EntityManager = null;

            OnManagerUnbound();

        }

        protected virtual void OnManagerBound()
        {
        }

        protected virtual void OnManagerUnbound()
        {
        }


        private void OnEntityAdded(Message message)
        {
            CheckEntity(message.DataAs<Entity>());
        }

        private void OnEntityAltered(Message message)
        {
            CheckEntity(message.DataAs<Entity>());
        }

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
            bool wasValid = _entities.Contains(entity);
            bool currentValidity = IsValid(entity);

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
            return _componentFilter.Any(t => !entity.ContainsComponent(t));
        }
    }
}
