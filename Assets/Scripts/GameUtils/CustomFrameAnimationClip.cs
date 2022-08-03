using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Custom Animation", menuName = "Scriptable Object/Custom/Custom Animation")]
public class CustomFrameAnimationClip : ScriptableObject
{
    [SerializeField] private Sprite[] frames;

    public int FramesCount => frames.Length;
    public int fps;
    public bool needLoop = false;

    //private int currentFrameIndex;

    public Sprite CurrentFrame(int currentFrameIndex)
    {
        return frames[currentFrameIndex];
    }

    /// <summary>
    /// 返回 <paramref name="currentFrameIndex"/> 的下一帧。
    /// 如果该切片需要循环播放，且到达切片最后一帧时，返回第 0 帧；不需要则返回第 <paramref name="currentFrameIndex"/> 帧。
    /// </summary>
    /// <param name="currentFrameIndex"></param>
    /// <returns></returns>
    public Sprite NextFrame(ref int currentFrameIndex)
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
        return frames[0];
    }
}
