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

namespace Core.Interface
{
    /// <summary>
    /// Interface for deep cloning
    /// </summary>
    public interface IDeepClone<out T> : IDeepClone
    {
        /// <summary>
        /// Clones instance of an object
        /// </summary>
        /// <returns></returns>
        public new T DeepClone();
    }

    /// <summary>
    /// Interface for deep cloning
    /// </summary>
    public interface IDeepClone
    {
        /// <summary>
        /// Clones instance of an object
        /// </summary>
        /// <returns></returns>
        public object DeepClone();
    }
}
