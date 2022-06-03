using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Custom Animation", menuName = "Scriptable Object/Custom/Custom Animation")]
public class CustomFrameAnimationClip : ScriptableObject
{
    [SerializeField] private Sprite[] frames;
    public int fps;
    public bool needLoop = false;

    private int currentFrameIndex;

    public Sprite CurrentFrame()
    {
        return frames[currentFrameIndex];
    }

    public Sprite NextFrame()
    {
        if (currentFrameIndex == frames.Length - 1)
        {
            currentFrameIndex = needLoop ? 0 : currentFrameIndex;
        }
        else
            currentFrameIndex++;

        return frames[currentFrameIndex];
    }

    public Sprite ResetClip()
    {
        return frames[currentFrameIndex = 0];
    }
}
