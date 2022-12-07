using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager> {

    [System.NonSerialized] public bool isGameStart = false;
    [System.NonSerialized] public bool isGameEnd = false;
    [System.NonSerialized] public float passedTime = 0.0F;
    [System.NonSerialized] public float standardTime = 0.0F;

    public void Awake() {
        if (this != Instance) {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start() {

    }

    void Update() {
        this.passedTime += Time.deltaTime;
    }

    /// <summary>
    /// 離すべき時間を格納して基準時間とする
    /// </summary>
    public void decideStandardTime() {
        this.standardTime = this.passedTime;
    }

    /// <summary>
    /// ゲーム開始を受信
    /// </summary>
    public void gameStart() {
        this.isGameStart = true;
    }

    /// <summary>
    /// ゲーム終了を受信
    /// </summary>
    public void gameEnd() {
        this.isGameEnd = true;
    }

    public float getPassedTime() {
        return this.passedTime;
    }
}
