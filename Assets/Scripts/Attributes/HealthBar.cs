using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{    
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health healthComponent = null;
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas rootCanvas = null;

        void Update()
        {
            float percent = healthComponent.GetPoints() / healthComponent.GetInitialHealth();
            foreground.localScale = new Vector3(percent, 1f, 1f);
            rootCanvas.enabled = 0f < percent && percent < 1f;
        }
    }
}