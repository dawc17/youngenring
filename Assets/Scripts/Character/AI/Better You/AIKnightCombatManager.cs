using UnityEngine;

namespace DKC
{
    public class AIKnightCombatManager : AICharacterCombatManager
    {
        [Header("Damage Colliders")]
        [SerializeField] KnightSwordDamageCollider swordDamageCollider;

        [Header("Damage")]
        [SerializeField] int baseDamage = 30;
        [SerializeField] float attack01DamageModifier = 1.4f;
        [SerializeField] float attack02DamageModifier = 1.0f;

        public void SetAttack01Damage()
        {
            swordDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
        }

        public void SetAttack02Damage()
        {
            swordDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
        }

        public void OpenRightHandDamageCollider()
        {
            aiCharacter.characterSFXManager.PlayAttackGrunt();
            swordDamageCollider.EnableDamageCollider();
        }

        public void CloseRightHandDamageCollider()
        {
            swordDamageCollider.DisableDamageCollider();
        }
    }
}
