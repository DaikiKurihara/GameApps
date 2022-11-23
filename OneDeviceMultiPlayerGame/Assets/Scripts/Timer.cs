using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public static float currentCountDownTime;
    private float currentCountUpTime = 0.0F;
    public TextMeshProUGUI countDownText;
    [SerializeField] float countTime = 5.0F;
    private CanvasManager _canvasManager;
    private bool isStarted;
    /** 指を離す時間（最大）の指定 */
    [SerializeField] int maxLeavingTime = 20;
    bool leaveFingerCounted = false;
    private float leavingTime = 0.0F;


    void Start() {
        _canvasManager = GameObject.FindWithTag("CanvasManager").GetComponent<CanvasManager>();
        // カウントダウン開始秒数をセット
        currentCountDownTime = countTime;
    }

    void Update() {
        // カウントダウン
        if (!this.isStarted) {
            // 経過時刻を引いていく
            currentCountDownTime -= Time.deltaTime;
            // 残り5.00秒で表示「05」残り4.00秒で表示「04」としたいため。+1秒しないと4.99秒で表示「04」となり感覚とズレる
            this.countDownText.text = string.Format("{0:00.00}", currentCountDownTime + 1.0);
            if (currentCountDownTime <= 0.000F) {
                currentCountDownTime = 0.00F;
                this.isStarted = true;
                this.countDownText.text = "";
                this.leavingTime = createLeaveFingerTime();
                Debug.Log("離す時間は：" + this.leavingTime);
                this.gameStart();
            }
        }


        // 離す時間までのカウントアップ
        if (isStarted && !leaveFingerCounted) {
            currentCountUpTime += Time.deltaTime;
            this.countDownText.text = string.Format("{0:00.00}", currentCountUpTime - 1.0);

            if (this.currentCountUpTime >= this.leavingTime) {
                Debug.Log("離せ！");
                this.leaveFingerCounted = true;
            }
        }

    }

    /// <summary>
    /// ゲーム開始の通知
    /// </summary>
    private void gameStart() {
        this._canvasManager.gameStart();
    }

    /// <summary>
    /// 指を離す指定時間を生成する
    /// </summary>
    private float createLeaveFingerTime() {

        float leavingTime = Random.Range(3 * 2, maxLeavingTime * 2);
        // 0.5秒単位で生成する
        return leavingTime / 2;
    }
}
