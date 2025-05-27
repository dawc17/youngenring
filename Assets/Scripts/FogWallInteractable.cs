using UnityEngine;
using Unity.Netcode;

namespace DKC
{
    public class FogWallInteractable : NetworkBehaviour
    {
        [Header("Fog")]
        [SerializeField] GameObject[] fogGameOjects;

        [SerializeField] Collider fogWallCollider;

        [Header("ID")]
        public int fogWallID;

        [Header("Active")]
        public NetworkVariable<bool> isActive = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            OnIsActiveChanged(false, isActive.Value);
            isActive.OnValueChanged += OnIsActiveChanged;
            WorldObjectManager.instance.AddFogWallToList(this);
            fogWallCollider.enabled = false;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            isActive.OnValueChanged -= OnIsActiveChanged;
            WorldObjectManager.instance.RemoveFogWallFromList(this);
        }

        private void OnIsActiveChanged(bool previousValue, bool newValue)
        {
            if (isActive.Value)
            {
                foreach (var fogObject in fogGameOjects)
                {
                    fogObject.SetActive(true);
                    fogWallCollider.enabled = true; // Enable the fog wall interactable
                }
            }
            else
            {
                foreach (var fogObject in fogGameOjects)
                {
                    fogObject.SetActive(false);
                    fogWallCollider.enabled = false; // Disable the fog wall interactable
                }
            }
        }
    }
}
