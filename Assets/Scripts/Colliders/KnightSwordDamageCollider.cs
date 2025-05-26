using UnityEngine;

namespace DKC
{
    public class KnightSwordDamageCollider : DamageCollider
    {
        [SerializeField] public AICharacterManager knightBoss;

        protected override void Awake()
        {
            base.Awake();

            damageCollider = GetComponent<Collider>();
            knightBoss = GetComponentInParent<AICharacterManager>();
        }

        protected override void DamageTarget(CharacterManager damageTarget)
        {
            // we dont want to damage the same target more than once in a single attack
            // so we add them to a list that checks before applying damage

            if (charactersDamaged.Contains(damageTarget))
                return;

            charactersDamaged.Add(damageTarget);

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Instance.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.contactPoint = contactPoint;
            damageEffect.angleHitFrom = Vector3.SignedAngle(knightBoss.transform.forward, damageTarget.transform.forward, Vector3.up);

            if (damageTarget.IsOwner)
            {
                damageTarget.characterNetworkManager.NotifyServerOfDamageServerRpc(
                    damageTarget.NetworkObjectId,
                    knightBoss.NetworkObjectId,
                    damageEffect.physicalDamage,
                    damageEffect.magicDamage,
                    damageEffect.fireDamage,
                    damageEffect.lightningDamage,
                    damageEffect.holyDamage,
                    damageEffect.poiseDamage,
                    damageEffect.angleHitFrom,
                    contactPoint.x,
                    contactPoint.y,
                    contactPoint.z);
            }
        }
    }
}