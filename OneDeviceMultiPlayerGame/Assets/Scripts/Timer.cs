using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public static float currentCountDownTime;
    public TextMeshProUGUI countDownText;
    [SerializeField] float countTime = 3.0F;
    private CanvasManager _canvasManager;
    private bool isStarted;

    void Start() {
        // カウントダウン開始秒数をセット
        currentCountDownTime = countTime;
        _canvasManager = GameObject.FindWithTag("CanvasManager").GetComponent<CanvasManager>();
    }

    void Update() {
        // カウントダウンタイムを整形して表示
        if (!this.isStarted) {

            // 経過時刻を引いていく
            currentCountDownTime -= Time.deltaTime;
            this.countDownText.text = string.Format("{0:00}", currentCountDownTime);
            if (currentCountDownTime <= 0.0F) {
                this.isStarted = true;
                // タイマー表示を0で固定させる
                this.countDownText.text = string.Format("{0:00}", 0.0F);
                this.gameStart();
            }
        }

    }

    private void gameStart() {
        // マネージャーにゲーム開始を通知
        this._canvasManager.gameStart();
    }
}
