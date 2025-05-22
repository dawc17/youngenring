using UnityEngine;

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
            weaponAction.AttemptToPerformAction(player, weaponPerformingAction);

            // perform action on other clinents (notify server)

        }
    }
}