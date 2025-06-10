using System;
using UnityEngine;

namespace MyUnityPackage.Interactions
{
    public abstract class ACondition : MonoBehaviour
    {
        public bool shouldBeTrue = true;
        public event Action<bool> onConditionMet;
        public abstract bool CheckCondition();

        public bool requiredForEnter;

        protected virtual void OnConditionMet(bool conditionMet)
        {
            Debug.Log("OnConditionMet : " + conditionMet);
            onConditionMet?.Invoke(conditionMet);
        }
    }
}