using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class CustomFrameAnimation : MonoBehaviour
{
    [SerializeField] private CustomFrameAnimationClip currentClip;
    [SerializeField] private bool playAutomatically = false;

    private UnityEvent onAnimationEnd = new UnityEvent();
    private SpriteRenderer animationRenderer;
    private float frameInterval;
    private float timer = 0f;
    private bool isPlaying = false;
    private bool isPause = false;

    private void Awake()
    {
        if (animationRenderer == null)
            animationRenderer = GetComponent<SpriteRenderer>();

    }
    // Start is called before the first frame update
    void Start()
    {
        if (currentClip == null) return;

        if (currentClip.fps != 0) frameInterval = 1f / currentClip.fps;
        ResetAnimation();
        isPlaying = playAutomatically;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPause && isPlaying && animationRenderer.enabled)
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
        if (nextFrame != null && animationRenderer.sprite == nextFrame)
        {
            isPlaying = false;
            onAnimationEnd.Invoke();
            onAnimationEnd.RemoveAllListeners();
            return;
        }
        animationRenderer.sprite = nextFrame;
    }

    public void Play()
    {
        if (!isPause) ResetAnimation();
        isPause = false;
        isPlaying = true;
    }

    public void Pause()
    {
        isPause = true;
    }
    public void Stop()
    {
        isPlaying = false;
    }

    public void ResetAnimation()
    {
        timer = 0;
        animationRenderer.sprite = currentClip.ResetClip();
        onAnimationEnd.RemoveAllListeners();
    }

    public CustomFrameAnimation PlayOnce()
    {
        currentClip.needLoop = false;
        Play();
        return this;
    }
    public CustomFrameAnimation PlayLoop()
    {
        currentClip.needLoop = true;
        Play();
        return this;
    }
    public void PlayNextFrame()
    {
        NextFrame();
    }

    public CustomFrameAnimation OnAnimationFinished(UnityAction action)
    {
        onAnimationEnd.AddListener(action);
        return this;
    }
}
