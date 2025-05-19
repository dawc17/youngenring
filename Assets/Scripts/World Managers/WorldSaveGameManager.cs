using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DKC
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager Instance;

        [SerializeField] PlayerManager player;

        [Header("Save/Load")] 
        [SerializeField] private bool saveGame;
        [SerializeField] private bool loadGame;
        
        [Header("World Scene Index")]
        [SerializeField] int worldSceneIndex = 1;

        [Header("Save Data Writer")] 
        private WriteSaveData saveFileDataWriter;

        [Header("Current Character Data")] 
        public CharacterSlot currentSlot;
        public CharacterSaveData currentCharacterData;
        private string fileName;
        
        [Header("Character Slots")] 
        public CharacterSaveData characterSlot01;
        public CharacterSaveData characterSlot02;
        public CharacterSaveData characterSlot03;
        public CharacterSaveData characterSlot04;
        public CharacterSaveData characterSlot05;
        
        private void Awake()
        {
            // Check if an instance of this class already exists
            if (Instance is null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (saveGame)
            {
                saveGame = false;
                SaveGame();
            }

            if (loadGame)
            {
                loadGame = false;
                LoadGame();
            }
        }

        private void DecideCharacterFileNameBasedOnCharacterSlotUsed()
        {
            switch (currentSlot)
            {
                case CharacterSlot.CharacterSlot_01:
                    fileName = "CharacterSlot_01";
                    break;
                case CharacterSlot.CharacterSlot_02:
                    fileName = "CharacterSlot_02";
                    break;
                case CharacterSlot.CharacterSlot_03:
                    fileName = "CharacterSlot_03";
                    break;
                case CharacterSlot.CharacterSlot_04:
                    fileName = "CharacterSlot_04";
                    break;
                case CharacterSlot.CharacterSlot_05:
                    fileName = "CharacterSlot_05";
                    break;
                default:
                    break;
            }
        }

        public void CreateNewGame()
        {
            // create a new file, with a file name depending on which character slot youre using
            DecideCharacterFileNameBasedOnCharacterSlotUsed();
            currentCharacterData = new CharacterSaveData();
        }

        public void LoadGame()
        {
            // load a previous file, with a file name depending on which slot we are using
            DecideCharacterFileNameBasedOnCharacterSlotUsed();

            saveFileDataWriter = new WriteSaveData();
            // generally works on multiple machine types
            saveFileDataWriter.saveDataPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = fileName;
            currentCharacterData = saveFileDataWriter.LoadSaveFile();

            StartCoroutine(LoadWorldScene());
        }

        public void SaveGame()
        {
            // save the current file under a file name depending on slot used
            DecideCharacterFileNameBasedOnCharacterSlotUsed();

            saveFileDataWriter = new WriteSaveData();
            saveFileDataWriter.saveDataPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = fileName;
            
            // pass the players info from game to their save file
            player.SaveGameDateToCurrentCharacterData(ref currentCharacterData);
            
            // write that info onto a json file saved to this machine
            saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
        }
        
        public IEnumerator LoadWorldScene()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
            yield return null;
        }

        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }
    }
}
