using System;
using UnityEngine;
using Unity.Netcode;

namespace DKC
{
    public class CharacterManager : NetworkBehaviour
    {
        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;
        [HideInInspector] public CharacterNetworkManager characterNetworkManager;

        [Header("Flags")] 
        public bool isPerformingAction = false;
        public bool applyRootMotion = false;
        public bool canRotate = true;
        public bool canMove = true;


        
        protected virtual void Awake()
        {
            DontDestroyOnLoad(this); 
            
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
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

        
    }
}
