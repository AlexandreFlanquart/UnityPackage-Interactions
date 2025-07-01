using System;
using UnityEngine;

namespace MyUnityPackage.Interactions
{
    public class RangeHandler : MonoBehaviour
    {
        private enum RangeCheckerRegisterType { Auto, Manual }
        [SerializeField] private RangeCheckerRegisterType rangeCheckerRegisterType = RangeCheckerRegisterType.Auto;
        [SerializeField] private Transform center;
        [SerializeField] private float maxDistance;

        public event Action onRangeEnter;
        public event Action onRangeExit;
        public event Action<float> onPlayerMoveInRange;

        private bool inRange = false;

        public Transform Center { get => center != null ? center : transform; }
        public float MaxDistance { get => maxDistance; }
        public bool InRange { get => inRange; }

        void Start()
        {
            if (rangeCheckerRegisterType == RangeCheckerRegisterType.Auto)
            {
                RegisterToRangeChecker();
            }
        }
        void OnEnable()
        {
            if (rangeCheckerRegisterType == RangeCheckerRegisterType.Auto)
            {
                RegisterToRangeChecker();
            }
        }

        void OnDisable()
        {
            if (rangeCheckerRegisterType == RangeCheckerRegisterType.Auto)
            {
                UnregisterFromRangeChecker();
            }
        }

        public void RegisterToRangeChecker()
        {
            RangeChecker rangeChecker = RangeChecker.GetInstance();
            if (rangeChecker != null)
                RangeChecker.GetInstance().AddRangeElement(this);
        }

        public void UnregisterFromRangeChecker()
        {
            RangeChecker.GetInstance().RemoveRangeElement(this);
        }

        public void ForceCalculateRange()
        {
            RangeChecker.GetInstance().CalculateRange(this);
        }

        public void CheckRange(float pDistance)
        {
            SetInRange(pDistance <= maxDistance);
            if (inRange)
            {
                onPlayerMoveInRange?.Invoke(pDistance);
            }
        }

        public void SetInRange(bool pIsInRange)
        {
            if (inRange == pIsInRange) return;

            inRange = pIsInRange;
            if (inRange)
            {
                onRangeEnter?.Invoke();
            }
            else
            {
                onRangeExit?.Invoke();
            }
        }

        #region Utility
#if UNITY_EDITOR
        private Color gizmoColor = Color.cyan; 

        // Show Gizmo in scene
        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(Center.position, maxDistance); 
        }
#endif
        #endregion
    }
}
