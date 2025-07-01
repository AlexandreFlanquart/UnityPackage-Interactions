using UnityEngine;
using MyUnityPackage.Interactions;

public class ConditionLevel : ACondition
{
    private GameManager gameManager;

    public void Start()
    {
        gameManager = GameManager.GetInstance();
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

