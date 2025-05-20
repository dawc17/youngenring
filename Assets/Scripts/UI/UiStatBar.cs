using System;
using Character.Player.UI;
using UnityEngine;
using UnityEngine.UI;

namespace DKC
{
    public class UiStatBar : MonoBehaviour
    {
        private Slider slider;
        private RectTransform rectTransform;

        [Header("Bar Options")] 
        [SerializeField] protected bool scaleBarLengthWithStats = true;
        [SerializeField] protected float widthScaleMultiplier = 1;
        // variable to scale bar size depending on endurance stat
        // secondary bar that shows how much stamina you consume with an action

        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
            rectTransform = GetComponent<RectTransform>();
        }

        public virtual void SetStat(int newValue)
        {
            slider.value = newValue;
        }

        public virtual void SetMaxStat(int maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;

            if (scaleBarLengthWithStats)
            {
                // scale the transform
                rectTransform.sizeDelta = new Vector2(maxValue * widthScaleMultiplier, rectTransform.sizeDelta.y);
                
                // resets the position of bars based on their layout group
                PlayerUIManager.instance.playerUIHudManager.RefreshHUD();
            }
        }
    }
}