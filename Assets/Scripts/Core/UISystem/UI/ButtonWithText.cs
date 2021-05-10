using Core.CustomAttributes.Validation;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Core.UISystem.UI
{
    public class ButtonWithText : MonoBehaviour
    {
        [SerializeField] [NotNull] private Button button;
        [SerializeField] [NotNull] private TMP_Text textLabel;

        public UnityEvent onClick => button.onClick;

        public Color color
        {
            get => button.image.color;
            set => button.image.color = value;
        }

        public string text
        {
            set => textLabel.text = value;
            get => textLabel.text;
        }
    }
}
