using System;
using UnityEngine;

namespace MyUnityPackage.Interactions
{
    public class ConditionRange : ACondition
    {
        [SerializeField] private RangeHandler rangeHandler;

        private bool inRange = false;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

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
            inRange = true;
            OnConditionMet(true);
        }

        private void OnRangeExit()
        {
            Debug.Log("Range Exit");
            inRange = false;
            OnConditionMet(false);
        }

        public override bool CheckCondition()
        {
            Debug.Log("In Range " + inRange);
            return inRange;
        }
    }
}
