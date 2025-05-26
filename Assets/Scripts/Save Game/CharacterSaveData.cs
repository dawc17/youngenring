using UnityEngine;

namespace DKC
{
    [System.Serializable]
    // since we reference this data for every save, this script is not a monobehaviour and is serializeble
    public class CharacterSaveData
    {
        [Header("Scene Index")]
        public int sceneIndex = 1;

        [Header("Character Name")]
        public string characterName = "Character";

        [Header("Time Played")]
        public float secondsPlayed;

        // why not vector3?
        // because we can only serialize basic data types (float int string bool)
        [Header("World Coordinates")]
        public float xPos;
        public float yPos;
        public float zPos;

        [Header("Resources")]
        public int currentHealth;
        public float currentStamina;

        [Header("Stats")]
        public int vitality;
        public int endurance;

        [Header("Bosses")]
        public SerializableDictionary<int, bool> bossesAwakened;
        public SerializableDictionary<int, bool> bossesDefeated;

        public CharacterSaveData()
        {
            bossesAwakened = new SerializableDictionary<int, bool>();
            bossesDefeated = new SerializableDictionary<int, bool>();
        }
    }
}