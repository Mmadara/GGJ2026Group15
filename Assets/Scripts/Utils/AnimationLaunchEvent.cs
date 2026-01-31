using System;
using UnityEngine;

public class AnimationLaunchEvent : MonoBehaviour
{

    private Action _onAnimationEvent;

    public void SetEventToLaunch(Action callback)
    {
        _onAnimationEvent = callback;
    }

    public void LaunchEvent()
    {
        _onAnimationEvent?.Invoke();
        _onAnimationEvent = null; 
    }
}
