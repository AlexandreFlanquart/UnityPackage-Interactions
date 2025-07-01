using System;
using UnityEngine;

namespace MyUnityPackage.Interactions
{
    public abstract class ACondition : MonoBehaviour
    {
        protected bool shouldBeTrue = true;
        public bool requiredForEffects;
        protected bool isReady = false;

        public event Action<bool> onConditionMet;
        public abstract bool CheckCondition();

        protected virtual void OnConditionMet(bool conditionMet)
        {
            Debug.Log("OnConditionMet : " + conditionMet);
            onConditionMet?.Invoke(conditionMet);
        }
    }
}