using TMPro;
using UnityEngine;

namespace DKC
{
    public class UI_StatBossHP : UiStatBar
    {
        [SerializeField] AIBossCharacterManager bossCharacter;
        public void EnableBossHPBar(AIBossCharacterManager boss)
        {
            bossCharacter = boss;
            bossCharacter.aiCharacterNetworkManager.currentHealth.OnValueChanged += OnBossHpChanged;
            SetMaxStat(bossCharacter.characterNetworkManager.maxHealth.Value);
            SetStat(bossCharacter.aiCharacterNetworkManager.currentHealth.Value);
            GetComponentInChildren<TextMeshProUGUI>().text = bossCharacter.characterName;
        }

        private void OnDestroy()
        {
            bossCharacter.aiCharacterNetworkManager.currentHealth.OnValueChanged -= OnBossHpChanged;
        }

        private void OnBossHpChanged(int oldValue, int newValue)
        {
            SetStat(newValue);

            if (newValue <= 0)
            {
                RemoveHPBar(2f); // Delay before removing the HP bar
            }
        }

        public void RemoveHPBar(float time)
        {
            Destroy(gameObject, time);
        }
    }
}
