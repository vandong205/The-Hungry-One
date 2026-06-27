using System;
using UnityEngine;

[Serializable]
public class DialogLine
{
    public string Name;
    public string Text;
    public AudioClip AudioClip;
    public AudioSource AudioSource;

    public DialogLine(
        string name,
        string text,
        AudioClip audioClip = null,
        AudioSource audioSource = null)
    {
        Name = name;
        Text = text;
        AudioClip = audioClip;
        AudioSource = audioSource;
    }
}