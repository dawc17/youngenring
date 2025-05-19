using UnityEngine;

namespace DKC
{
    public class PlayerUIHudManager : MonoBehaviour
    {
        [SerializeField] private UiStatBar staminaBar;

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

