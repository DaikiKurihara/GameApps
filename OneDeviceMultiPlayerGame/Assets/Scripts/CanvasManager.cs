using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : SingletonMonoBehaviour<CanvasManager> {

    public void Awake() {
        if (this != Instance) {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start() {

    }

    /// <summary>
    /// スタートタイマーによるゲーム開始時の挙動
    /// </summary>
    public void gameStart() {
        Debug.Log("ゲーム開始");
    }
}
