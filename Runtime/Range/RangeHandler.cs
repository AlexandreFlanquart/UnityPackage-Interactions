using System;
using Sirenix.OdinInspector;
using UnityEngine;
using MyUnityPackage.Toolkit;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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
        ServiceLocator.GetService<RangeChecker>().AddRangeElement(this);
    }

    public void UnregisterFromRangeChecker()
    {
        ServiceLocator.GetService<RangeChecker>().RemoveRangeElement(this);
    }

    public void ForceCalculateRange()
    {
        ServiceLocator.GetService<RangeChecker>().CalculateRange(this);
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
            //Debug.Log($"{transform.name} in range");
            onRangeEnter?.Invoke();
        }
        else
        {
            //Debug.Log($"{transform.name} out of range");
            onRangeExit?.Invoke();
        }
    }

    #region Utility
#if UNITY_EDITOR
    private Color gizmoColor = Color.cyan; // Couleur du Gizmo

    // Affiche le Gizmo dans la scène
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(Center.position, maxDistance); // Sphère filaire
    }
#endif
    #endregion
}
