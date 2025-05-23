using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DKC
{
    public class PlayerInputManager : MonoBehaviour
    {
        // 1. read player input
        // move chatacter based on input
        public static PlayerInputManager instance;
        public PlayerManager player;
        PlayerControls playerControls;

        [Header("Movement Input")]
        [SerializeField] Vector2 movement;
        public float verticalInput;
        public float horizontalInput;
        public float moveAmount;

        [Header("Camera Input")]
        [SerializeField] Vector2 cameraInput;
        public float cameraVerticalInput;
        public float cameraHorizontalInput;

        [Header("Lock On Input")]
        [SerializeField] bool lockOnInput = false;

        [Header("Player Action Input")]
        [SerializeField] bool dodgeInput = false;
        [SerializeField] bool sprintInput = false;
        [SerializeField] bool jumpInput = false;
        [SerializeField] bool rb_input = false;


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.activeSceneChanged += OnSceneChange;
            instance.enabled = false;

            if (playerControls != null)
            {
                playerControls.Disable();
            }
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if (newScene.buildIndex == WorldSaveGameManager.Instance.GetWorldSceneIndex())
            {
                instance.enabled = true;

                if (playerControls != null)
                {
                    playerControls.Enable();
                }
            }
            else
            {
                instance.enabled = false;

                if (playerControls != null)
                {
                    playerControls.Disable();
                }
            }
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;
        }

        // if window is minimized disable controls
        private void OnApplicationFocus(bool hasFocus)
        {
            if (enabled)
            {
                if (hasFocus)
                {
                    playerControls.Enable();
                }
                else
                {
                    playerControls.Disable();
                }
            }
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.PlayerMovement.Movement.performed += i => movement = i.ReadValue<Vector2>();
                playerControls.PlayerCamera.CameraControls.performed += i => cameraInput = i.ReadValue<Vector2>();
                playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
                playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
                playerControls.PlayerActions.RB.performed += i => rb_input = true;

                // lock on input
                playerControls.PlayerActions.LockOn.performed += i => lockOnInput = true;

                playerControls.UI.LockCursor.performed += i =>
                {
                    if (player == null)
                        return;

                    Cursor.lockState = CursorLockMode.Locked;
                };

                // holding sprint input
                playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
                playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;
            }

            playerControls.Enable();
        }

        private void Update()
        {
            HandleAllInputs();
        }

        private void HandleAllInputs()
        {
            HandleCameraMovementInput();
            HandlePlayerMovementInput();
            HandleDodgeInput();
            HandleSprintInput();
            HandleJumpInput();
            HandleRBInput();
            HandleLockOnInput();
        }

        // MOVEMENT

        private void HandlePlayerMovementInput()
        {
            verticalInput = movement.y;
            horizontalInput = movement.x;

            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

            if (moveAmount <= 0.5 && moveAmount > 0)
            {
                moveAmount = 0.5f;
            }
            else if (moveAmount > 0.5 && moveAmount <= 1)
            {
                moveAmount = 1;
            }
            // we pass 0 on the horizontal because we are not locked on
            // we use horizontal when strafing only
            if (player == null)
                return;

            player.playerAnimationManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);

            // if we are locked on pass the horizontal as well
        }

        // CAMERA
        private void HandleCameraMovementInput()
        {
            cameraVerticalInput = cameraInput.y;
            cameraHorizontalInput = cameraInput.x;
        }

        private void HandleLockOnInput()
        {
            if (player.playerNetworkManager.isLockedOn.Value)
            {
                if (player.playerCombatManager.currentTarget == null)
                    return;

                if (player.playerCombatManager.currentTarget.isDead.Value)
                {
                    player.playerNetworkManager.isLockedOn.Value = false;
                }

                // attempt to find new target
            }

            if (lockOnInput && player.playerNetworkManager.isLockedOn.Value)
            {
                lockOnInput = false;
                PlayerCamera.instance.ClearLockOnTargets();
                player.playerNetworkManager.isLockedOn.Value = false;
                // are we already locked on?
                // is our current target dead?
                return;

            }

            if (lockOnInput && !player.playerNetworkManager.isLockedOn.Value)
            {
                lockOnInput = false;
                // if using ranged weapon, return

                // enable lock on
                PlayerCamera.instance.HandleLocatingLockOnTargets();

                if (PlayerCamera.instance.nearestTarget != null)
                {
                    player.playerCombatManager.SetTarget(PlayerCamera.instance.nearestTarget);
                    player.playerNetworkManager.isLockedOn.Value = true;
                }
            }
        }

        // ACTIONS

        private void HandleDodgeInput()
        {
            if (dodgeInput)
            {
                dodgeInput = false;

                // return if ui is open
                // perform a dodge

                player.playerLocomotionManager.AttemptToPerformDodge();
            }
        }

        private void HandleSprintInput()
        {
            if (sprintInput)
            {
                player.playerLocomotionManager.HandleSprinting();
            }
            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }
        }

        private void HandleJumpInput()
        {
            if (jumpInput)
            {
                jumpInput = false;

                // if window ui open, return

                // attempt to perform a jump
                player.playerLocomotionManager.AttemptToPerformJump();
            }
        }

        private void HandleRBInput()
        {
            if (rb_input)
            {
                rb_input = false;

                // if window ui open, return

                // attempt to perform a rb action
                player.playerNetworkManager.SetCharacterActionHand(true);

                // todo: if we are twohanding run two hand action

                player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oh_rbAction, player.playerInventoryManager.currentRightHandWeapon);
            }
        }
    }

}
