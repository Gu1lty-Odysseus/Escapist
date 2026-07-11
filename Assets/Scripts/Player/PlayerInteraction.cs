using UnityEngine;
using UnityEngine.InputSystem;

namespace Escapist.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("Raycast Configurations")]
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float interactRange = 3.0f;
        [SerializeField] private LayerMask interactableLayer;

        private InputAction interactAction;
        private IInteractable currentInteractableTarget;

        private void Start()
        {
            var inputActions = new InputSystem_Actions();
            inputActions.Player.Enable();
            interactAction = inputActions.Player.Interact;

            interactAction.performed += OnInteractPressed;
        }

        private void Update()
        {
            PerformInteractionCheck();
        }

        private void PerformInteractionCheck()
        {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            
            if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
            {
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    if (currentInteractableTarget != interactable)
                    {
                        currentInteractableTarget = interactable;
                        // Design hook: You can invoke a UI event here to render currentInteractableTarget.InteractionPrompt onto the player canvas
                    }
                    return;
                }
            }

            // Lose targeted object reference if raycast misses
            currentInteractableTarget = null;
        }

        private void OnInteractPressed(InputAction.CallbackContext context)
        {
            if (currentInteractableTarget != null)
            {
                currentInteractableTarget.Interact();
            }
        }

        private void OnDestroy()
        {
            if (interactAction != null)
            {
                interactAction.performed -= OnInteractPressed;
            }
        }
    }
}