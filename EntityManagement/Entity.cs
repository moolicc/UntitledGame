using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UntitledGame.EntityManagement
{
    class Entity
    {
        public EntityManager EntityManager { get; private set; }

        private List<IComponent> _components;

        public Entity()
        {
            _components = new List<IComponent>();
        }

        public void Bind(EntityManager manager)
        {
            if(EntityManager != null)
            {
                throw new InvalidOperationException("Entity already bound to an EntityManager.");
            }
            EntityManager = manager;
        }

        public void Unbind()
        {
            if(EntityManager == null)
            {
                throw new InvalidOperationException("Entity not bound to an EntityManager.");
            }
            EntityManager = null;
        }

        public bool ContainsComponent<T>() where T : IComponent
        {
            return _components.Any(c => c is T);
        }
        public bool ContainsComponent(Type componentType)
        {
            return _components.Any(c => c.GetType() == componentType);
        }

        public void AddComponent(IComponent component)
        {
            _components.Add(component);

            if (EntityManager != null)
            {
                EntityManager.MessageDispatcher.Invoke(EntityManager.MESSAGE_ENTITY_ALTERED, this);
            }
        }

        public void RemoveComponent(IComponent component)
        {
            _components.Remove(component);

            if (EntityManager != null)
            {
                EntityManager.MessageDispatcher.Invoke(EntityManager.MESSAGE_ENTITY_ALTERED, this);
            }
        }

        public T GetComponent<T>() where T : IComponent
        {
            return (T)_components.FirstOrDefault(c => c is T);
        }

        public IComponent GetComponent(Type componentType)
        {
            return _components.FirstOrDefault(c => c.GetType() == componentType);
        }

        public IEnumerable<IComponent> GetComponents(params Type[] componentTypes)
        {
            foreach (var item in _components)
            {
                if(componentTypes.Contains(item.GetType()))
                {
                    yield return item;
                }
            }
        }

        public IEnumerable<IComponent> GetComponents()
        {
            return _components;
        }
    }
}
