using UnityEngine;
using MyUnityPackage.Interactions;

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
        Debug.Log("CheckCondition : " + isReady);
        return isReady;
    }

    private void OnLevelChanged(int lvl)
    {
        isReady = lvl >= 10;
        OnConditionMet(isReady);
    }
}

