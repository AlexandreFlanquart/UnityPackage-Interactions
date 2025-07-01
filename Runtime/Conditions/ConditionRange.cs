using UnityEngine;

namespace MyUnityPackage.Interactions
{
    public class ConditionRange : ACondition
    {
        [SerializeField] private RangeHandler rangeHandler;

        void OnEnable()
        {
            rangeHandler.onRangeEnter += OnRangeEnter;
            rangeHandler.onRangeExit += OnRangeExit;
        }

        void OnDisable()
        {
            rangeHandler.onRangeEnter -= OnRangeEnter;
            rangeHandler.onRangeExit -= OnRangeExit;
        }

        private void OnRangeEnter()
        {
            isReady = true;
            OnConditionMet(isReady);
        }

        private void OnRangeExit()
        {
            isReady = false;
            OnConditionMet(isReady);
        }

        public override bool CheckCondition()
        {
            Debug.Log("CheckCondition In Range " + isReady);
            return isReady;
        }
    }
}
