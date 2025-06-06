using UnityEngine;

public class ChangeColor : MonoBehaviour
{

    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private RangeHandler rangeHandler;

    [SerializeField] bool isColliding;

    void Start()
    {
        if (isColliding)
            return;
        rangeHandler.onPlayerMoveInRange += OnPlayerMoveInRange;
        rangeHandler.onRangeEnter += ColorOnEnter;
        rangeHandler.onRangeExit += ColorOnExit;
    }

    public void OnPlayerMoveInRange(float pDistance)
    {

        Color color = new Color(pDistance / rangeHandler.MaxDistance, 1 - pDistance / rangeHandler.MaxDistance, 0, 1);
        mesh.material.SetColor("_BaseColor", color);
    }
    public void ColorOnEnter()
    {
        Debug.Log("OnEnter");
        mesh.material.SetColor("_BaseColor", Color.green);
    }
    public void ColorOnExit()
    {
        Debug.Log("OnExit");
        mesh.material.SetColor("_BaseColor", Color.red);
    }
}
