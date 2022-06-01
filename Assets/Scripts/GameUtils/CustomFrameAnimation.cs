using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CustomFrameAnimation : MonoBehaviour
{
    [SerializeField] private CustomFrameAnimationClip currentClip;
    [SerializeField] private bool playAutomatically = false;

    private SpriteRenderer animationRenderer;
    private float frameInterval;
    private float timer = 0f;
    private bool isPlaying = false;

    private int currentFrameIndex;

    private void Awake()
    {
        if (animationRenderer == null)
            animationRenderer = GetComponent<SpriteRenderer>();

    }
    // Start is called before the first frame update
    void Start()
    {
        if (currentClip.fps != 0) frameInterval = 1f / currentClip.fps;

        isPlaying = playAutomatically;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            timer += Time.deltaTime;

            if (timer > frameInterval)
            {
                NextFrame();
                timer = 0;
            }
        }
    }

    private void NextFrame()
    {
        Sprite nextFrame = currentClip.NextFrame();
        if (animationRenderer.sprite == nextFrame)
        {
            isPlaying = false;
            return;
        }
        animationRenderer.sprite = nextFrame;
    }

    public void Play()
    {
        timer = 0;
        currentFrameIndex = 0;
        isPlaying = true;
    }
    public void PlayOnce()
    {
        currentClip.needLoop = false;
        Play();
    }
    public void PlayLoop()
    {
        currentClip.needLoop = true;
        Play();
    }
    public void PlayNextFrame()
    {
        NextFrame();
    }

}
