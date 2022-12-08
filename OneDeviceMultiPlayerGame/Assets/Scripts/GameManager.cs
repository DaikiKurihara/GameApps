using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager> {

    [System.NonSerialized] public bool isGameStart = false;
    [System.NonSerialized] public bool isGameEnd = false;
    /** ゲーム開始からの経過時間 */
    [System.NonSerialized] public float passedTime = 0.0F;
    /** 指を離す指定時間 */
    [System.NonSerialized] public float standardTime = 0.0F;
    /** 指を離した時間を格納する辞書 */
    [System.NonSerialized] public Dictionary<int, float> leftTimeMap = new Dictionary<int, float>();

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
        if (this.isGameStart) {
            this.passedTime += Time.deltaTime;
        }
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fingerId"></param>
    private void addLeftTimeMap(int fingerId) {
        this.leftTimeMap.Add(fingerId, this.passedTime);
        Debug.Log(string.Join(",", this.leftTimeMap.ToString()));
        Debug.Log(string.Join(",", this.leftTimeMap.ToArray()));
    }
}
