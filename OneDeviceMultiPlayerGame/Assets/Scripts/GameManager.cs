using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager> {

    [System.NonSerialized] public bool isGameStart = false;

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

    }

    /// <summary>
    /// ????????????????????
    /// </summary>
    public void gameStart() {
        Debug.Log("?????");
        this.isGameStart = true;
    }
}
