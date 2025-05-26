using UnityEngine;

namespace DKC
{
    public class AIBiterCombatManager : AICharacterCombatManager
    {
        [Header("Damage Colliders")]
        [SerializeField] BiterHandDamageCollider rightHandDamageCollider;
        [SerializeField] BiterHandDamageCollider leftHandDamageCollider;

        [Header("Damage")]
        [SerializeField] int baseDamage = 25;
        [SerializeField] float attack01DamageModifier = 1.4f;
        [SerializeField] float attack02DamageModifier = 1.0f;

        public void SetAttack01Damage()
        {
            rightHandDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
            leftHandDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
        }

        public void SetAttack02Damage()
        {
            rightHandDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
            leftHandDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
        }

        public void OpenRightHandDamageCollider()
        {
            aiCharacter.characterSFXManager.PlayAttackGrunt();
            rightHandDamageCollider.EnableDamageCollider();
        }

        public void CloseRightHandDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
        }

        public void OpenLeftHandDamageCollider()
        {
            aiCharacter.characterSFXManager.PlayAttackGrunt();
            leftHandDamageCollider.EnableDamageCollider();
        }

        public void CloseLeftHandDamageCollider()
        {
            leftHandDamageCollider.DisableDamageCollider();
        }
    }
}