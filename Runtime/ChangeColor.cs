using UnityEngine;

namespace MyUnityPackage.Interactions
{
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
            rangeHandler.onRangeEnter += ChangeColorGreen;
            rangeHandler.onRangeExit += ChangeColorRed;
        }

        public void OnPlayerMoveInRange(float pDistance)
        {
            Color color = new Color(pDistance / rangeHandler.MaxDistance, 1 - pDistance / rangeHandler.MaxDistance, 0, 1);
            mesh.material.SetColor("_BaseColor", color);
        }

        public void ChangeColorBlue()
        {
            Debug.Log("Blue");
            mesh.material.SetColor("_BaseColor", Color.blue);
        }
        public void ChangeColorGreen()
        {
            Debug.Log("Green");
            mesh.material.SetColor("_BaseColor", Color.green);
        }
        public void ChangeColorRed()
        {
            Debug.Log("Red");
            mesh.material.SetColor("_BaseColor", Color.red);
        }
    }
}
