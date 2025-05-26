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
        [SerializeField] float walkingSpeed = 2;
        [SerializeField] float runningSpeed = 5;
        [SerializeField] float sprintingSpeed = 7;
        [SerializeField] float rotationSpeed = 15;
        [SerializeField] int sprintingStaminaCost = 2;
        private Vector3 targetRotationDirection;
        private Vector3 moveDirection;

        [Header("Jump")]
        [SerializeField] private float jumpStaminaCost = 10;
        [SerializeField] float jumpHeight = 2.5f;
        [SerializeField] float jumpForwardSpeed = 5;
        [SerializeField] float freeFallSpeed = 2;
        private Vector3 jumpDirection;

        [Header("Dodge")]
        [SerializeField] private float dodgeStaminaCost = 25;
        [SerializeField] private float backstepStaminaCost = 10;
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
                if (!player.playerNetworkManager.isLockedOn.Value || player.playerNetworkManager.isSprinting.Value)
                {
                    player.playerAnimationManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
                }
                // if locked on pass vertical and horizontal
                else
                {
                    // if we are locked on, pass the horizontal input
                    player.playerAnimationManager.UpdateAnimatorMovementParameters(horizontalMovement, verticalMovement, player.playerNetworkManager.isSprinting.Value);
                }
            }
        }

        public void HandleAllMovement()
        {
            HandleGroundedMovement();
            HandleRotation();
            HandleJumpingMovement();
            HandleFreeFallMovement();
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
            if (!canMove)
                return;

            GetMovementValues();
            // movement dir is based on camera direction
            moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
            moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0;


            if (player.playerNetworkManager.isSprinting.Value)
            {
                player.characterController.Move(moveDirection * (sprintingSpeed * Time.deltaTime));
            }
            else
            {
                if (PlayerInputManager.instance.moveAmount > 0.5f)
                {
                    player.characterController.Move(moveDirection * (runningSpeed * Time.deltaTime));
                }
                else if (PlayerInputManager.instance.moveAmount <= 0.5f)
                {
                    player.characterController.Move(moveDirection * (walkingSpeed * Time.deltaTime));
                }
            }
        }

        private void HandleJumpingMovement()
        {
            if (player.playerNetworkManager.isJumping.Value)
            {
                player.characterController.Move(jumpDirection * jumpForwardSpeed * Time.deltaTime);
            }
        }

        private void HandleFreeFallMovement()
        {
            if (!isGrounded)
            {
                Vector3 freeFallDirection;

                freeFallDirection = PlayerCamera.instance.transform.forward * PlayerInputManager.instance.verticalInput;
                freeFallDirection = freeFallDirection + PlayerCamera.instance.transform.right * PlayerInputManager.instance.horizontalInput;
                freeFallDirection.y = 0;

                player.characterController.Move(freeFallDirection * freeFallSpeed * Time.deltaTime);
            }
        }

        private void HandleRotation()
        {
            if (player.isDead.Value)
                return;

            if (!canRotate)
                return;

            if (player.playerNetworkManager.isLockedOn.Value)
            {
                if (player.playerNetworkManager.isSprinting.Value || player.playerLocomotionManager.isRolling)
                {
                    Vector3 targetDirection = Vector3.zero;
                    targetDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
                    targetDirection += PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
                    targetDirection.Normalize();
                    targetDirection.y = 0;

                    if (targetDirection == Vector3.zero)
                    {
                        targetDirection = transform.forward;
                    }

                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    transform.rotation = finalRotation;
                }
                else
                {
                    if (player.playerCombatManager.currentTarget == null)
                        return;

                    Vector3 targetDirection;
                    targetDirection = player.playerCombatManager.currentTarget.transform.position - transform.position;
                    targetDirection.y = 0;
                    targetDirection.Normalize();

                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    transform.rotation = finalRotation;
                }
            }
            else
            {
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
        }

        public void HandleSprinting()
        {
            if (player.isPerformingAction)
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }

            if (player.playerNetworkManager.currentStamina.Value <= 0)
            {
                player.playerNetworkManager.isSprinting.Value = false;
                return;
            }

            if (moveAmount >= 0.5)
            {
                player.playerNetworkManager.isSprinting.Value = true;
            }
            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }

            if (player.playerNetworkManager.isSprinting.Value)
            {
                player.playerNetworkManager.currentStamina.Value -= sprintingStaminaCost * Time.deltaTime;
            }
        }

        public void AttemptToPerformDodge()
        {
            if (player.isPerformingAction)
                return;

            if (player.playerNetworkManager.currentStamina.Value <= 0)
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
                player.characterSFXManager.PlayRollSFX();
                player.playerNetworkManager.currentStamina.Value -= dodgeStaminaCost;
            }
            // if stationary:
            else
            {
                player.playerAnimationManager.PlayBackstepAnimation();
                player.playerNetworkManager.currentStamina.Value -= backstepStaminaCost;
            }
        }

        public void AttemptToPerformJump()
        {
            // if performing action do not allow jump (will change when combat is added)
            if (player.isPerformingAction)
                return;

            // if we are out of stamina, return
            if (player.playerNetworkManager.currentStamina.Value <= 0)
                return;

            // if we are already jumping, return
            if (player.playerNetworkManager.isJumping.Value)
                return;

            // if we are in the air, return!!!
            if (!isGrounded)
                return;

            // lose stamina
            player.playerAnimationManager.PlayTargetActionAnimation("Main_Jump_01", false);
            player.playerNetworkManager.isJumping.Value = true;

            player.playerNetworkManager.currentStamina.Value -= jumpStaminaCost;

            jumpDirection = PlayerCamera.instance.cameraObject.transform.forward *
                                 PlayerInputManager.instance.verticalInput;
            jumpDirection += PlayerCamera.instance.cameraObject.transform.right *
                                 PlayerInputManager.instance.horizontalInput;

            jumpDirection.y = 0;
            jumpDirection.Normalize();

            if (jumpDirection != Vector3.zero)
            {

                // if sprinting, thunderfuck at full speed
                if (player.playerNetworkManager.isSprinting.Value)
                {
                    jumpDirection *= 1;
                }
                // if running, dont thunderfuck as far
                else if (PlayerInputManager.instance.moveAmount > 0.5f)
                {
                    jumpDirection *= 0.5f;
                }
                // if walking dont thunderfuck at all
                else if (PlayerInputManager.instance.moveAmount <= 0.5f)
                {
                    jumpDirection *= 0.25f;
                }
            }
        }

        public void ApplyJumpingVelocity()
        {
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
        }

    }
}


