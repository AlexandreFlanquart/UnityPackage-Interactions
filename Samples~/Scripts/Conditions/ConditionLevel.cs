using UnityEngine;
using MyUnityPackage.Interactions;

namespace MyUnityPackage.Interactions.Samples
{
    public class ConditionLevel : ACondition
{
    private GameManagerInteractions gameManager;

    public void Start()
    {
        gameManager = GameManagerInteractions.GetInstance();
        gameManager.OnLevelChange += OnLevelChanged;
    }

    protected override bool EvaluateCondition()
    {
        return isReady;
    }

    private void OnLevelChanged(int lvl)
    {
        isReady = lvl >= 10;
        OnConditionMet(isReady);
    }
}
}

