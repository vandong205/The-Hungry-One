using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
public class DialogBox : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject layout;
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private InputActionReference skipAction;
    [SerializeField] private TMP_Text NameText;


    [Header("Audio")]
    [SerializeField] private AudioSource defaultAudioSource;

    [Header("Typing")]
    [SerializeField] private float charactersPerSecond = 30f;

    private readonly Queue<DialogLine> textQueue = new();

    private Tween typingTween;

    private Action onDialogCompleted;
    private float callbackDelay;

    private bool callbackInvoked;
    private bool isTyping;

    private AudioSource currentAudioSource;
    #region Unity

    private void Awake()
    {
        dialogText.text = string.Empty;
        dialogText.maxVisibleCharacters = 0;

        layout.SetActive(false);
    }

    private void OnEnable()
    {
        if (skipAction != null)
        {
            skipAction.action.Enable();
            skipAction.action.performed += HandleSkip;
        }
    }

    private void OnDisable()
    {
        if (skipAction != null)
        {
            skipAction.action.performed -= HandleSkip;
        }
    }

    private void OnDestroy()
    {
        typingTween?.Kill();
    }

    #endregion

    #region Public API

    public void StartDialog(
        IEnumerable<DialogLine> lines,
        Action callback = null,
        float delay = 0f)
    {
        typingTween?.Kill();

        StopAllCoroutines();

        StopCurrentAudio();

        textQueue.Clear();

        onDialogCompleted = callback;
        callbackDelay = delay;
        callbackInvoked = false;

        isTyping = false;

        foreach (DialogLine line in lines)
        {
            if (line != null &&
                !string.IsNullOrWhiteSpace(line.Text))
            {
                textQueue.Enqueue(line);
            }
        }

        ShowNextInQueue();
    }

    #endregion

    #region Dialog

    private void Show(DialogLine line)
    {
        typingTween?.Kill();

        StopCurrentAudio();

        layout.SetActive(true);

        string text = GetTextWithSpaceChars(line.Text);

        dialogText.text = text;
        dialogText.maxVisibleCharacters = 0;

        currentAudioSource =
            line.AudioSource != null
                ? line.AudioSource
                : defaultAudioSource;

        if (line.AudioClip != null &&
            currentAudioSource != null)
        {
            currentAudioSource.Stop();
            currentAudioSource.clip = line.AudioClip;
            currentAudioSource.Play();
        }

        int length = text.Length;

        float duration = Mathf.Max(
            0.01f,
            length / charactersPerSecond
        );

        isTyping = true;
        NameText.text = line.Name;
        typingTween = DOTween.To(
                () => dialogText.maxVisibleCharacters,
                value => dialogText.maxVisibleCharacters = value,
                length,
                duration
            )
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                isTyping = false;
            });
    }

    private void ShowNextInQueue()
    {
        typingTween?.Kill();

        if (textQueue.Count == 0)
        {
            CompleteDialog();
            return;
        }

        DialogLine nextLine = textQueue.Dequeue();

        Show(nextLine);
    }

    private void CompleteDialog()
    {
        Hide();

        if (callbackInvoked)
            return;

        callbackInvoked = true;

        if (onDialogCompleted == null)
            return;

        if (callbackDelay <= 0f)
        {
            onDialogCompleted.Invoke();
        }
        else
        {
            StartCoroutine(InvokeDelayed());
        }
    }

    private IEnumerator InvokeDelayed()
    {
        yield return new WaitForSeconds(callbackDelay);

        onDialogCompleted?.Invoke();
    }

    private void Hide()
    {
        typingTween?.Kill();

        isTyping = false;

        StopCurrentAudio();

        dialogText.text = string.Empty;
        dialogText.maxVisibleCharacters = 0;

        layout.SetActive(false);
    }

    private void StopCurrentAudio()
    {
        if (currentAudioSource != null)
        {
            currentAudioSource.Stop();
        }
    }

    #endregion

    #region Input

    private void HandleSkip(InputAction.CallbackContext context)
    {
        Skip();
    }

    private void Skip()
    {
        typingTween?.Kill();
        isTyping = false;

        if (textQueue.Count > 0)
        {
            ShowNextInQueue();
            return;
        }

        CompleteDialog();
    }

    #endregion

    private string GetTextWithSpaceChars(string text)
    {
        return text + " [space]";
    }
}