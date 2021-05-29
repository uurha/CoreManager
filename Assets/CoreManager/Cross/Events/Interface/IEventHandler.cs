﻿#region license

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

using System;
using System.Collections.Generic;

namespace CoreManager.Cross.Events.Interface
{
    /// <summary>
    /// Interface for event handler.
    /// <code>
    /// <br/>public void Subscribe(params Delegate[] subscriber)
    /// <br/>{
    /// <br/>    foreach (var action in subscriber.OfType<EventTypes.DelegateClass>())
    /// <br/>    {
    /// <br/>        MyEvent += action;
    /// <br/>    }
    /// <br/>}
    /// </code>
    /// <seealso cref="EventTypes"/>
    /// </summary>
    public interface IEventHandler
    {
        /// <summary>
        /// Invoking events that need to be invoked on scene initialization.
        /// </summary>
        public void InvokeEvents();

        /// <summary>
        /// Subscribing delegates to event
        /// </summary>
        /// <param name="subscribers"></param>
        public void Subscribe(IEnumerable<Delegate> subscribers);

        /// <summary>
        /// Unsubscribing delegates to event
        /// </summary>
        /// <param name="unsubscribers"></param>
        public void Unsubscribe(IEnumerable<Delegate> unsubscribers);
    }
}
