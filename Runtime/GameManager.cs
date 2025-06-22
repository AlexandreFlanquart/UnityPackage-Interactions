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
        nbInteractionDone++;
        interactionDoneText.text = nbInteractionDone++.ToString();
    }

    public void SetLevel(int lvl)
    {
        currentLvl = lvl;
        levelText.text = lvl.ToString();
        OnLevelChange?.Invoke(currentLvl);
    }
    public void AddLevel()
    {
        SetLevel(currentLvl++);
    }
    public void RemoveLevel()
    {
        SetLevel(currentLvl--);
    }


}
