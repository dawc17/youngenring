using UnityEngine;

namespace DKC
{
    public class PlayerUIHudManager : MonoBehaviour
    {
        [SerializeField] private UiStatBar staminaBar;

        public void SetNewStaminaValue(float oldValue, float newValue)
        {
            staminaBar.SetStat(Mathf.RoundToInt(newValue));
        }

        public void SetMaxStaminaValue(int maxStamina)
        {
            staminaBar.SetMaxStat(maxStamina);
        }
    }
}