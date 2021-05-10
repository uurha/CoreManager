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
