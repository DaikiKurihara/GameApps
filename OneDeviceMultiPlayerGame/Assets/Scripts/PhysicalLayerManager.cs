using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物理層の出力（音、振動）を統括するクラス
/// </summary>
public class PhysicalLayerManager : SingletonMonoBehaviour<PhysicalLayerManager> {
    [SerializeField] private AudioClip leftTouchSE;
    [SerializeField] private AudioClip touchSE;
    [SerializeField] private AudioClip surpriseSE;
    [SerializeField] private AudioClip surprise2SE;
    [SerializeField] private AudioClip fireSE;
    private AudioSource audioSource;

    public void Awake() {
        if (this != Instance) {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void onLeftTouch() {
        audioSource.PlayOneShot(leftTouchSE);
    }

    public void onTouch() {
        audioSource.PlayOneShot(touchSE);
    }

    public void onSurpriseRandom() {
        // 0,1,2のいずれかを取る。Max値は含まない
        int random = Random.Range(0, 3);
        if (random == 0) onSurprise();
        else if (random == 1) onSurprise2();
        else shortVibration();
    }

    public void onSurprise() {
        audioSource.PlayOneShot(surpriseSE);
    }

    public void onSurprise2() {
        audioSource.PlayOneShot(surprise2SE);
    }

    public void shortVibration() {
        if (SystemInfo.supportsVibration) {
            Handheld.Vibrate();
        }
    }

    /// <summary>
    /// サークルの色が変わったとき
    /// </summary>
    public void onFire() {
        audioSource.PlayOneShot(fireSE);
    }
}
