using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PhotoGallaryController : MonoBehaviour, IScrollHandler
{
    [Serializable]
    public class GallaryItem
    {
        public Sprite sprite;
        [TextArea(1, 10)]
        public string caption;
    }
    
    public GameObject viewport;
    private RawImage _activeImg;
    private RawImage _inactiveImg;
    
    public GameObject preview;
    private Image[] _previewImgs;
    private RawImage _borderImg;

    public GallaryItem[] allItems;

    private int _offset = 0;

    private bool _isScrolling = false;
    private Vector3 _viewportSpacing;
    private Vector3 _previewSpacing;

    public RawImage caption;
    public TextMeshProUGUI captionText;
    private float _captionFullOpacity;
    
    void Start()
    {
        foreach (var img in viewport.GetComponentsInChildren<RawImage>())
        {
            if (img.name == "Img 1") _activeImg = img;
            else _inactiveImg = img;
        }

        _activeImg.texture = allItems[0].sprite.texture;

        if (allItems.Length > 1)
        {
            _inactiveImg.texture = allItems[1].sprite.texture;

            _previewImgs = preview.GetComponentsInChildren<Image>();
            Array.Sort(_previewImgs, (i1, i2) => i1.transform.position.x.CompareTo(i2.transform.position.x));
            _borderImg = preview.GetComponentInChildren<RawImage>();
        
            for (int i = 0; i < _previewImgs.Length; i++)
            {
                _previewImgs[i].sprite = allItems[i % allItems.Length].sprite;
            }
        }
        
        var tempColor = caption.color;
        _captionFullOpacity = tempColor.a;
        tempColor.a = 0;
        caption.color = tempColor;

        tempColor = captionText.color;
        tempColor.a = 0;
        captionText.color = tempColor;
        captionText.text = allItems[0].caption;
    }

    public void ShowCaption()
    {
        StartCoroutine(FadeInCaption());
    }

    public void HideCaption()
    {
        StartCoroutine(FadeOutCaption());
    }

    private IEnumerator FadeInCaption()
    {
        for (float t = 0; t <= 1; t += 0.05f)
        {
            float captionOpacity = Mathf.Lerp(0, _captionFullOpacity, t);
            float textOpacity = Mathf.Lerp(0, 1, t);

            var tempColor = caption.color;
            tempColor.a = captionOpacity;
            caption.color = tempColor;

            tempColor = captionText.color;
            tempColor.a = textOpacity;
            captionText.color = tempColor;
            
            yield return null;
        }
    }
    
    private IEnumerator FadeOutCaption()
    {
        for (float t = 0; t <= 1; t += 0.05f)
        {
            float captionOpacity = Mathf.Lerp(_captionFullOpacity, 0, t);
            float textOpacity = Mathf.Lerp(1, 0, t);

            var tempColor = caption.color;
            tempColor.a = captionOpacity;
            caption.color = tempColor;

            tempColor = captionText.color;
            tempColor.a = textOpacity;
            captionText.color = tempColor;

            yield return null;
        }
    }

    //On viewport as it blocks other events
    public void Scrollwheel()
    {
        if(Input.mouseScrollDelta.y > 0) ScrollLeft();
        else ScrollRight();
    }
    
    // On entire panel
    public void OnScroll(PointerEventData eventData)
    {
        if (eventData.scrollDelta.y > 0) ScrollLeft();
        else ScrollRight();
    }
    
    public void ScrollLeft()
    {
        if (!_isScrolling && allItems.Length > 1)
        {
            UpdateSpacings();
            StartCoroutine(AnimateLeft(1));
        }
    }

    public void ScrollRight()
    {
        if (!_isScrolling && allItems.Length > 1)
        {
            UpdateSpacings();
            StartCoroutine(AnimateRight(1));
        }
    }

    public void ButtonScroll(int num)
    {
        int scrolls = Mod(num - _offset, _previewImgs.Length);
        if (scrolls != 0)
        {
            UpdateSpacings();
            StartCoroutine(AnimateRight(scrolls));
        }
    }

    private void UpdateSpacings()
    {
        float viewportDistX = _activeImg.transform.position.x - _inactiveImg.transform.position.x;
        _viewportSpacing = new Vector3(Math.Abs(viewportDistX), 0, 0);
        float previewDistX = _previewImgs[Mod(_offset + 1, _previewImgs.Length)].transform.position.x -
                             _previewImgs[Mod(_offset, _previewImgs.Length)].transform.position.x;
        _previewSpacing = new Vector3(previewDistX, 0, 0);
    }

    private IEnumerator AnimateLeft(int numOfScrolls)
    {
        _isScrolling = true;
        Vector3 startPos = _activeImg.transform.position;
        Vector3 endPos = startPos + _viewportSpacing;

        _inactiveImg.transform.position = startPos - _viewportSpacing;
        _inactiveImg.texture = allItems[Mod(_offset-1, allItems.Length)].sprite.texture;

        Image firstImg = _previewImgs[Mod(_offset, _previewImgs.Length)];
        Image lastImg = _previewImgs[Mod(_offset - 1, _previewImgs.Length)];

        Vector3 previewStartPos = firstImg.transform.position - _previewSpacing;
        Vector3 previewEndPos = previewStartPos + _previewSpacing;

        lastImg.transform.position = previewStartPos;
        lastImg.sprite = allItems[Mod(_offset - 1,allItems.Length)].sprite;
        
        for (float t = 0; t <= 1; t += 0.05f)
        {
            Vector3 activePos = Vector3.Lerp(startPos, endPos, t);
            _activeImg.transform.position = activePos;
            _inactiveImg.transform.position = activePos - _viewportSpacing;
            
            Vector3 firstPos = Vector3.Lerp(previewStartPos, previewEndPos, t);
            for (int i = 0; i < _previewImgs.Length; i++)
            {
                _previewImgs[Mod(_offset - 1 + i, _previewImgs.Length)].transform.position = firstPos + _previewSpacing * i;
            }

            _borderImg.transform.position = firstPos;
            
            yield return null;
        }

        _activeImg.transform.position = endPos;
        _inactiveImg.transform.position = startPos;
        for (int i = 0; i < _previewImgs.Length; i++)
        {
            _previewImgs[Mod(_offset - 1 + i, _previewImgs.Length)].transform.position = previewEndPos + _previewSpacing * i;
        }
        _borderImg.transform.position = previewEndPos;

        RawImage temp = _activeImg;
        _activeImg = _inactiveImg;
        _inactiveImg = temp;
        _offset--;

        captionText.text = allItems[Mod(_offset, allItems.Length)].caption;
        
        _isScrolling = false;
        
        numOfScrolls--;
        if (numOfScrolls > 0) StartCoroutine(AnimateLeft(numOfScrolls));
    }

    private IEnumerator AnimateRight(int numOfScrolls)
    {
        _isScrolling = true;

        Vector3 viewportStartPos = _activeImg.transform.position;
        Vector3 viewportEndPos = viewportStartPos - _viewportSpacing;

        _inactiveImg.transform.position = viewportStartPos + _viewportSpacing;
        _inactiveImg.texture = allItems[Mod(_offset + 1, allItems.Length)].sprite.texture;

        Image firstImg = _previewImgs[Mod(_offset, _previewImgs.Length)];
        Image lastImg = _previewImgs[Mod(_offset - 1, _previewImgs.Length)];

        Vector3 previewStartPos = firstImg.transform.position;
        Vector3 previewEndPos = previewStartPos - _previewSpacing;

        lastImg.transform.position = previewStartPos + _previewSpacing * (_previewImgs.Length - 1);
        lastImg.sprite = allItems[Mod(_offset + _previewImgs.Length - 1, allItems.Length)].sprite;

        for (float t = 0; t <= 1; t += 0.05f)
        {
            Vector3 activePos = Vector3.Lerp(viewportStartPos, viewportEndPos, t);
            _activeImg.transform.position = activePos;
            _inactiveImg.transform.position = activePos + _viewportSpacing;

            Vector3 firstPos = Vector3.Lerp(previewStartPos, previewEndPos, t);
            for (int i = 0; i < _previewImgs.Length; i++)
            {
                _previewImgs[Mod(_offset + i, _previewImgs.Length)].transform.position = firstPos + _previewSpacing * i;
            }

            _borderImg.transform.position = firstPos;
            
            yield return null;
        }

        _activeImg.transform.position = viewportEndPos;
        _inactiveImg.transform.position = viewportStartPos;
        for (int i = 0; i < _previewImgs.Length; i++)
        {
            _previewImgs[Mod(_offset + i, _previewImgs.Length)].transform.position = previewEndPos + _previewSpacing * i;
        }
        _borderImg.transform.position = previewStartPos;

        RawImage temp = _activeImg;
        _activeImg = _inactiveImg;
        _inactiveImg = temp;
        _offset++;
        
        captionText.text = allItems[Mod(_offset, allItems.Length)].caption;

        _isScrolling = false;

        numOfScrolls--;
        if (numOfScrolls > 0) StartCoroutine(AnimateRight(numOfScrolls));
    }

    private int Mod(int x, int m)
    {
        int r = x % m;
        return (r < 0) ? r + m : r;
    }
}
