using UnityEngine;
using UnityEngine.UI;

namespace DKC
{
    public class PlayerUIHudManager : MonoBehaviour
    {
        [Header("Stat Bars")]
        [SerializeField] private UiStatBar staminaBar;
        [SerializeField] private UiStatBar healthBar;

        [Header("Quick Slots")]
        [SerializeField] Image rightWeaponQuickSlotIcon;
        [SerializeField] Image leftWeaponQuickSlotIcon;

        public void RefreshHUD()
        {
            healthBar.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(true);

            staminaBar.gameObject.SetActive(false);
            staminaBar.gameObject.SetActive(true);
        }

        public void SetNewHealthValue(int oldValue, int newValue)
        {
            if (healthBar != null)
            {
                healthBar.SetStat(newValue);
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

        public void SetRightWeaponQuickSlotIcon(int weaponID)
        {
            WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(weaponID);
            if (weapon == null)
            {
                Debug.LogError($"Weapon with ID {weaponID} not found in WorldItemDatabase (NULL).", this);
                rightWeaponQuickSlotIcon.enabled = false;
                rightWeaponQuickSlotIcon.sprite = null;
                return;
            }

            if (weapon.itemIcon == null)
            {
                Debug.LogError($"Weapon {weapon.name} does not have an icon assigned.", this);
                rightWeaponQuickSlotIcon.enabled = false;
                rightWeaponQuickSlotIcon.sprite = null;
                return;
            }

            // this is where you would check to see if you meet item requirements (todo)

            rightWeaponQuickSlotIcon.sprite = weapon.itemIcon;
            rightWeaponQuickSlotIcon.enabled = true;
        }

        public void SetLeftWeaponQuickSlotIcon(int weaponID)
        {
            WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(weaponID);
            if (weapon == null)
            {
                Debug.LogError($"Weapon with ID {weaponID} not found in WorldItemDatabase (NULL).", this);
                leftWeaponQuickSlotIcon.enabled = false;
                leftWeaponQuickSlotIcon.sprite = null;
                return;
            }

            if (weapon.itemIcon == null)
            {
                Debug.LogError($"Weapon {weapon.name} does not have an icon assigned.", this);
                leftWeaponQuickSlotIcon.enabled = false;
                leftWeaponQuickSlotIcon.sprite = null;
                return;
            }

            // this is where you would check to see if you meet item requirements (todo)

            leftWeaponQuickSlotIcon.sprite = weapon.itemIcon;
            leftWeaponQuickSlotIcon.enabled = true;
        }
    }
}

