using TMPro;
using UnityEngine;

namespace Core.Demo.Scripts.Model
{
    [RequireComponent(typeof(Renderer))]
    public class InstancedObject : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        private Renderer _renderer;

        public void Initialize(Color color, string str)
        {
            _renderer = GetComponent<Renderer>();
            _renderer.material.color = color;
            text.text = str;
        }
    }
}