using System.Collections.Generic;
using UnityEngine;
namespace RyanPierce.Events
{
    public abstract class BaseGameEvent<T> : ScriptableObject
    {
        protected readonly List<IGameEventListener<T>> eventListeners = new List<IGameEventListener<T>>();

        public virtual void Raise(T item)
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
            {
                eventListeners[i].OnEventRaised(item);
            }
        }

        public void RegisterListener(IGameEventListener<T> listener)
        {
            //Make sure it doesnt contain the listener
            if (eventListeners.Contains(listener)) { return; }

            eventListeners.Add(listener);
        }

        public void UnRegisterListener(IGameEventListener<T> listener)
        {
            //Make sure contains the listener
            if (!eventListeners.Contains(listener)) { return; }

            eventListeners.Remove(listener);
        }
    }
}