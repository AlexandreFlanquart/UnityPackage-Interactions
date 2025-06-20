using UnityEngine;
using MyUnityPackage.Toolkit;

namespace MyUnityPackage.Interactions
{
    public class PlayerMovement : MonoBehaviour
    {
        Rigidbody rb;

        float pitch;
        float LookSensitivity = 0.5f;
        float MaxPitch = 89f;
        Transform vCam;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {


            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;


            rb = GetComponent<Rigidbody>();
            ServiceLocator.GetService<InputManager>().OnPressDirection += Move;
            ServiceLocator.GetService<InputManager>().OnLookDirection += LookAround;
            vCam = Camera.main.transform;
        }

        private void Move(Vector2 move)
        {
            Vector3 v = new Vector3(move.x / 5, 0, move.y / 5);
            v = transform.TransformDirection(v);
            rb.MovePosition(transform.position + v);
            //transform.position += new Vector3(move.x / 5, 0, move.y / 5);
        }

        private void LookAround(Vector2 look)
        {
            pitch += look.y * LookSensitivity * -1;
            pitch = Mathf.Clamp(pitch, -MaxPitch, MaxPitch);

            vCam.localRotation = Quaternion.Euler(pitch, 0, 0);
            transform.Rotate(0, look.x * LookSensitivity, 0);
        }
    }
}
