using Character.Player.UI;
using UnityEngine;

namespace DKC
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerAnimationManager playerAnimationManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerNetworkManager playerNetworkManager;
        [HideInInspector] public PlayerStatsManager playerStatsManager;

        // ReSharper disable once RedundantOverriddenMember
        protected override void Awake()
        {
            base.Awake();
            
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimationManager = GetComponent<PlayerAnimationManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
        }

        protected override void Update()
        {
            base.Update();
            
            // if we dont own character, do not run anything
            if(!IsOwner)
                return;
            
            playerLocomotionManager.HandleAllMovement();
            
            // regen stamina
            playerStatsManager.RegenerateStamina();
        }

        protected override void LateUpdate()
        {
            // if we dont own character, do not run anything
            if(!IsOwner)
                return;
            
            base.LateUpdate();
            
            PlayerCamera.instance.HandleAllCameraActions();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsOwner)
            {
                PlayerCamera.instance.player = this;
                PlayerInputManager.instance.player = this;
                WorldSaveGameManager.Instance.player = this;

                playerNetworkManager.currentStamina.OnValueChanged +=
                    PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue;
                playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenTimer;
                
                // this will be moved when saving and loading is added
                playerNetworkManager.maxStamina.Value =
                    playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
                playerNetworkManager.currentStamina.Value =
                    playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
                PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);
               
            }
        }

        public void SaveGameDateToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
            currentCharacterData.xPos = transform.position.x;
            currentCharacterData.yPos = transform.position.y;
            currentCharacterData.zPos = transform.position.z;
        }

        public void LoadGameFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            playerNetworkManager.characterName.Value = currentCharacterData.characterName;
            Vector3 myPos = new Vector3(currentCharacterData.xPos, currentCharacterData.yPos, currentCharacterData.zPos);
            transform.position = myPos;
        }
    }
}