using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DKC
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager Instance;

        [SerializeField] public PlayerManager player;

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
            LoadAllCharacterSlots();
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

        public string DecideCharacterFileNameBasedOnCharacterSlotUsed(CharacterSlot characterSlot)
        {
            string fileName = "";
            switch (characterSlot)
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

            return fileName;
        }

        public void ChangeCharacterData()
        {
            currentCharacterData = saveFileDataWriter.LoadSaveFile();
        }

        public void CreateNewGame()
        {
            saveFileDataWriter = new WriteSaveData();
            saveFileDataWriter.saveDataPath = Application.persistentDataPath;

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotUsed(CharacterSlot.CharacterSlot_01);
            if (!saveFileDataWriter.CheckIfFileExists())
            {
                currentSlot = CharacterSlot.CharacterSlot_01;
                currentCharacterData = new CharacterSaveData();
                OnNewGame();
                return;
            }

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotUsed(CharacterSlot.CharacterSlot_02);
            if (!saveFileDataWriter.CheckIfFileExists())
            {
                currentSlot = CharacterSlot.CharacterSlot_02;
                currentCharacterData = new CharacterSaveData();
                OnNewGame();
                return;
            }

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotUsed(CharacterSlot.CharacterSlot_03);
            if (!saveFileDataWriter.CheckIfFileExists())
            {
                currentSlot = CharacterSlot.CharacterSlot_03;
                currentCharacterData = new CharacterSaveData();
                OnNewGame();
                return;
            }

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotUsed(CharacterSlot.CharacterSlot_04);
            if (!saveFileDataWriter.CheckIfFileExists())
            {
                currentSlot = CharacterSlot.CharacterSlot_04;
                currentCharacterData = new CharacterSaveData();
                OnNewGame();
                return;
            }

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotUsed(CharacterSlot.CharacterSlot_05);
            if (!saveFileDataWriter.CheckIfFileExists())
            {
                currentSlot = CharacterSlot.CharacterSlot_05;
                currentCharacterData = new CharacterSaveData();
                OnNewGame();
                return;
            }

            TitleScreenManager.Instance.DisplayNoFreeSlotPopup();
        }

        private void OnNewGame()
        {
            // saves newly created stats and items when creation screen is added
            player.playerNetworkManager.vitality.Value = 15;
            player.playerNetworkManager.endurance.Value = 10;

            SaveGame();
            StartCoroutine(LoadWorldScene());
        }

        public void LoadGame()
        {
            // load a previous file, with a file name depending on which slot we are using
            fileName = DecideCharacterFileNameBasedOnCharacterSlotUsed(currentSlot);

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
            fileName = DecideCharacterFileNameBasedOnCharacterSlotUsed(currentSlot);

            saveFileDataWriter = new WriteSaveData();
            saveFileDataWriter.saveDataPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = fileName;

            // pass the players info from game to their save file
            player.SaveGameDateToCurrentCharacterData(ref currentCharacterData);

            // write that info onto a json file saved to this machine
            saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
        }

        public void DeleteGame(CharacterSlot characterSlot)
        {
            // chose a file to delete based on name
            saveFileDataWriter = new WriteSaveData();
            saveFileDataWriter.saveDataPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotUsed(characterSlot);

            saveFileDataWriter.DeleteSaveFile();
        }

        // load all character slots on device when starting game
        private void LoadAllCharacterSlots()
        {
            saveFileDataWriter = new WriteSaveData();
            saveFileDataWriter.saveDataPath = Application.persistentDataPath;

            saveFileDataWriter.saveFileName =
                DecideCharacterFileNameBasedOnCharacterSlotUsed(CharacterSlot.CharacterSlot_01);
            characterSlot01 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName =
                DecideCharacterFileNameBasedOnCharacterSlotUsed(CharacterSlot.CharacterSlot_02);
            characterSlot02 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName =
                DecideCharacterFileNameBasedOnCharacterSlotUsed(CharacterSlot.CharacterSlot_03);
            characterSlot03 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName =
                DecideCharacterFileNameBasedOnCharacterSlotUsed(CharacterSlot.CharacterSlot_04);
            characterSlot04 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName =
                DecideCharacterFileNameBasedOnCharacterSlotUsed(CharacterSlot.CharacterSlot_05);
            characterSlot05 = saveFileDataWriter.LoadSaveFile();
        }

        public IEnumerator LoadWorldScene()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

            player.LoadGameFromCurrentCharacterData(ref currentCharacterData);

            yield return null;
        }

        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }
    }
}
