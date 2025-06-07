using System;
using UnityEngine;

namespace MyUnityPackage.Interactions
{
    public abstract class AInteractionType : MonoBehaviour
    {
        public abstract event Action onEnter;
        public abstract event Action onExit;
        public abstract event Action onInteract;
    }
}

