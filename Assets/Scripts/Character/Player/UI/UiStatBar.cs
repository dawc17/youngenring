using System;
using UnityEngine;
using UnityEngine.UI;

namespace DKC
{
    public class UiStatBar : MonoBehaviour
    {
        private Slider slider;
        // variable to scale bar size depending on endurance stat
        // secondary bar that shows how much stamina you consume with an action

        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
        }

        public virtual void SetStat(int newValue)
        {
            slider.value = newValue;
        }

        public virtual void SetMaxStat(int maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;
        }
    }
}