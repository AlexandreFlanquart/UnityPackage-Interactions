using UnityEngine;

namespace MyUnityPackage.Interactions
{
    public abstract class AEffect : MonoBehaviour
    {
        public abstract void OnEnable();
        public abstract void OnDisable();
        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void OnInteract();
    }
}
