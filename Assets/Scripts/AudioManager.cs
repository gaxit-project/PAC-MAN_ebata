using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] se;
    [SerializeField] private AudioClip[] bgm;

    public void PlaySound(int n)
    {
        audioSource.PlayOneShot(se[n]);
    }

    public void PlayBGM(int n)
    {
        audioSource.clip = bgm[n];
        audioSource.Play();
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }

}
