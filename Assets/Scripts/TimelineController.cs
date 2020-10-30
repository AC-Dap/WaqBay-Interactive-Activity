using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineController : MonoBehaviour
{
    public Slider slider;
    
    public GameObject animComponents;
    private Animator anim;
    public string animationStateName;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = animComponents.GetComponent<Animator>();
        anim.speed = 0;
    }

    public void SetAnimationTime()
    {
        anim.Play("Base Layer." + animationStateName, 0, slider.normalizedValue);
    }
}
