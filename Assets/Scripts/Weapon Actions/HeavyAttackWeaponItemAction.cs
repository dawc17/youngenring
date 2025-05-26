using UnityEngine;

namespace DKC
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Heavy Attack Action")]
    public class HeavyAttackWeaponItemAction : WeaponItemAction
    {
        [SerializeField] string heavyAttack_01 = "Main_Heavy_Attack_01"; // main = main hand (right)
        [SerializeField] string heavyAttack_02 = "Main_Heavy_Attack_02";
        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

            if (!playerPerformingAction.IsOwner)
                return;

            // check for stops

            if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
                return;

            if (!playerPerformingAction.characterLocomotionManager.isGrounded)
                return;

            PerformHeavyAttack(playerPerformingAction, weaponPerformingAction);
        }

        private void PerformHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            // if we are attacking and we can combo
            if (playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon && playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon = false;

                // perform an attack based on the previous attack we just played
                if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == heavyAttack_01)
                {
                    playerPerformingAction.playerAnimationManager.PlayTargetAttackActionAnimation(AttackType.HeavyAttack02, heavyAttack_02, true, true);
                }
                else
                {
                    playerPerformingAction.playerAnimationManager.PlayTargetAttackActionAnimation(AttackType.HeavyAttack01, heavyAttack_01, true, true);
                }
            }
            // otherwise just perform a regular attack
            else if (!playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerAnimationManager.PlayTargetAttackActionAnimation(AttackType.HeavyAttack01, heavyAttack_01, true, true);
            }
        }
    }
}