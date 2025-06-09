using System;
using UnityEngine;

namespace MyUnityPackage.Interactions
{
    public abstract class ACondition : MonoBehaviour
    {
        public bool shouldBeTrue = true;
        public event Action onConditionMet;
        public abstract bool CheckCondition();

        protected virtual void OnConditionMet(bool conditionMet)
        {
            Debug.Log("OnConditionMet : " + conditionMet);
            //Probleme ici
            if (conditionMet == shouldBeTrue)
                onConditionMet?.Invoke();
        }
    }
}