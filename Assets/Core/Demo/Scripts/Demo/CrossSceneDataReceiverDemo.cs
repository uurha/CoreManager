using Core.Cross.SceneData;
using Core.CustomAttributes.Validation;
using Core.Demo.Scripts.Model;
using TMPro;
using UnityEngine;

namespace Core.Demo.Scripts.Demo
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
            if (!CrossSceneDataHandler.Instance.GetData(out DataTransfer data)) return;
            intText.text = data.IntData.ToString();
            colorText.text = data.ColorData.ToString();
            stringText.text = data.StrData;
        }
    }
}
