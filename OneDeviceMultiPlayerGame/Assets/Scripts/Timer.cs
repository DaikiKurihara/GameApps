using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System;
// avoid a conflict between System.Random and UnityEngine.
using Random = UnityEngine.Random;

public class Timer : MonoBehaviour {

    private float currentCountDownTime;
    private float currentCountUpTime = 0.0F;
    public TextMeshProUGUI countDownText;
    [SerializeField] private float countTime = 5.0F;
    /** 指を離す時間（最大）の指定 */
    [SerializeField] int maxLeavingTime = 20;
    private bool _leaveFingerCounted = false;
    private float _leavingTime = 0.0F;

    private GameManager _gameManager;
    private CanvasManager _canvasManager;
    private PhysicalLayerManager _physicalLayerManager;

    public void reset() {
        this.currentCountDownTime = countTime;
        this._leaveFingerCounted = false;
        this.currentCountUpTime = 0.0F;
    }

    void Start() {
        _gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        _canvasManager = GameObject.FindWithTag("CanvasManager").GetComponent<CanvasManager>();
        _physicalLayerManager = GameObject.FindWithTag("PhysicalLayerManager").GetComponent<PhysicalLayerManager>();
        // カウントダウン開始秒数をセット
        this.currentCountDownTime = countTime;
    }

    void Update() {
        // カウントダウン
        if (this._gameManager.IsCountDownStart && !this._gameManager.IsGameStart) {
            // 経過時刻を引いていく
            this.currentCountDownTime -= Time.deltaTime;
            this.countDownText.text = formatTime(this.currentCountDownTime);
            if (this.currentCountDownTime <= 0.000F) {
                this.currentCountDownTime = 0.00F;
                this.gameStart();
                this.countDownText.text = "";
                this._leavingTime = createLeaveFingerTime();
                createSurpriseTime();
                Debug.Log("離す時間は：" + this._leavingTime);
            }
        } else if (!this._gameManager.IsCountDownStart && !this._gameManager.IsGameStart) {
            // カウントダウン開始秒数をリセット
            this.currentCountDownTime = countTime;
            this.countDownText.text = formatTime(this.currentCountDownTime);
        }

        // 離す時間までのカウントアップ
        if (this._gameManager.IsGameStart && !_leaveFingerCounted) {
            currentCountUpTime += Time.deltaTime;

            // びびらす
            onSurprise();

            if (this.currentCountUpTime >= this._leavingTime) {
                this.stop();
                this._leaveFingerCounted = true;
                // 色を変えて離す合図
                this._canvasManager.turnLeftLightBlue();
            }
        }
    }

    /// <summary>
    /// ゲーム開始の通知
    /// </summary>
    private void gameStart() {
        this._gameManager.IsGameStart = true;
    }

    /// <summary>
    /// 指を離す時間の通知
    /// </summary>
    private void stop() {
        this._gameManager.IsStopped = true;
    }

    /// <summary>
    /// 指を離す指定時間を生成する
    /// </summary>
    private float createLeaveFingerTime() {
        // int型で生成した値をfloatの2で除算して0.5秒単位で生成する
        float leavingTime = (Random.Range(3 * 2, maxLeavingTime * 2)) / 2.0F;
        // ゲームマネージャーに離す時間確定を通知
        this._gameManager.decideStandardTime(leavingTime);
        return leavingTime;
    }

    /// <summary>
    /// ビビらす
    /// </summary>
    private void onSurprise() {
        if (_gameManager.SurpriseTimes?.Count > 0 && _gameManager.SurpriseTimes[0] <= currentCountUpTime) {
            _physicalLayerManager.onSurpriseRandom();
            _gameManager.SurpriseTimes.RemoveAt(0);
        }
    }

    /// <summary>
    /// びびらす時間と回数を生成
    /// </summary>
    /// <returns></returns>
    private void createSurpriseTime() {
        // 0 ~指を離す時間÷5の回数分びびらす（19秒なら3回）
        int surpriseCount = Mathf.FloorToInt(Random.Range(0, _gameManager.StandardTime / 5));
        float maxTime = _gameManager.StandardTime - 1.0F;
        Debug.Log($"びびらし回数：{surpriseCount}");
        for (int i = 0; i < surpriseCount; i++) {
            float minTime = i == 0 ? 1.0F : _gameManager.SurpriseTimes[i - 1] + 1.0F;
            _gameManager.SurpriseTimes.Add(Random.Range(minTime, maxTime));
        }
        Debug.Log($"びびらし秒数：{string.Join(",", _gameManager.SurpriseTimes)}");
    }

    /// <summary>
    /// 表示時間の文字列を返す
    /// </summary>
    private string formatTime(float t) {
        // 返すフォーマット後文字列
        string s;
        // 秒以下表示桁数
        int Ndigit = 2;
        // 秒部分
        int tSecond = (int)Math.Floor(Math.Abs(t));
        // 秒以下部分
        int tMilliSecond = (int)Math.Floor(Math.Abs(t - tSecond) * Math.Pow(10, Ndigit));
        s = string.Format("{0:00}:{1:00}", tSecond, tMilliSecond);
        return s;
    }
}
