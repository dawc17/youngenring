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

        [Header("Popups")] 
        [SerializeField] private GameObject noCharacterSlotsPopup;

        [SerializeField] private Button noChatacterSlotsButton;
        
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
            
            loadMenuReturnButton.Select();
        }

        public void CloseLoadGameMenu()
        {
            // close load menu
            titleScreenLoadMenu.SetActive(false);
            // open main menu
            titleScreenMainMenu.SetActive(true);
            
            loadMenuReturnButton.Select();
        }

        public void DisplayNoFreeSlotPopup()
        {
            noCharacterSlotsPopup.SetActive(true);
            noChatacterSlotsButton.Select();
        }

        public void CloseNoFreeCharacterSlotsPopup()
        {
            noCharacterSlotsPopup.SetActive(false);
            mainMenuNewGameButton.Select();
        }
    }
}
