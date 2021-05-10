using System;
using System.Collections.Generic;

namespace Core.Cross.Events.Interface
{
    /// <summary>
    /// Interface for event handler.
    /// <code>
    /// public void Subscribe(params Delegate[] subscriber)
    /// {
    ///     foreach (var action in subscriber.OfType<CrossEventTypes.DelegateClass/>())
    ///     {
    ///         MyEvent += action;
    ///     }
    /// }
    ///
    /// </code>
    /// </summary>
    public interface ICrossEventHandler
    {
        /// <summary>
        /// Invoking events what need to be invoked on scene initializing.
        /// </summary>
        public void InvokeEvents();

        public void Subscribe(IEnumerable<Delegate> subscribers);

        public void Unsubscribe(IEnumerable<Delegate> unsubscribers);
    }
}
