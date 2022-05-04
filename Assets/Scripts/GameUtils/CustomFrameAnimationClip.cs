using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Custom Animation",menuName = "Scriptable Object/Custom/Custom Animation")]
public class CustomFrameAnimationClip : ScriptableObject
{
    public Sprite[] frames;
    public int fps;
    public bool needLoop = false;
}
