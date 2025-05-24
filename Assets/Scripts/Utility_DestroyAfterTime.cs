using UnityEngine;

namespace DKC
{
    public class Utility_DestroyAfterTime : MonoBehaviour
    {
        [SerializeField] float timeUntilDestroy = 5;

        private void Awake()
        {
            // Destroy this GameObject after the specified time
            Destroy(gameObject, timeUntilDestroy);
        }
    }
}