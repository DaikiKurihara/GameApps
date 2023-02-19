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
        Debug.Log("離れる音!");
        audioSource.PlayOneShot(leftTouchSE);
    }

    public void onTouch() {
        Debug.Log("触った音!");
        audioSource.PlayOneShot(touchSE);
    }

    public void onSurprise() {

    }
}
