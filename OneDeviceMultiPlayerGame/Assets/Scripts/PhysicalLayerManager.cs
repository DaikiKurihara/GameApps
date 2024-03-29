﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物理層の出力（音、振動）を統括するクラス
/// </summary>
public class PhysicalLayerManager : MonoBehaviour {
    [SerializeField] private AudioClip leftTouchSE;
    [SerializeField] private AudioClip touchSE;
    [SerializeField] private AudioClip surpriseSE;
    [SerializeField] private AudioClip surprise2SE;
    [SerializeField] private AudioClip fireSE;
    [SerializeField] private AudioClip resultSE;
    [SerializeField] private AudioClip forceTerminatedSE;
    private AudioSource audioSource;
    private GameManager gameManager;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        gameManager = GameObject.FindWithTag(CommonConstant.GAME_MANAGER).GetComponent<GameManager>();
    }

    public void onLeftTouch() {
        audioSource.PlayOneShot(leftTouchSE);
    }

    public void onTouch() {
        audioSource.PlayOneShot(touchSE);
    }

    public void onSurpriseRandom() {
        if (!gameManager.isOnFeintSound && gameManager.isOnVibration) {
            shortVibration();
            return;
        } else if (gameManager.isOnFeintSound && !gameManager.isOnVibration) {
            onSurpriseSoundRandom();
            return;
        } else if (!gameManager.isOnFeintSound && !gameManager.isOnVibration) {
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

    private void onSurprise() {
        Debug.Log("音が鳴った");
        audioSource.PlayOneShot(surpriseSE);
    }

    private void onSurprise2() {
        Debug.Log("音2が鳴った");
        audioSource.PlayOneShot(surprise2SE);
    }

    private void shortVibration() {
        Debug.Log("バイブ");
        if (SystemInfo.supportsVibration) {
            Handheld.Vibrate();
        }
    }

    /// <summary>
    /// サークルの色が変わったとき
    /// </summary>
    public void onFire() {
        if (!gameManager.isOnFeintSound && gameManager.isOnVibration) {
            shortVibration();
            return;
        } else if (gameManager.isOnFeintSound && !gameManager.isOnVibration) {
            onSurpriseSoundRandom();
            return;
        } else if (!gameManager.isOnFeintSound && !gameManager.isOnVibration) {
            return;
        }
        int random = Random.Range(0, 4);
        if (random == 0) {
            onSurprise();
        } else if (random == 1) {
            onSurpriseSoundRandom();
        } else if (random == 2) {
            onSurpriseSoundRandom();
            shortVibration();
        } else if (random == 3) {
            // なにもしない
        }
    }

    public void result() {
        if (gameManager.IsForcedTermination) audioSource.PlayOneShot(forceTerminatedSE);
        else audioSource.PlayOneShot(resultSE);
    }
}
