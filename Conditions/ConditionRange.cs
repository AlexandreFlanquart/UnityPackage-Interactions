using System;
using UnityEngine;

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
        inRange = false;
        OnConditionMet(false);
    }

    public override bool CheckCondition()
    {
        return inRange;
    }
}
