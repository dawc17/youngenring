using Unity.Netcode;
using UnityEngine;

namespace DKC
{
    public class DoNot : MonoBehaviour
    {
        public static DoNot Instance;
        
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
    }
}