using UnityEngine;
using System.Collections.Generic;

namespace DKC
{
    public class WorldItemDatabase : MonoBehaviour
    {
        public static WorldItemDatabase Instance;

        public WeaponItem unarmedWeapon;

        [Header("Weapons")]
        [SerializeField] List<WeaponItem> weapons = new List<WeaponItem>();

        [Header("Items")]
        private List<Item> items = new List<Item>();

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

            // add all weapons to list of items
            foreach (var weapon in weapons)
            {
                items.Add(weapon);
            }

            // assign item id for every items
            for (int i = 0; i < items.Count; i++)
            {
                items[i].itemID = i;
            }
        }
    }
}