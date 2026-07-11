using UnityEngine;
using UnityEngine.InputSystem;

namespace Escapist.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float walkSpeed = 4.0f;
        [SerializeField] private float crouchSpeed = 2.0f;
        [SerializeField] private float gravity = -9.81f;

        [Header("Crouch Configuration")]
        [SerializeField] private float standHeight = 2.0f;
        [SerializeField] private float crouchHeight = 1.0f;
        [SerializeField] private float crouchTransitionSpeed = 10f;
        [SerializeField] private Transform cameraHolder;

        // Dependencies
        private CharacterController characterController;
        private InputAction moveAction;
        private InputAction crouchAction;

        // Internal State
        private Vector2 currentInputVector;
        private Vector3 velocity;
        private bool isCrouching;
        private float targetHeight;

        public bool IsMoving => characterController.isGrounded && currentInputVector.sqrMagnitude > 0.01f;
        public bool IsCrouching => isCrouching;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            targetHeight = standHeight;
        }

        private void Start()
        {
            // Initializing through the automatically bound InputSystem_Actions asset maps
            var inputActions = new InputSystem_Actions();
            inputActions.Player.Enable();

            moveAction = inputActions.Player.Move;
            crouchAction = inputActions.Player.Crouch;

            // Bind Crouch Contexts
            crouchAction.started += _ => ToggleCrouch(true);
            crouchAction.canceled += _ => ToggleCrouch(false);
        }

        private void Update()
        {
            HandleInput();
            ApplyMovement();
            ApplyCrouchTransition();
        }

        private void HandleInput()
        {
            currentInputVector = moveAction.ReadValue<Vector2>();
        }

        private void ApplyMovement()
        {
            // Reset gravity velocity if already grounded
            if (characterController.isGrounded && velocity.y < 0)
            {
                velocity.y = -2f; 
            }

            float currentSpeed = isCrouching ? crouchSpeed : walkSpeed;
            
            // Calculate direction relative to transform rotation orienting with mouse-look
            Vector3 moveDirection = transform.forward * currentInputVector.y + transform.right * currentInputVector.x;
            characterController.Move(moveDirection * currentSpeed * Time.deltaTime);

            // Gravity calculations
            velocity.y += gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }

        private void ToggleCrouch(bool state)
        {
            isCrouching = state;
            targetHeight = isCrouching ? crouchHeight : standHeight;
        }

        private void ApplyCrouchTransition()
        {
            // Smoothly lerp height configuration parameters to prevent sudden jarring frame snaps
            if (!Mathf.Approximately(characterController.height, targetHeight))
            {
                characterController.height = Mathf.Lerp(characterController.height, targetHeight, crouchTransitionSpeed * Time.deltaTime);
                
                // Adjust position of camera relative to center anchor mapping
                Vector3 camPos = cameraHolder.localPosition;
                camPos.y = Mathf.Lerp(camPos.y, targetHeight * 0.45f, crouchTransitionSpeed * Time.deltaTime);
                cameraHolder.localPosition = camPos;
            }
        }
    }
}