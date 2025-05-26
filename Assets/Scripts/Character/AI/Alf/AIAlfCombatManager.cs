using System.Collections.Generic;
using UnityEngine;

namespace DKC
{
    public class AIAlfCombatManager : AICharacterCombatManager
    {
        [Header("Damage Colliders")]
        [SerializeField] AlfClubDamageCollider clubDamageCollider;
        [SerializeField] Transform durksStompingFoot;
        [SerializeField] float durksStompRadius = 0.1f;

        [Header("Damage")]
        [SerializeField] int baseDamage = 25;
        [SerializeField] float attack01DamageModifier = 1.0f;
        [SerializeField] float attack02DamageModifier = 1.4f;
        [SerializeField] float stompDamage = 25f;

        public void SetAttack01Damage()
        {
            clubDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
        }

        public void SetAttack02Damage()
        {
            clubDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
        }

        public void OpenClubDamageCollider()
        {
            aiCharacter.characterSFXManager.PlayAttackGrunt();
            clubDamageCollider.EnableDamageCollider();
        }

        public void CloseClubDamageCollider()
        {
            clubDamageCollider.DisableDamageCollider();
        }

        public void ActivateDurkStomp()
        {
            Collider[] colliders = Physics.OverlapSphere(durksStompingFoot.position, durksStompRadius, WorldUtilityManager.instance.GetCharacterLayers());

            foreach (var collider in colliders)
            {
                CharacterManager character = collider.GetComponent<CharacterManager>();

                List<CharacterManager> charactersDamaged = new List<CharacterManager>();

                if (character != null)
                {
                    if (charactersDamaged.Contains(character))
                    {
                        continue; // Skip if already damaged
                    }

                    charactersDamaged.Add(character); // Add to damaged list

                    if (character.IsOwner)
                    {
                        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Instance.takeDamageEffect);
                        damageEffect.physicalDamage = stompDamage;
                        damageEffect.poiseDamage = stompDamage;

                        character.characterEffectsManager.ProcessInstantEffect(damageEffect);
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(durksStompingFoot.position, durksStompRadius);
        }

        public override void PivotTowardsTarget(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isPerformingAction)
            {
                return; // Do not pivot if performing an action
            }

            if (viewableAngle >= 61 && viewableAngle <= 110)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_90", true);
            }
            else if (viewableAngle <= -61 && viewableAngle >= -110)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left_90", true);
            }
            else if (viewableAngle >= 146 && viewableAngle <= 180)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_180", true);
            }
            else if (viewableAngle <= -146 && viewableAngle >= -180)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left_180", true);
            }
        }
    }
}
