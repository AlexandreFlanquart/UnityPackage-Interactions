using System;
using TMPro;
using UnityEngine;

public class GameManagerInteractions : MonoBehaviour
{
    static GameManagerInteractions instance;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI interactionDoneText;

    private int nbInteractionDone = 0;
    private int _currentLvl = 0;
    private int CurrentLvl
    {
        get => _currentLvl;
        set
        {
            _currentLvl = value;
            levelText.text = lvlText + CurrentLvl.ToString();
            OnLevelChange?.Invoke(CurrentLvl);
        }
    }
    public event Action<int> OnLevelChange;

    const string lvlInteraction = "Interactions : ";
    const string lvlText = "Level : ";

    void Awake()
    {
        if (instance == null)
            instance = this;

        interactionDoneText.text = lvlInteraction + nbInteractionDone.ToString();
        levelText.text = lvlText + CurrentLvl;
    }

    public static GameManagerInteractions GetInstance()
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
        CurrentLvl = lvl;
    }
    public int GetLevel()
    {
        return CurrentLvl;
    }
    public void AddLevel()
    {
        SetLevel(++CurrentLvl);
    }
    public void RemoveLevel()
    {
        SetLevel(--CurrentLvl);
    }


}
