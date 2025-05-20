using System;
using UnityEngine;

namespace DKC
{
    public class CharacterStatsManager : MonoBehaviour

    {
        private CharacterManager character;
        
        [Header("Stamina Regeneration")] 
        private float staminaRegenerationTimer = 0;
        private float staminaTickTimer = 0;
        [SerializeField] float staminaRegenerationDelay = 2;
        [SerializeField] int staminaRegenerationAmount = 2;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Start()
        {
        }

        public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
        {
            float stamina = 0;

            stamina = endurance * 10;
            return Mathf.RoundToInt(stamina);
        }
        
        public int CalculateHealthBasedOnVitalityLevel(int vitality)
        {
            float health = 0;

            health = vitality * 15;
            return Mathf.RoundToInt(health);
        }
        
        public virtual void RegenerateStamina()
        {
            if (!character.IsOwner)
                return;
            
            if (character.characterNetworkManager.isSprinting.Value)
                return;
            
            if (character.isPerformingAction)
                return;

            staminaRegenerationTimer += Time.deltaTime;

            if (staminaRegenerationTimer >= staminaRegenerationDelay)
            {
                if (character.characterNetworkManager.currentStamina.Value < character.characterNetworkManager.maxStamina.Value)
                {
                    staminaTickTimer += Time.deltaTime;

                    if (staminaTickTimer >= 0.1)
                    {
                        staminaTickTimer = 0;
                        character.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
                    }
                }
            }
        }

        public virtual void ResetStaminaRegenTimer(float oldValue, float newValue)
        {
            // only reset the regen if action used stamina
            // dont reset if already regening
            if (newValue < oldValue)
            {
                staminaRegenerationTimer = 0;
            }
        }
    }
}