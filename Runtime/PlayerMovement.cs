using UnityEngine;
using MyUnityPackage.Toolkit;

namespace MyUnityPackage.Interactions
{
    public class PlayerMovement : MonoBehaviour
    {
        Rigidbody rb;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            ServiceLocator.GetService<InputManager>().OnPressDirection += Move;
        }

        private void Move(Vector2 move)
        {
            Vector3 v = new Vector3(move.x / 5, 0, move.y / 5);
            rb.MovePosition(transform.position + v);
            //transform.position += new Vector3(move.x / 5, 0, move.y / 5);
        }
    }
}
