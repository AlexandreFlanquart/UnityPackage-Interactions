using UnityEngine;
using MyUnityPackage.Interactions;

public class ChangeColorEffect : AEffect
{
    [SerializeField] private MeshRenderer mesh;

    public override void OnEnable() { }
    public override void OnDisable() { }

    public override void OnEnter()
    {
        mesh.material.SetColor("_BaseColor", Color.blue);
    }
    public override void OnInteract()
    {
        mesh.material.SetColor("_BaseColor", Color.green);
    }
    public override void OnExit()
    {
        mesh.material.SetColor("_BaseColor", Color.red);
    }
}

