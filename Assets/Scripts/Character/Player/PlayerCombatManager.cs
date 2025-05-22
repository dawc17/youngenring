using UnityEngine;
using Unity.Netcode;

namespace DKC
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        PlayerManager player;

        public WeaponItem currentWeaponBeingUsed;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
        {
            if (player.IsOwner)
            {
                weaponAction.AttemptToPerformAction(player, weaponPerformingAction);
            }
            // perform action on other clinents (notify server)
            player.playerNetworkManager.NotifyServerOfWeaponActionServerRpc(NetworkManager.Singleton.LocalClientId, weaponAction.actionID, weaponPerformingAction.itemID);

        }
    }
}