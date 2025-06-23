using System;
using MyUnityPackage.Toolkit;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI interactionDoneText;

    private int nbInteractionDone = 0;
    private int currentLvl = 0;
    public event Action<int> OnLevelChange;

    void Awake()
    {
        ServiceLocator.AddService<GameManager>(gameObject);
    }

    public void IncrementInteractionCount()
    {
        const string lvlInteraction = "Interactions : ";
        nbInteractionDone++;
        interactionDoneText.text = lvlInteraction + nbInteractionDone.ToString();
    }

    public void SetLevel(int lvl)
    {
        Debug.Log("Lvl " + lvl);
        currentLvl = lvl;
        const string lvlText = "Level : ";
        levelText.text = lvlText + lvl.ToString();
        OnLevelChange?.Invoke(currentLvl);
    }
    public void AddLevel()
    {
        SetLevel(++currentLvl);
    }
    public void RemoveLevel()
    {
        SetLevel(--currentLvl);
    }


}
