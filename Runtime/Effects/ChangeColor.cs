using UnityEngine;

namespace MyUnityPackage.Interactions
{
    public class ChangeColor : AEffect
    {
        [SerializeField] private MeshRenderer mesh;

        public override void OnEnable() { }
        public override void OnDisable() { }

        public override void OnEnter()
        {
            Debug.Log("Blue");
            mesh.material.SetColor("_BaseColor", Color.blue);
        }
        public override void OnInteract()
        {
            Debug.Log("Green");
            mesh.material.SetColor("_BaseColor", Color.green);
        }
        public override void OnExit()
        {
            Debug.Log("Red");
            mesh.material.SetColor("_BaseColor", Color.red);
        }
    }
}
