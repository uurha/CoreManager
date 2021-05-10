using System;
using System.Collections.Generic;

namespace Core.Cross.Events.Interface
{

    /// <summary>
    /// Interface for subscribers.
    /// <code>
    /// public IEnumerable<Delegate/> GetSubscribers()
    /// {
    ///     var list = new Delegate[] {(CrossEventTypes.DelegateClass) MyMethod, (CrossEventTypes.DelegateClass2) MyMethod2};
    ///     return list;
    /// }
    /// </code>
    /// </summary>
    public interface ICrossEventSubscriber
    {
        /// <summary>
        /// Returns IEnumerable with all methods which need to be subscribed.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Delegate> GetSubscribers();
    }
}
