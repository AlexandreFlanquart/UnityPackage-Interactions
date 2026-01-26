using UnityEngine;

namespace MyUnityPackage.Interactions.Samples
{
    public class ChangeColorEffect : AEffect
{
    [SerializeField] private MeshRenderer mesh;

    public override void ActivateEffect()
    {
        if (mesh == null) return;
        mesh.material.SetColor("_BaseColor", Color.gray);
    }

    public override void DeactivateEffect()
    {
        if (mesh == null) return;
        mesh.material.SetColor("_BaseColor", Color.black);
    }

    public override void EnterEffect()
    {
        if (mesh == null) return;
        mesh.material.SetColor("_BaseColor", Color.blue);
    }
    public override void InteractEffect()
    {
        if (mesh == null) return;
        mesh.material.SetColor("_BaseColor", Color.green);
    }
    public override void ExitEffect()
    {
        if (mesh == null) return;
        mesh.material.SetColor("_BaseColor", Color.red);
    }
}
}

