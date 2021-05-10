using System.Collections.Generic;
using System.Linq;
using Core.Cross.Events.Interface;
using Core.Extensions;

namespace Core.Managers
{
    public static class GameManager
    {
        private static IList<ICrossEventHandler> _handlers;

        /// <summary>
        /// Initialising cross subscriptions for all handlers in scene.
        /// </summary>
        public static void InitializeSubscriptions()
        {
            if (!UnityExtensions.TryToFindObjectsOfType(out _handlers) ||
                !UnityExtensions.TryToFindObjectsOfType(out IList<ICrossEventSubscriber> crossEventSubscribers))
                return;
            foreach (var crossEventHandler in _handlers) crossEventHandler.Subscribe(crossEventSubscribers.SelectMany(x => x.GetSubscribers()));
        }

        public static void Subscribe(ICrossEventSubscriber subscriber)
        {
            foreach (var crossEventHandler in _handlers) crossEventHandler.Subscribe(subscriber.GetSubscribers());
        }

        public static void Unsubscribe(ICrossEventSubscriber subscriber)
        {
            foreach (var crossEventHandler in _handlers) crossEventHandler.Unsubscribe(subscriber.GetSubscribers());
        }

        public static void AddHandler(ICrossEventHandler handler, bool invokeNeeded)
        {
            _handlers.Add(handler);
            if (UnityExtensions.TryToFindObjectsOfType(out IList<ICrossEventSubscriber> crossEventSubscribers)) return;
            handler.Subscribe(crossEventSubscribers.SelectMany(x => x.GetSubscribers()));
            if (invokeNeeded) handler.InvokeEvents();
        }

        public static void RemoveHandler(ICrossEventHandler handler)
        {
            _handlers.Remove(handler);
        }

        /// <summary>
        /// Invoking event on handlers.
        /// </summary>
        public static void InvokeBase()
        {
            if (!UnityExtensions.TryToFindObjectsOfType(out IList<ICrossEventHandler> handlers)) return;
            foreach (var handler in handlers) handler.InvokeEvents();
        }
    }
}
