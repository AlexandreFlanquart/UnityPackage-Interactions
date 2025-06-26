using System;
using MyUnityPackage.Toolkit;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    static GameManager instance;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI interactionDoneText;

    private int nbInteractionDone = 0;
    private int currentLvl = 0;
    public event Action<int> OnLevelChange;

    const string lvlInteraction = "Interactions : ";
    const string lvlText = "Level : ";
    void Awake()
    {
        if (instance == null)
            instance = this;
        //ServiceLocator.AddService<GameManager>(gameObject);

        interactionDoneText.text = lvlInteraction + nbInteractionDone.ToString();
        levelText.text = lvlText + currentLvl;
    }

    public static GameManager GetInstance()
    {
        return instance;
    }
    public void IncrementInteractionCount()
    {
        nbInteractionDone++;
        interactionDoneText.text = lvlInteraction + nbInteractionDone.ToString();
    }

    public void SetLevel(int lvl)
    {
        Debug.Log("Lvl " + lvl);
        currentLvl = lvl;

        levelText.text = lvlText + currentLvl.ToString();
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
