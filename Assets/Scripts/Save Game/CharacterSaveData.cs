using UnityEngine;

namespace DKC
{
    [System.Serializable]
    // since we reference this data for every save, this script is not a monobehaviour and is serializeble
    public class CharacterSaveData
    {
        [Header("Character Name")]
        public string characterName;

        [Header("Time Played")] 
        public float secondsPlayed;

        // why not vector3?
        // because we can only serialize basic data types (float int string bool)
        [Header("World Coordinates")] 
        public float xPos;
        public float yPos;
        public float zPos;
    }
}