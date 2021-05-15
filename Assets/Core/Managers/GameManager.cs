#region license

// Copyright 2021 Arcueid Elizabeth D'athemon
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
//     http://www.apache.org/licenses/LICENSE-2.0 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.

#endregion

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

        /// <summary>
        /// Subscribing event subscriber after scene Awoken to event handlers.
        /// </summary>
        /// <param name="subscriber"></param>
        public static void Subscribe(ICrossEventSubscriber subscriber)
        {
            foreach (var crossEventHandler in _handlers) crossEventHandler.Subscribe(subscriber.GetSubscribers());
        }

        /// <summary>
        /// Unsubscribing event subscriber after scene Awoken from event handlers.
        /// </summary>
        /// <param name="subscriber"></param>
        public static void Unsubscribe(ICrossEventSubscriber subscriber)
        {
            foreach (var crossEventHandler in _handlers) crossEventHandler.Unsubscribe(subscriber.GetSubscribers());
        }

        /// <summary>
        /// Adding new handler after scene Awoken to list of event handlers.
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="subscriptionsNeeded">If false invokeNeeded will not be called</param>
        /// <param name="invokeNeeded"></param>
        public static void AddHandler(ICrossEventHandler handler, bool subscriptionsNeeded = true, bool invokeNeeded = false)
        {
            _handlers.Add(handler);
            if (!subscriptionsNeeded) return;

            if (UnityExtensions.TryToFindObjectsOfType(out IList<ICrossEventSubscriber> crossEventSubscribers))
            {
                handler.Subscribe(crossEventSubscribers.SelectMany(x => x.GetSubscribers()));
            }
            if (invokeNeeded) handler.InvokeEvents();
        }

        /// <summary>
        /// Removing event handler after scene Awoken from list
        /// </summary>
        /// <param name="handler"></param>
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
