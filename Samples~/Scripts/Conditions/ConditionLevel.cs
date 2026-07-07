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

    void OnEnable()
    {
        // Before Start() the manager isn't cached yet — Start() handles the first subscription
        if (gameManager == null) return;

        gameManager.OnLevelChange += OnLevelChanged;
        // Re-sync: the level may have changed while we were disabled
        OnLevelChanged(gameManager.GetLevel());
    }

    void OnDisable()
    {
        if (gameManager == null) return;

        gameManager.OnLevelChange -= OnLevelChanged;
        ResetReadyState();
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

