using System;
using System.Collections.Generic;
using System.Text;

namespace UntitledGame.EntityManagement
{
    class EntityManager
    {
        public const string MESSAGE_ENTITY_ALTERED = nameof(EntityManager) + ".EntityAltered";
        public const string MESSAGE_ENTITY_ADDED = nameof(EntityManager) + ".EntityAdded";
        public const string MESSAGE_ENTITY_REMOVED = nameof(EntityManager) + ".EntityRemoved";
        public MessageDispatcher MessageDispatcher { get; private set; }

        private List<Entity> _entities;
        private List<SystemBase> _systems;


        public EntityManager()
        {
            MessageDispatcher = new MessageDispatcher();
            _entities = new List<Entity>();
        }

        public void AddEntity(Entity entity)
        {
            entity.Bind(this);
            _entities.Add(entity);
            MessageDispatcher.Invoke(MESSAGE_ENTITY_ADDED, entity);
        }

        public void RemoveEntity(Entity entity)
        {
            entity.Unbind();
            _entities.Remove(entity);
            MessageDispatcher.Invoke(MESSAGE_ENTITY_REMOVED, entity);
        }

        public IEnumerable<Entity> GetEntities()
        {
            return _entities;
        }


        public void AddSystem(SystemBase system)
        {
            system.Bind(this);
            _systems.Add(system);
        }

        public void RemoveSystem(SystemBase system)
        {
            system.Unbind();
            _systems.Remove(system);
        }

        public IEnumerable<SystemBase> GetSystems()
        {
            return _systems;
        }
    }
}
