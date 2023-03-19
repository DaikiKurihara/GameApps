using TMPro;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    private float currentCountDownTime;
    private float currentCountUpTime = 0.0F;
    public TextMeshProUGUI countDownText;
    [SerializeField] private float countTime = 5.0F;
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
                this.gameStart();

            }
        } else if (!this._gameManager.IsCountDownStart && !this._gameManager.IsGameStart) {
            // カウントダウン開始秒数をリセット
            this.currentCountDownTime = countTime;
            this.countDownText.text = formatTime(this.currentCountDownTime);
        }

        // 離す時間までのカウントアップ
        // 強制終了されている場合は動かない
        if (!_gameManager.IsForcedTermination && this._gameManager.IsGameStart && !_leaveFingerCounted) {
            currentCountUpTime += Time.deltaTime;

            // びびらす
            onSurprise();

            if (this.currentCountUpTime >= this._leavingTime) {
                this.stop();
                this._leaveFingerCounted = true;
                // 色を変えて離す合図
                this._canvasManager.turnLeftLight();
            }
        }
    }

    /// <summary>
    /// ゲーム開始の通知
    /// </summary>
    private void gameStart() {
        this._gameManager.IsGameStart = true;
        this.currentCountDownTime = 0.00F;
        this.countDownText.text = "";
        this._canvasManager.turnLeftLightDefault();
        this._leavingTime = createLeaveFingerTime();
        Debug.Log("離す時間は：" + this._leavingTime);
        createSurpriseTime();
        GameObject.FindWithTag(CommonConstant.COLOR_INSTRUCTION).GetComponent<Image>().color = new Color32(0, 0, 0, 0);
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
        float leavingTime = Random.Range(3, _gameManager.MaxTime);
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
            _canvasManager.supriseColorChange();
            _gameManager.SurpriseTimes.RemoveAt(0);
        }
    }

    /// <summary>
    /// びびらす時間と回数を生成
    /// </summary>
    /// <returns></returns>
    private void createSurpriseTime() {
        // 0~指を離す時間÷5の回数分びびらす（19秒なら最大3回）
        int surpriseCount = Mathf.FloorToInt(Random.Range(0, _gameManager.StandardTime / 5));
        // max時間は指を離す時間より1秒以上前
        float maxTime = _gameManager.StandardTime - 1.0F;
        Debug.Log($"びびらし回数：{surpriseCount}");
        for (int i = 0; i < surpriseCount; i++) {
            // min時間は前回のビビらせ時間より1秒以上間隔を空ける
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
        s = $"<mspace=0.5em>{string.Format("{0:00}:{1:00}", tSecond, tMilliSecond)}</mspace>";
        return s;
    }
}
