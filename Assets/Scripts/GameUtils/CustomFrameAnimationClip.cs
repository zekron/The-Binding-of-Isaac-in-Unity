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
    /// ���� <paramref name="currentFrameIndex"/> ����һ֡��
    /// �������Ƭ��Ҫѭ�����ţ��ҵ�����Ƭ���һ֡ʱ�����ص� 0 ֡������Ҫ�򷵻ص� <paramref name="currentFrameIndex"/> ֡��
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
