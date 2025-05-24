using DKC;
using UnityEngine;
using UnityEngine.Rendering;

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
            PlayDirectionalBasedDamageAnimation(character);
            // check for build ups (posion, bleed etc)
            PlayDamageSFX(character);
            PlayDamageVFX(character);

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

        private void PlayDamageVFX(CharacterManager character)
        {
            // if fire damaqge, play fire particles etc

            character.characterEffectsManager.PlayBloodSplatterVFX(contactPoint);
        }

        private void PlayDamageSFX(CharacterManager character)
        {
            AudioClip physicalDamageSFX = WorldSFXManager.instance.ChooseRandomSFXFromArray(WorldSFXManager.instance.physicalDamageSFX);

            character.characterSFXManager.PlaySFX(physicalDamageSFX);
        }

        private void PlayDirectionalBasedDamageAnimation(CharacterManager character)
        {
            if (!character.IsOwner)
                return;

            if (character.isDead.Value)
                return;
            // calculate if poise is broken
            poiseIsBroken = true;

            if (angleHitFrom >= 145 && angleHitFrom <= 180)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forwardMediumDamageAnimations);
            }
            else if (angleHitFrom <= -145 && angleHitFrom >= -180)
            {
                // hit from the front
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forwardMediumDamageAnimations);
            }
            else if (angleHitFrom >= -45 && angleHitFrom <= 45)
            {
                // hit from the back
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.backwardMediumDamageAnimations);
            }
            else if (angleHitFrom >= -145 && angleHitFrom <= -45)
            {
                // hit from the left
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.leftMediumDamageAnimations);
            }
            else if (angleHitFrom >= 45 && angleHitFrom <= 145)
            {
                // hit from the right
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.rightMediumDamageAnimations);
            }

            // if poise is broken play a stagger anim
            if (poiseIsBroken)
            {
                character.characterAnimatorManager.lastDamageAnimationPlayed = damageAnimation;
                character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, true);
            }
        }
    }
}