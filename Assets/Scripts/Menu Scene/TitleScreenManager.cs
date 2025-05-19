using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace DKC
{
    public class TitleScreenManager : MonoBehaviour
    {
        public static TitleScreenManager Instance;
        [Header("Menus")]
        [SerializeField] private GameObject titleScreenMainMenu;
        [SerializeField] private GameObject titleScreenLoadMenu;

        [Header("Buttons")] 
        [SerializeField] private Button loadMenuReturnButton;
        [SerializeField] private Button mainMenuLoadGameButton;
        [SerializeField] private Button mainMenuNewGameButton;
        [SerializeField] private Button deleteCharacterPopupConfirmButton;

        [Header("Popups")] 
        [SerializeField] private GameObject noCharacterSlotsPopup;
        [SerializeField] private Button noChatacterSlotsButton;
        [SerializeField] GameObject deleteCharacterSlotPopup;

        [Header("Character Slots")] 
        public CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLOT;

        [Header("Title Screen Inputs")] 
        [SerializeField] private bool deleteCharacterSlot = false;
        
        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        public void StartNewGame()
        {
            WorldSaveGameManager.Instance.CreateNewGame();
        }

        public void OpenLoadGameMenu()
        {
            // close main menu
            titleScreenMainMenu.SetActive(false);
            // open load menu
            titleScreenLoadMenu.SetActive(true);
            
            // Ensure delete character popup is hidden when opening the load menu
            if (deleteCharacterSlotPopup != null)
            {
                deleteCharacterSlotPopup.SetActive(false);
            }
            
            SelectNoSlot(); // Reset selected slot to prevent accidental delete popup
            loadMenuReturnButton.Select();
        }

        public void CloseLoadGameMenu()
        {
            // close load menu
            titleScreenLoadMenu.SetActive(false);
            // open main menu
            titleScreenMainMenu.SetActive(true);
            
            mainMenuLoadGameButton.Select();
        }

        public void DisplayNoFreeSlotPopup()
        {
            noCharacterSlotsPopup.SetActive(true);
            noChatacterSlotsButton.Select();
        }

        public void CloseNoFreeCharacterSlotsPopup()
        {
            noCharacterSlotsPopup.SetActive(false);
            mainMenuLoadGameButton.Select();
        }
        
        // character slots

        public void SelectCharacterSlot(CharacterSlot characterSlot)
        {
            currentSelectedSlot = characterSlot;
        }

        public void SelectNoSlot()
        {
            currentSelectedSlot = CharacterSlot.NO_SLOT;
        }

        public void AttemptToDeleteCharacterSlot()
        {
            if (currentSelectedSlot != CharacterSlot.NO_SLOT)
            {
                deleteCharacterSlotPopup.SetActive(true);
                deleteCharacterPopupConfirmButton.Select();
            }
        }

        public void DeleteCharacterSlot()
        {
            deleteCharacterSlotPopup.SetActive(false);
            WorldSaveGameManager.Instance.DeleteGame(currentSelectedSlot);
            
            // refresh the list when delete
            titleScreenLoadMenu.SetActive(false);
            titleScreenLoadMenu.SetActive(true);
            loadMenuReturnButton.Select();
        }

        public void CloseDeleteCharacterPopup()
        {
            deleteCharacterSlotPopup.SetActive(false);
            loadMenuReturnButton.Select();
        }
    }
}
