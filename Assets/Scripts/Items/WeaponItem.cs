using UnityEngine;

namespace DKC
{
    public class WeaponItem : Item
    {
        // animator controller override

        [Header("Weapon Model")]
        public GameObject weaponModel;

        [Header("Weapon Requirements")]
        public int strengthReq = 0;
        public int dexterityReq = 0;
        public int intelligenceReq = 0;
        public int faithReq = 0;

        [Header("Weapon Base Damage")]
        public int physicalDamage = 0;
        public int magicDamage = 0;
        public int fireDamage = 0;
        public int lightningDamage = 0;
        public int holyDamage = 0;

        // weapon guard absorptions

        [Header("Weapon Base Poise Damage")]
        public float poiseDamage = 10;
        // offensive poise damage modifier

        // weapon modifiers

        [Header("Attack Modifiers")]
        public float lightAttack01Modifier = 1.1f;
        public float heavyAttack01Modifier = 1.4f;
        public float chargedAttack01Modifier = 2.0f;

        [Header("Stamina Cost Modifiers")]
        public int baseStaminaCost = 20;
        public float lightAttackStaminaCostModifier = 0.9f;
        // running attack stamina cost modifier
        // heavy attack stamina cost modifier

        [Header("Actions")]
        public WeaponItemAction oh_rbAction; // one handed right bumper action
        public WeaponItemAction oh_rtAction; // one handed right trigger action

        // ash of war

        // blocking sounds
    }
}