using UnityEngine;

namespace DKC
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Stamina Damage")]
    public class TakeStaminaDamageEffect : InstantCharacterEffect
    {
        public float staminaDamageAmount;
        public override void ProcessEffect(CharacterManager character)
        {
            CalculateStaminaDamage(character);
        }

        private void CalculateStaminaDamage(CharacterManager character)
        {
            // compare the base stamina against other player effects/modifiers
            // change the value before subtracting/adding it
            // play sfx or vfx during effect

            if (character.IsOwner)
            {
                Debug.Log("character is taking: " + staminaDamageAmount + " stamina damage.");
                character.characterNetworkManager.currentStamina.Value -= staminaDamageAmount;
            }
        }
    }
}