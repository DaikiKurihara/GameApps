using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonMonoBehaviour<AudioManager> {
    [SerializeField] private AudioClip leftTouchSE;
    [SerializeField] private AudioClip touchSE;
    [SerializeField] private AudioClip surpriseSE;
    private AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }


    public void onLeftTouch() {
        audioSource.PlayOneShot(leftTouchSE);
    }

    public void onTouch() {
        audioSource.PlayOneShot(touchSE);
    }

    public void onSurprise() {
        audioSource.PlayOneShot(surpriseSE);
    }
}
