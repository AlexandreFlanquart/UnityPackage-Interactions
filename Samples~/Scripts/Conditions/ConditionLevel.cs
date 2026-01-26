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

    public override bool CheckCondition()
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

