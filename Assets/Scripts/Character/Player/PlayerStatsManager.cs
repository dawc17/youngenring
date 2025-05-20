using UnityEngine;

namespace DKC
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        private PlayerManager player;
        
        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Start()
        {
            base.Start();

            // why calculate this here?
            // when we make character creatin menyu and set stats, this will be calculated there
            // until then they are never calculated so we do it here on start, if a save file exists they will be overwritten
            CalculateHealthBasedOnVitalityLevel(player.playerNetworkManager.vitality.Value);
            CalculateStaminaBasedOnEnduranceLevel(player.playerNetworkManager.endurance.Value);
        }
    }
}