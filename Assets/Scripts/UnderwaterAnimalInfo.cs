using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnderwaterAnimalInfo : MonoBehaviour
{
    public TextMeshProUGUI tooltip;
    private bool showTooltip;
    
    public void ShowAnimalInfoPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void HideAnimalInfoPanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    public void ShowTooltip(string text)
    {
        showTooltip = true;
        tooltip.gameObject.SetActive(true);
        tooltip.text = text;
        tooltip.rectTransform.sizeDelta = new Vector2(tooltip.preferredWidth, tooltip.preferredHeight);
    }
    
    public void HideTooltip()
    {
        showTooltip = false;
        tooltip.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (showTooltip)
        {
            tooltip.transform.position = Input.mousePosition;
        }
    }
}
