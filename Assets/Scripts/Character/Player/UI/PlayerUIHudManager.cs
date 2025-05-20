using UnityEngine;

namespace DKC
{
    public class PlayerUIHudManager : MonoBehaviour
    {
        [SerializeField] private UiStatBar staminaBar;
        [SerializeField] private UiStatBar healthBar;

        public void RefreshHUD()
        {
            healthBar.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(true);
            
            staminaBar.gameObject.SetActive(false);
            staminaBar.gameObject.SetActive(true);
        }
        
        public void SetNewHealthValue(float oldValue, float newValue)
        {
            if (healthBar != null)
            {
                healthBar.SetStat(Mathf.RoundToInt(newValue));
            }
            else
            {
                Debug.LogError("HealthBar is not assigned in PlayerUIHudManager.", this);
            }
        }
        
        public void SetMaxHealthValue(int maxHealth)
        {
            if (healthBar != null)
            {
                healthBar.SetMaxStat(maxHealth);
            }
            else
            {
                Debug.LogError("HealthBar is not assigned in PlayerUIHudManager.", this);
            }
        }
        
        public void SetNewStaminaValue(float oldValue, float newValue)
        {
            if (staminaBar != null)
            {
                staminaBar.SetStat(Mathf.RoundToInt(newValue));
            }
            else
            {
                Debug.LogError("StaminaBar is not assigned in PlayerUIHudManager.", this);
            }
        }

        public void SetMaxStaminaValue(int maxStamina)
        {
            if (staminaBar != null)
            {
                staminaBar.SetMaxStat(maxStamina);
            }
            else
            {
                Debug.LogError("StaminaBar is not assigned in PlayerUIHudManager.", this);
            }
        }
    }
}

