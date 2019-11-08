using System;
using System.Collections.Generic;
using System.Text;

namespace UntitledGame.EntityManagement
{
    class EntityManager
    {
        // Message identifiers for the MessageDispatcher.
        public const string MESSAGE_ENTITY_ALTERED = nameof(EntityManager) + ".EntityAltered";
        public const string MESSAGE_ENTITY_ADDED = nameof(EntityManager) + ".EntityAdded";
        public const string MESSAGE_ENTITY_REMOVED = nameof(EntityManager) + ".EntityRemoved";

        // The messagedispatcher that this entitymanager and systems can use to communicate between one-another.
        public MessageDispatcher MessageDispatcher { get; private set; }

        private List<Entity> _entities;
        private List<SystemBase> _systems;


        public EntityManager()
        {
            MessageDispatcher = new MessageDispatcher();
            // Note: We might should give this an initial capacity if performance becomes an issue.
            _entities = new List<Entity>();
        }

        public void AddEntity(Entity entity)
        {
            // Inform the entity that it now has an owner.
            entity.Bind(this);
            _entities.Add(entity);

            // Notify everybody listening in that we have a new entity.
            MessageDispatcher.Invoke(MESSAGE_ENTITY_ADDED, entity);
        }

        public void RemoveEntity(Entity entity)
        {
            // Inform the entity that it no longer has an owner.
            entity.Unbind();
            _entities.Remove(entity);

            // Notify everybody listening in that we are losing an entity.
            MessageDispatcher.Invoke(MESSAGE_ENTITY_REMOVED, entity);
        }

        public IEnumerable<Entity> GetEntities()
        {
            return _entities;
        }


        public void AddSystem(SystemBase system)
        {
            // Inform the system that it now has an owner.
            system.Bind(this);
            _systems.Add(system);
        }

        public void RemoveSystem(SystemBase system)
        {
            // Inform the system that it no longer has an owner.
            system.Unbind();
            _systems.Remove(system);
        }

        public IEnumerable<SystemBase> GetSystems()
        {
            return _systems;
        }
    }
}
