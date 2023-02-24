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
    private GameManager _gameManager;

    public void Awake() {
        if (this != Instance) {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        audioSource = GetComponent<AudioSource>();
        _gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    public void onLeftTouch() {
        audioSource.PlayOneShot(leftTouchSE);
    }

    public void onTouch() {
        audioSource.PlayOneShot(touchSE);
    }

    public void onSurpriseRandom() {
        Debug.Log($"バイブ:{_gameManager.isOnVibration}、音{_gameManager.isOnFeintSound}");
        if (!_gameManager.isOnFeintSound && _gameManager.isOnVibration) {
            shortVibration();
            return;
        } else if (_gameManager.isOnFeintSound && !_gameManager.isOnVibration) {
            onSurpriseSoundRandom();
            return;
        } else if (!_gameManager.isOnFeintSound && !_gameManager.isOnVibration) {
            return;
        }
        // 0,1のいずれかを取る。Max値は含まない
        int random = Random.Range(0, 2);
        if (random == 0) onSurpriseSoundRandom();
        else if (random == 1) shortVibration();
    }

    private void onSurpriseSoundRandom() {
        int random = Random.Range(0, 2);
        if (random == 0) onSurprise();
        else if (random == 1) onSurprise2();
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
