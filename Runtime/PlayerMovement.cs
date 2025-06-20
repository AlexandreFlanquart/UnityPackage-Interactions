using UnityEngine;
using MyUnityPackage.Toolkit;

namespace MyUnityPackage.Interactions
{
    public class PlayerMovement : MonoBehaviour
    {
        Rigidbody rb;

        float pitch;
        float LookSensitivity = 0.4f;
        float lookX = 0f;
        float MaxPitch = 89f;
        Transform vCam;
        int maxBoucle = 20;
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

            Quaternion quatCam/*vCam.localRotation */= Quaternion.Euler(pitch, 0, 0);

            for (int i = 0; i < maxBoucle; i++)
                vCam.localRotation = Quaternion.Slerp(vCam.localRotation, quatCam, 1f / maxBoucle);
            //transform.Rotate(0, look.x * LookSensitivity, 0);

            lookX += look.x * LookSensitivity;
            Quaternion quat = Quaternion.Euler(0, lookX, 0);
            for (int i = 0; i < maxBoucle; i++)
                transform.rotation = Quaternion.Slerp(transform.rotation, quat, 1f / maxBoucle);
        }
    }
}
