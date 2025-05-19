using System;
using TMPro;
using UnityEngine;

namespace DKC
{
    public class UiCharacterSaveSlot : MonoBehaviour
    {
        private WriteSaveData saveFileWriter;

        [Header("Game Slot")] 
        public CharacterSlot characterSlot;

        [Header("Character Info")] 
        public TextMeshProUGUI characterName;
        public TextMeshProUGUI timePlayed;

        private void OnEnable()
        {
            LoadSaveSlot();
        }

        private void LoadSaveSlot()
        {
            saveFileWriter = new WriteSaveData();
            saveFileWriter.saveDataPath = Application.persistentDataPath;

            if (characterSlot == CharacterSlot.CharacterSlot_01)
            {
                saveFileWriter.saveFileName =
                    WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotUsed(characterSlot);

                // if file exists get info from it
                if (saveFileWriter.CheckIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.Instance.characterSlot01.characterName;
                }
                // if not disable game object
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_02)
            {
                saveFileWriter.saveFileName =
                    WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotUsed(characterSlot);

                // if file exists get info from it
                if (saveFileWriter.CheckIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.Instance.characterSlot02.characterName;
                }
                // if not disable game object
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_03)
            {
                saveFileWriter.saveFileName =
                    WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotUsed(characterSlot);

                // if file exists get info from it
                if (saveFileWriter.CheckIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.Instance.characterSlot03.characterName;
                }
                // if not disable game object
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_04)
            {
                saveFileWriter.saveFileName =
                    WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotUsed(characterSlot);

                // if file exists get info from it
                if (saveFileWriter.CheckIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.Instance.characterSlot04.characterName;
                }
                // if not disable game object
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_05)
            {
                saveFileWriter.saveFileName =
                    WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotUsed(characterSlot);

                // if file exists get info from it
                if (saveFileWriter.CheckIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.Instance.characterSlot05.characterName;
                }
                // if not disable game object
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }

        public void LoadGameFromCharacterSlot()
        {
            WorldSaveGameManager.Instance.currentSlot = characterSlot;
            WorldSaveGameManager.Instance.LoadGame();
        }

        public void SelectCurrentSlot()
        {
            TitleScreenManager.Instance.SelectCharacterSlot(characterSlot);
        }
    }
}