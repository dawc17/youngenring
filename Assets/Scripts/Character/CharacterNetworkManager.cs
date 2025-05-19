using System;
using UnityEngine;
using Unity.Netcode;

namespace DKC
{
    public class CharacterNetworkManager : NetworkBehaviour
    {
        private CharacterManager character;
        
        [Header("Position")] 
        public NetworkVariable<Vector3> networkPosition =
            new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);
        public NetworkVariable<Quaternion> networkRotation =
            new NetworkVariable<Quaternion>(Quaternion.identity, NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);

        public Vector3 networkPositionVelocity;
        public float networkPositionSmoothTime = 0.1f;
        public float networkRotationSmoothTime = 0.1f;
        
        [Header("Animator")]
        public NetworkVariable<float> animatorHorizontalParameter = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> animatorVerticalParameter = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> networkMoveAmount = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Flags")]
        public NetworkVariable<bool> isSprinting = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        protected void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        [ServerRpc]
        public void NotifyServerOfActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            if (IsServer)
            {
                PlayActionAnimationForAllClientsClientRpc(clientID, animationID, applyRootMotion);
            }
        }
        
        [ClientRpc]
        public void PlayActionAnimationForAllClientsClientRpc(ulong clientId, string animationID, bool applyRootMotion)
        {
            if (clientId != NetworkManager.Singleton.LocalClientId)
            {
                PerformActionAnimationFromServer(animationID, applyRootMotion);
            }
        }

        private void PerformActionAnimationFromServer(string animationID, bool applyRootMotion)
        {
            character.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(animationID, 0.2f);
        }
    }
}