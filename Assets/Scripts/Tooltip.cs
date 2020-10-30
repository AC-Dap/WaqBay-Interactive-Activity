using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI tooltip;
    private RectTransform _parentPanel;
    
    private bool _showTooltip;

    private void Start()
    {
        _parentPanel = tooltip.transform.parent.GetComponent<RectTransform>();
    }

    public void ShowTooltip(string text)
    {
        text = text.Replace("\\n", "\n");

        _showTooltip = true;
        tooltip.gameObject.SetActive(true);
        tooltip.text = text;
        tooltip.rectTransform.sizeDelta = new Vector2(tooltip.preferredWidth, tooltip.preferredHeight);
    }
    
    public void HideTooltip()
    {
        _showTooltip = false;
        tooltip.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (_showTooltip)
        {
            Vector2 center = Input.mousePosition;
            Vector3[] corners = new Vector3[4];
            _parentPanel.GetWorldCorners(corners);

            center.x -= corners[0].x;
            center.y -= corners[0].y;
            center *= 1600f / (corners[3].x - corners[0].x);
            
            Vector2 dims = tooltip.rectTransform.sizeDelta;

            center.x = Math.Max(center.x, dims.x/2 + 20);
            center.x = Math.Min(center.x, 1600 - dims.x/2 - 20);
            center.y = Math.Max(center.y, 20);
            center.y = Math.Min(center.y, 900 - dims.y - 20);

            tooltip.rectTransform.anchoredPosition = center;
        }
    }
}
