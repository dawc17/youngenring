using UnityEngine;
using UnityEngine.EventSystems;

namespace DKC
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        PlayerManager player;
        [HideInInspector] public float verticalMovement;
        [HideInInspector] public float horizontalMovement;
        [HideInInspector] public float moveAmount;
        
        [Header("Movement Settings")]
        private Vector3 targetRotationDirection;
        private Vector3 moveDirection;
        [SerializeField] float walkingSpeed = 2;
        [SerializeField] float runningSpeed = 5;
        [SerializeField] float rotationSpeed = 15;

        [Header("Dodge")]
        private Vector3 rollDirection;
        
        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Update()
        {
            base.Update();
            
            // if player is owner update values
            if (player.IsOwner)
            {
                player.characterNetworkManager.animatorVerticalParameter.Value = verticalMovement;
                player.characterNetworkManager.animatorHorizontalParameter.Value = horizontalMovement;
                player.characterNetworkManager.networkMoveAmount.Value = moveAmount;
            }
            else
            {   
                // if not, get values from network
                moveAmount = player.characterNetworkManager.networkMoveAmount.Value;
                verticalMovement = player.characterNetworkManager.animatorVerticalParameter.Value;
                horizontalMovement = player.characterNetworkManager.animatorHorizontalParameter.Value;
                
                // if not locked on pass move amount
                player.playerAnimationManager.UpdateAnimatorMovementParameters(0, moveAmount);
                
                // if locked on pass vertical and horizontal
            }
        }

        public void HandleAllMovement()
        {
            HandleGroundedMovement();
            HandleRotation();
        }

        private void GetMovementValues()
        {
            verticalMovement = PlayerInputManager.instance.verticalInput;
            horizontalMovement = PlayerInputManager.instance.horizontalInput;
            moveAmount = PlayerInputManager.instance.moveAmount;
            
            // clamp later
        }

        private void HandleGroundedMovement()
        {
            if (!player.canMove)
                return;
            
            GetMovementValues();
            // movement dir is based on camera direction
            moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
            moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0;

            if (PlayerInputManager.instance.moveAmount > 0.5f)
            {
                player.characterController.Move(moveDirection * (runningSpeed * Time.deltaTime));
            }
            else if (PlayerInputManager.instance.moveAmount <= 0.5f)
            {
                player.characterController.Move(moveDirection * (walkingSpeed * Time.deltaTime));
            }
        }

        private void HandleRotation()
        {
            if (!player.canRotate)
                return;
            
            targetRotationDirection = Vector3.zero;
            targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
            targetRotationDirection = targetRotationDirection + PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0;
            
            if (targetRotationDirection == Vector3.zero)
            {
                targetRotationDirection = transform.forward;
            }
            
            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }

        public void AttemptToPerformDodge()
        {
            if (player.isPerformingAction)
                return;
            
            // if moving:
            if (PlayerInputManager.instance.moveAmount > 0)
            {
                rollDirection = PlayerCamera.instance.cameraObject.transform.forward *
                                PlayerInputManager.instance.verticalInput;
                rollDirection += PlayerCamera.instance.cameraObject.transform.right *
                                 PlayerInputManager.instance.horizontalInput;

                rollDirection.y = 0;
                rollDirection.Normalize();
                Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
                player.transform.rotation = playerRotation;
                
                player.playerAnimationManager.PlayRollAnimation();
            }
            // if stationary:
            else
            {
                player.playerAnimationManager.PlayBackstepAnimation();
            }
        }

        public void AttemptToPerformGay()
        {
            if (player.isPerformingAction)
                return;
            player.playerAnimationManager.PlayGayAnimation();
        }
    }
}

