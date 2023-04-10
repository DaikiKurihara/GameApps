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
    private bool leaveFingerCounted = false;
    private float leavingTime = 0.0F;

    private GameManager gameManager;
    private CanvasManager canvasManager;
    private PhysicalLayerManager physicalLayerManager;

    public void reset() {
        currentCountDownTime = countTime;
        leaveFingerCounted = false;
        currentCountUpTime = 0.0F;
    }

    void Start() {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        canvasManager = GameObject.FindWithTag("CanvasManager").GetComponent<CanvasManager>();
        physicalLayerManager = GameObject.FindWithTag("PhysicalLayerManager").GetComponent<PhysicalLayerManager>();
        // カウントダウン開始秒数をセット
        currentCountDownTime = countTime;
    }

    void Update() {
        // カウントダウン
        if (gameManager.IsCountDownStart && !gameManager.IsGameStart) {
            // 経過時刻を引いていく
            currentCountDownTime -= Time.deltaTime;
            countDownText.text = formatTime(currentCountDownTime);
            if (currentCountDownTime <= 0.000F) {
                gameStart();

            }
        } else if (!gameManager.IsCountDownStart && !gameManager.IsGameStart) {
            // カウントダウン開始秒数をリセット
            currentCountDownTime = countTime;
            countDownText.text = formatTime(currentCountDownTime);
        }

        // 離す時間までのカウントアップ
        // 強制終了されている場合は動かない
        if (!gameManager.IsForcedTermination && gameManager.IsGameStart && !leaveFingerCounted) {
            currentCountUpTime += Time.deltaTime;

            // びびらす
            onSurprise();

            if (currentCountUpTime >= leavingTime) {
                stop();
                leaveFingerCounted = true;
                // 色を変えて離す合図
                canvasManager.turnLeftLight();
            }
        }
    }

    /// <summary>
    /// ゲーム開始の通知
    /// </summary>
    private void gameStart() {
        gameManager.IsGameStart = true;
        currentCountDownTime = 0.00F;
        countDownText.text = "";
        canvasManager.turnLeftLightDefault();
        leavingTime = createLeaveFingerTime();
        createSurpriseTime();
        GameObject.FindWithTag(CommonConstant.COLOR_INSTRUCTION).GetComponent<Image>().color = new Color32(0, 0, 0, 0);
    }

    /// <summary>
    /// 指を離す時間の通知
    /// </summary>
    private void stop() {
        gameManager.IsStopped = true;
    }

    /// <summary>
    /// 指を離す指定時間を生成する
    /// </summary>
    private float createLeaveFingerTime() {
        float leavingTime = Random.Range(3, gameManager.MaxTime);
        // ゲームマネージャーに離す時間確定を通知
        gameManager.decideStandardTime(leavingTime);
        return leavingTime;
    }

    /// <summary>
    /// ビビらす
    /// </summary>
    private void onSurprise() {
        if (gameManager.SurpriseTimes?.Count > 0 && gameManager.SurpriseTimes[0] <= currentCountUpTime) {
            physicalLayerManager.onSurpriseRandom();
            canvasManager.supriseColorChange();
            gameManager.SurpriseTimes.RemoveAt(0);
        }
    }

    /// <summary>
    /// びびらす時間と回数を生成
    /// </summary>
    /// <returns></returns>
    private void createSurpriseTime() {
        // 0~指を離す時間÷5の回数分びびらす（19秒なら最大3回）
        int surpriseCount = Mathf.FloorToInt(Random.Range(0, gameManager.StandardTime / 5));
        // max時間は指を離す時間より1秒以上前
        float maxTime = gameManager.StandardTime - 1.0F;
        for (int i = 0; i < surpriseCount; i++) {
            // min時間は前回のビビらせ時間より1秒以上間隔を空ける
            float minTime = i == 0 ? 1.0F : gameManager.SurpriseTimes[i - 1] + 1.0F;
            gameManager.SurpriseTimes.Add(Random.Range(minTime, maxTime));
        }
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
        s = $"<mspace=0.5em>{string.Format("{0:00}.{1:00}", tSecond, tMilliSecond)}</mspace>";
        return s;
    }
}
