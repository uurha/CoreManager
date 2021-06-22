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

using CorePlugin.Attributes.Validation;
using CorePlugin.Cross.SceneData;
using CorePlugin.Samples.Scripts.Model;
using TMPro;
using UnityEngine;

namespace CorePlugin.Samples.Scripts.Demo
{
    public class CrossSceneDataReceiverDemo : MonoBehaviour
    {
        [SerializeField] [NotNull] private TMP_Text intText;
        [SerializeField] [NotNull] private TMP_Text colorText;
        [SerializeField] [NotNull] private TMP_Text stringText;

        private void Awake()
        {
            ReceiveData();
        }

        public void ReceiveData()
        {
            if (!SceneDataHandler.Instance.GetData(out DataTransfer data)) return;
            intText.text = data.IntData.ToString();
            colorText.text = data.ColorData.ToString();
            stringText.text = data.StrData;
        }
    }
}
