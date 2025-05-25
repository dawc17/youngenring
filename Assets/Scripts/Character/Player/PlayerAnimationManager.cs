using System;
using UnityEngine;

namespace DKC
{
    public class PlayerAnimationManager : CharacterAnimatorManager
    {
        PlayerManager player;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        private void OnAnimatorMove()
        {
            if (player.IsOwner && player.applyRootMotion)
            {
                Vector3 velocity = player.animator.deltaPosition;
                player.characterController.Move(velocity);
                player.transform.rotation *= player.animator.deltaRotation;
            }
        }

        public void PlayRollAnimation()
        {
            // Set root motion to true before starting animation
            player.applyRootMotion = true;
            PlayTargetActionAnimation("Roll_Forward_01", true, true, false, false);
            player.playerLocomotionManager.isRolling = true;
        }

        public void PlayBackstepAnimation()
        {
            player.applyRootMotion = true;
            PlayTargetActionAnimation("Backstep_01", true, true, false, false);
        }

        // animation event calls
        public override void EnableCanDoCombo()
        {
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                player.playerCombatManager.canComboWithMainHandWeapon = true;
            }
            else
            {
                // enable offhand combo
            }
        }

        public override void DisableCanDoCombo()
        {
            player.playerCombatManager.canComboWithMainHandWeapon = false;
            // disable offhand combo
        }
    }
}

