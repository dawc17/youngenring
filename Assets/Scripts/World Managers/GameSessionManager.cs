using System.Collections.Generic;
using UnityEngine;

namespace DKC
{
    public class GameSessionManager : MonoBehaviour
    {
        public static GameSessionManager Instance;
        [Header("Active Players In Session")]
        public List<PlayerManager> players = new List<PlayerManager>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void AddPlayerToActivePlayers(PlayerManager player)
        {
            if (!players.Contains(player))
            {
                players.Add(player);
            }
            else
            {
                Debug.LogWarning("Player is already in the active players list.");
            }

            for (int i = players.Count - 1; i > -1; i--)
            {
                if (players[i] == null)
                {
                    players.RemoveAt(i);
                }
            }
        }

        public void RemovePlayerFromActivePlayers(PlayerManager player)
        {
            if (players.Contains(player))
            {
                players.Remove(player);
                Debug.Log($"Player {player.name} removed from active players list.");
            }

            for (int i = players.Count - 1; i > -1; i--)
            {
                if (players[i] == null)
                {
                    players.RemoveAt(i);
                }
            }
        }
    }
}