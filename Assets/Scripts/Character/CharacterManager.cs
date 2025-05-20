using System;
using System.Collections;
using UnityEngine;
using Unity.Netcode;

namespace DKC
{
    public class CharacterManager : NetworkBehaviour
    {
        [Header("Status")] 
        public NetworkVariable<bool> isDead = new NetworkVariable<bool>(false,
            NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;
        [HideInInspector] public CharacterNetworkManager characterNetworkManager;
        [HideInInspector] public CharacterEffectsManager characterEffectsManager;
        [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
        
        [Header("Flags")] 
        public bool isPerformingAction = false;
        public bool isJumping = false;
        public bool isGrounded = true;
        public bool applyRootMotion = false;
        public bool canRotate = true;
        public bool canMove = true;


        
        protected virtual void Awake()
        {
            DontDestroyOnLoad(this); 
            
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
            characterEffectsManager = GetComponent<CharacterEffectsManager>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (!IsOwner)
            {
                if (characterController != null)
                {
                    characterController.enabled = false;
                }
            }
        }

        protected virtual void Update()
        {
            animator.SetBool("isGrounded", isGrounded);
            // if character owned by us, assign position to our transform
            if (IsOwner)
            {
                characterNetworkManager.networkPosition.Value = transform.position;
                characterNetworkManager.networkRotation.Value = transform.rotation;
            }
            // if controlled from elsewhere, update position locally based on network position
            else
            {
                // position
                transform.position = Vector3.SmoothDamp(transform.position,
                    characterNetworkManager.networkPosition.Value, 
                    ref characterNetworkManager.networkPositionVelocity,
                    characterNetworkManager.networkPositionSmoothTime);
                
                // rotation
                transform.rotation = Quaternion.Slerp
                    (transform.rotation, 
                        characterNetworkManager.networkRotation.Value, 
                        characterNetworkManager.networkRotationSmoothTime);
            }
        }

        protected virtual void LateUpdate()
        {
            
        }

        public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner)
            {
                characterNetworkManager.currentHealth.Value = 0;
                isDead.Value = true;
                
                // reset any flags here that need to be reset
                
                // if we are not grounded, play an aerial death animation

                if (!manuallySelectDeathAnimation)
                {
                    characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
                }
            }
            // play death sfx

            yield return new WaitForSeconds(5);
            
            // award players with runes
            
            // disable character
        }

        public virtual void ReviveCharacter()
        {
            
        }
    }
}
