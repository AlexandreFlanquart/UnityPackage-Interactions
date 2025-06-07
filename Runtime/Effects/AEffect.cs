using UnityEngine;

namespace MyUnityPackage.Interactions
{
    public abstract class AEffect : MonoBehaviour
    {
        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void OnInteract();
    }
}
