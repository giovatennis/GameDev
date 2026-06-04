using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthUI : MonoBehaviour
{
    public Image[] healthIcons;

    public void RemoveIcon()
    {
        for (int i = healthIcons.Length - 1; i >= 0; i--)
        {
            if (healthIcons[i].enabled)
            {
                healthIcons[i].enabled = false;
                return;
            }
        }
    }
}