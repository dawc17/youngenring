using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace DKC
{
    public class WorldCharacterEffectsManager : MonoBehaviour
    {
        public static WorldCharacterEffectsManager Instance;

        [SerializeField] List<InstantCharacterEffect> instantEffects;

        private void Awake()
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

        private void OnCanvasGroupChanged()
        {
            for (int i = 0; i < instantEffects.Count; i++)
            {
                instantEffects[i].instantEffectID = i;
            }
        }
    }
}