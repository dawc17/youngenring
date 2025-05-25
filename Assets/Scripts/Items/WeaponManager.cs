using UnityEngine;

namespace DKC
{
    public class WeaponManager : MonoBehaviour
    {
        public MeleeWeaponDamageCollider meleeDamageCollider;

        private void Awake()
        {
            meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
        }

        public void SetWeaponDamage(CharacterManager characterWieldingWeapon, WeaponItem weapon)
        {
            meleeDamageCollider.characterCausingDamage = characterWieldingWeapon;
            meleeDamageCollider.physicalDamage = weapon.physicalDamage;
            meleeDamageCollider.magicDamage = weapon.magicDamage;
            meleeDamageCollider.fireDamage = weapon.fireDamage;
            meleeDamageCollider.lightningDamage = weapon.lightningDamage;
            meleeDamageCollider.holyDamage = weapon.holyDamage;

            meleeDamageCollider.lightAttack01Modifier = weapon.lightAttack01Modifier;
            meleeDamageCollider.lightAttack02Modifier = weapon.lightAttack02Modifier;
            meleeDamageCollider.heavyAttack01Modifier = weapon.heavyAttack01Modifier;
            meleeDamageCollider.heavyAttack02Modifier = weapon.heavyAttack02Modifier;
            meleeDamageCollider.chargedAttack01Modifier = weapon.chargedAttack01Modifier;
            meleeDamageCollider.chargedAttack02Modifier = weapon.chargedAttack02Modifier;
        }
    }
}