using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoPanelController : MonoBehaviour, IPointerDownHandler
{

    public Animator infoButtonAni;
    private int _instanceID;

    private void Start()
    {
        _instanceID = gameObject.GetInstanceID();
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
        if(infoButtonAni != null) infoButtonAni.enabled = false;
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
        if (infoButtonAni != null)
        {
            infoButtonAni.enabled = true;
            infoButtonAni.SetTrigger("Normal");   
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.pointerPressRaycast.gameObject.GetInstanceID() == _instanceID) HidePanel();
    }
}
