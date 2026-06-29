using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using System;

public class GlobalEffect : MonoBehaviour
{
    [SerializeField] private Volume volume;
    [SerializeField] private float darkExposure = -10f;
    [SerializeField] private float brightExposure = 0f;

    private ColorAdjustments colorAdjustments;
    private Tween exposureTween;
      private static GlobalEffect _instance;
    public static GlobalEffect Instance=>_instance;
    private void Awake()
    {
          if (_instance != null)
        {
            Destroy(_instance.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        if (volume == null)
            volume = FindFirstObjectByType<Volume>();

        if (volume != null)
        {
            volume.profile.TryGet(out colorAdjustments);
        }
    }

    /// <summary>
    /// Fade từ tối sang sáng.
    /// </summary>
    public void FadeIn(float duration=1,Action callback=null)
    {
        if (colorAdjustments == null)
            return;
        Debug.Log("Da chay effect fadein");
        exposureTween?.Kill();

        colorAdjustments.postExposure.value = darkExposure;

        exposureTween = DOTween.To(
            () => colorAdjustments.postExposure.value,
            x => colorAdjustments.postExposure.value = x,
            brightExposure,
            duration
        ).OnComplete(() =>
        {
            callback?.Invoke();
        });
    }

    /// <summary>
    /// Fade từ sáng sang tối.
    /// </summary>
    public void FadeOut(float duration=1,Action callback=null)
    {
        if (colorAdjustments == null)
            return;
        Debug.Log("Da chay effect fadeout");
        exposureTween?.Kill();

        colorAdjustments.postExposure.value = brightExposure;

        exposureTween = DOTween.To(
            () => colorAdjustments.postExposure.value,
            x => colorAdjustments.postExposure.value = x,
            darkExposure,
            duration
        ).OnComplete(()=>
        callback?.Invoke());
    }
}