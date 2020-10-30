using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAspectRatio : MonoBehaviour
{

    public float targetHeight = 9.0f;
    public float targetWidth = 16.0f;
    
    private float _targetAspect;
    private Camera _camera;

    private Resolution _lastRes;

    // Use this for initialization
    void Start ()
    {
        _targetAspect = targetWidth / targetHeight;
        _camera = GetComponent<Camera>();
        UpdateAspectRatio();
    }

    private void Update()
    {
        if(Screen.width != _lastRes.width || Screen.height != _lastRes.height) UpdateAspectRatio();
    }

    private void UpdateAspectRatio()
    {
        _lastRes = Screen.currentResolution;
        
        // determine the game window's current aspect ratio
        float windowAspect = (float) Screen.width / (float) Screen.height;

        // current viewport height should be scaled by this amount
        float scaleHeight = windowAspect / _targetAspect;

        // obtain camera component so we can modify its viewport

        // if scaled height is less than current height, add letterbox
        if (scaleHeight < 1.0f)
        {  
            Rect rect = _camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
        
            _camera.rect = rect;
        }
        else // add pillarbox
        {
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = _camera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            _camera.rect = rect;
        }
    }
}
