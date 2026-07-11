using UnityEngine;
using UnityEngine.InputSystem;

namespace Escapist.Player
{
    public class CameraLookAndBob : MonoBehaviour
    {
        [Header("Look Sensitivities")]
        [SerializeField] private float mouseSensitivity = 15.0f;
        [SerializeField] private float upperLookLimit = 80.0f;
        [SerializeField] private float lowerLookLimit = -80.0f;

        [Header("Head Bob Settings")]
        [SerializeField] private float bobFrequency = 12.0f;
        [SerializeField] private float bobHorizontalAmplitude = 0.05f;
        [SerializeField] private float bobVerticalAmplitude = 0.05f;
        
        [Header("References")]
        [SerializeField] private PlayerMovement movementController;
        [SerializeField] private Transform cameraTransform;

        private InputAction lookAction;
        private float verticalRotation = 0f;
        private float bobTimer = 0f;
        private Vector3 cameraDefaultLocalPosition;

        private void Start()
        {
            var inputActions = new InputSystem_Actions();
            inputActions.Player.Enable();
            lookAction = inputActions.Player.Look;

            // Lock Cursor for clean FPS gameplay
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            cameraDefaultLocalPosition = cameraTransform.localPosition;
        }

        private void Update()
        {
            HandleLook();
            HandleHeadBob();
        }

        private void HandleLook()
        {
            Vector2 lookInput = lookAction.ReadValue<Vector2>() * mouseSensitivity * 0.05f;

            // Rotate Player transform layout horizontally (Yaw)
            transform.Rotate(Vector3.up * lookInput.x);

            // Rotate Camera container vertically (Pitch with clamping parameters)
            verticalRotation -= lookInput.y;
            verticalRotation = Mathf.Clamp(verticalRotation, lowerLookLimit, upperLookLimit);
            cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        }

        private void HandleHeadBob()
        {
            if (movementController.IsMoving)
            {
                // Speed up timer cadence dynamically depending on movement parameters
                bobTimer += Time.deltaTime * bobFrequency;
                
                // Construct natural sine wave calculation coordinates
                float newX = Mathf.Cos(bobTimer / 2) * bobHorizontalAmplitude;
                float newY = Mathf.Sin(bobTimer) * bobVerticalAmplitude;

                cameraTransform.localPosition = cameraDefaultLocalPosition + new Vector3(newX, newY, 0f);
            }
            else
            {
                // Return to structural zero offset state efficiently when standing still
                bobTimer = 0f;
                cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, cameraDefaultLocalPosition, Time.deltaTime * 5f);
            }
        }
    }
}