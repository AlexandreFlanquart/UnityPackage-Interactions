using MyUnityPackage.Toolkit;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int nbInteractionDone = 0;

    void Awake()
    {
        ServiceLocator.AddService<GameManager>(gameObject);
    }

    public void IncrementInteractionCount()
    {
        nbInteractionDone++;
    }

}
