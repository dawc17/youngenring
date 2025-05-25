using UnityEngine;

namespace DKC
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack Action")]
    public class LightAttackWeaponItemAction : WeaponItemAction
    {
        [SerializeField] string lightAttack_01 = "Main_Light_Attack_01"; // main = main hand (right)
        [SerializeField] string lightAttack_02 = "Main_Light_Attack_02";
        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

            if (!playerPerformingAction.IsOwner)
                return;

            // check for stops

            if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
                return;

            if (!playerPerformingAction.isGrounded)
                return;

            PerformLightAttack(playerPerformingAction, weaponPerformingAction);
        }

        private void PerformLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {

            // if we are attacking and we can combo
            if (playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon && playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon = false;

                // perform an attack based on the previous attack we just played
                if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == lightAttack_01)
                {
                    playerPerformingAction.playerAnimationManager.PlayTargetAttackActionAnimation(AttackType.LightAttack02, lightAttack_02, true, true);
                }
                else
                {
                    playerPerformingAction.playerAnimationManager.PlayTargetAttackActionAnimation(AttackType.LightAttack01, lightAttack_01, true, true);
                }
            }
            // otherwise just perform a regular attack
            else if (!playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerAnimationManager.PlayTargetAttackActionAnimation(AttackType.LightAttack01, lightAttack_01, true, true);
            }
        }
    }
}