using System;
using UnityEngine;

public abstract class ACondition : MonoBehaviour
{
    public bool shouldBeTrue = true;
    public event Action<bool> onConditionMet;
    public abstract bool CheckCondition();

    protected virtual void OnConditionMet(bool conditionMet)
    {
        onConditionMet?.Invoke(conditionMet);
    }
}
