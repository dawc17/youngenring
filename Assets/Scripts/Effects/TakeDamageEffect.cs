using DKC;
using UnityEngine;

namespace DKC
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]
    public class TakeDamageEffect : InstantCharacterEffect
    {
        [Header("Character Causing Damage")] 
        public CharacterManager characterCausingDamage;

        [Header("Damage")] 
        public float physicalDamage = 0; // in future will be split into standart strike slash and pierce
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;
        
        [Header("Final Damage")]
        private int finalDamageDealt = 0; // the damage the character takes after all calculations have been made

        [Header("Poise")] 
        public float poiseDamage = 0;
        public bool poiseIsBroken = false; // poise broken = stunned
        
        // todo
        // buildups
        // build up effect amounts

        [Header("Animation")] 
        public bool playDamageAnimation = true;
        public bool manuallySelectDamageAnimation = false;
        public string damageAnimation;

        [Header("SFX")] 
        public bool willPlayDamageSFX = true;
        public AudioClip elementalDamageSFX; // on top of regular sfx

        [Header("Direction Damage Came From")] 
        public float angleHitFrom; // used to determine what damage animation to play based on angle the character was hit from
        public Vector3 contactPoint; // used to determine where the blood fx instantiate
        
        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);
            
            // dont process anything if character is dead
            if (character.isDead.Value)
                return;
            
            // check for iFrames
            
            CalculateDamage(character);
            // check which direction damage came from
            // play a damage animation
            // check for build ups (posion, bleed etc)
            // play damage sfx and vfx 
            
            // if character is ai, check for new target if character causing damage is present
        }

        private void CalculateDamage(CharacterManager character)
        {
            if (!character.IsOwner)
                return;
            
            if (characterCausingDamage != null)
            {
                // check for damage modifiers and modify base damage
                // physical *= physicalModifier type shit
            }
            
            // check character for falt defense and subtract from the damage
            
            // check character for armor absorption and subtract the percentage from the damage
            
            // add all damage types together and apply final damage
            finalDamageDealt =
                Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

            if (finalDamageDealt <= 0)
            {
                finalDamageDealt = 1;
            }

            Debug.Log("Final damage given: " + finalDamageDealt);
            character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;
            
            // calculate poise damage to determine if they will be stunned
        }
    }
}