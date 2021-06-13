using UnityEngine;

namespace CorePlugin.Console
{
    sealed class LogIcons : ScriptableObject
    {
        [SerializeField] private Sprite infoActive;
        [SerializeField] private Sprite infoInactive;

        [SerializeField] private Sprite warningActive;
        [SerializeField] private Sprite warningInactive;
        [SerializeField] private Sprite errorActive;
        [SerializeField] private Sprite errorInactive;

        public Sprite InfoActive => infoActive;

        public Sprite WarningActive => warningActive;

        public Sprite ErrorActive => errorActive;

        public Sprite InfoInactive => infoInactive;

        public Sprite WarningInactive => warningInactive;

        public Sprite ErrorInactive => errorInactive;
    }
}
