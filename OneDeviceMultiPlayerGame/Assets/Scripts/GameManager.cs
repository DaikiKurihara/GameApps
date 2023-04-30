using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

    /** GameManager */
    private CanvasManager canvasManager;
    [SerializeField] private GameObject resultCanvas;

    // 準備画面突入有無のフラグ
    private bool isCountDownStart = false;
    public bool IsCountDownStart {
        set { isCountDownStart = value; }
        get { return isCountDownStart; }
    }

    // 準備時間が終わって全プレイヤーの指がついている状態
    private bool isGameStart = false;
    public bool IsGameStart {
        set {
            isGameStart = true;
            gameStart();
        } // falseは代入不可
        get { return isGameStart; }
    }

    // ゲーム終了（ゲーム開始→ストップタイム→指が全部離れた状態）フラグ
    private bool isGameEnd = false;
    public bool IsGameEnd {
        set {
            isGameEnd = true;
            decideEndTime();
        } // falseは代入不可
        get { return isGameEnd; }
    }

    // 指を離す時間になったか
    private bool isStopped = false;
    public bool IsStopped {
        set { isStopped = true; } // falseは代入不可
        get { return isStopped; }
    }

    // 現在タッチしているプレイヤーの数
    private int touchingPlayerCount = 0;
    // ゲーム開始時のプレイヤー数
    private int playerCount = 0;

    // 結果表示
    private bool isOpenResult = false;

    /** ゲーム開始からの経過時間 */
    private float passedTime { get; set; } = 0.0F;
    /** 指を離す指定時間 */
    private float standardTime { get; set; } = 0.0F;
    public float StandardTime {
        get { return standardTime; }
    }
    /** 指を離す最大時間 */
    private float maxTime;
    public float MaxTime {
        get {
            if (maxTime == 0) {
                return 15;
            }
            return maxTime;
        }
    }
    [NonSerialized] public bool isOnVibration;
    /** ビビらせ音有無 */
    [NonSerialized] public bool isOnFeintSound;
    /** ゲームが終了した時間 */
    private float endTime { get; set; } = 0.0F;
    /// <summary>
    /// ゲームが強制終了されたか
    /// </summary>
    private bool isForcedTermination = false;
    public bool IsForcedTermination {
        get { return isForcedTermination; }
    }
    /** 指を離した時間を格納する辞書 */
    private Dictionary<int, float> leftTimeMap = new Dictionary<int, float>();
    /** フライングしたfingerIDリスト */
    private List<int> falseStartedList = new List<int>();
    /** fingerIDをキーにプレイヤーNoを格納する辞書 */
    private Dictionary<int, int> playerNumberMap = new Dictionary<int, int>();
    /** ビビらす時間 */
    private List<float> surpriseTimes = new List<float>();
    public List<float> SurpriseTimes {
        get { return surpriseTimes; }
    }
    /** <fingerID, プレイヤーNo, ランク, 秒差> */
    private List<(int fingerId, int playerNum, int rank, float diff)> playersResult = new List<(int fingerId, int playerNum, int rank, float diff)>();
    public List<(int fingerId, int playerNum, int rank, float diff)> PlayersResult {
        get { return playersResult; }
    }
    private List<Color32> colors = new List<Color32>() { ColorConstant.LEFT_LIGHT_PINK, ColorConstant.LEFT_LIGHT_GREEN, ColorConstant.LEFT_LIGHT_BLUE };
    public List<Color32> Colors {
        get { return colors.ToList(); }
    }
    private int leftCircleColorIndex = -1;
    public int LeftCircleColorIndex {
        get { return leftCircleColorIndex; }
    }

    void Start() {
        canvasManager = GameObject.FindWithTag("CanvasManager").GetComponent<CanvasManager>();
        dicideCircleColor();
    }

    void Update() {
        if (isGameStart) {
            passedTime += Time.deltaTime;
        }

        // 指が離れてから3秒後に結果を出す
        if (!isForcedTermination && isOpenResult && passedTime - endTime > 3.0) {
            openResult();
        }
    }

    /// <summary>
    /// ゲームスタート（（準備時間が終わって全プレイヤーの指がついている状態）に走る処理
    /// </summary>
    private void gameStart() {
        playerCount = Input.touchCount;
        canvasManager.dicidePlayerNumbers();
    }

    /// <summary>
    /// fingerIdを受け取り、fingerIdをキーにした経過時間のセットを作る
    /// </summary>
    /// <param name="fingerId"></param>
    public void fingerLeft(int fingerId) {
        leftTimeMap.Add(fingerId, passedTime - standardTime);
        if (!isStopped) {
            // 指を離す時間になっていない場合、フライングしたプレイヤーとして格納しておく
            falseStartPlayer(fingerId);
        }
    }

    /// <summary>
    /// 指を離すべき基準時間を決定する
    /// </summary>
    public void decideStandardTime(float standardTime) {
        this.standardTime = standardTime;
    }

    /// <summary>
    /// 全員の指が離れた時間を決定する
    /// </summary>
    private void decideEndTime() {
        endTime = passedTime;
        isOpenResult = true;
    }

    public void increaseTouchingPlayerCount() {
        touchingPlayerCount++;
    }

    public void decreaseTouchingPlayerCount() {
        if (touchingPlayerCount > 0) {
            touchingPlayerCount--;
        }
    }

    public void setMaxTime(string value) {
        int i;
        if (int.TryParse(value, out i)) {
            i = i == 0 ? int.Parse(MaxTimeMapConstant.DEFAULT_MAX_TIME) : i;
            maxTime = i;
        } else {
            maxTime = int.Parse(MaxTimeMapConstant.DEFAULT_MAX_TIME);
        }
    }

    /// <summary>
    /// フライングしたプレイヤーを格納
    /// </summary>
    /// <param name="fingerId"></param>
    private void falseStartPlayer(int fingerId) {
        falseStartedList.Add(fingerId);
        if (falseStartedList.Count == playerCount) {
            isForcedTermination = true;
            // forceTerminateをコルーチンで呼ぶ
            StartCoroutine("forceTerminate");
        }
    }

    public void checkPlayerCount() {
        if (Input.touchCount != touchingPlayerCount) {
            Debug.Log($"プレイヤー人数に異常が発生しました。{touchingPlayerCount}");
            gameReset();
            canvasManager.displayError();
        }
    }

    public void dicidePlayerNumber(int fingerId, int playerNum) {
        playerNumberMap.Add(fingerId, playerNum);
    }

    /// <summary>
    /// 指を離す時の色を決める
    /// </summary>
    private void dicideCircleColor() {
        leftCircleColorIndex = UnityEngine.Random.Range(0, colors.Count);
    }

    private void openResult() {
        // フライングしたプレイヤーを除いたディクショナリをクローンする
        var cloneDict = new Dictionary<int, float>(leftTimeMap).
            Where(x => x.Value > 0).
            ToDictionary(d => d.Key, d => d.Value);
        // 話した時間の絶対値を取る
        List<int> keys = new List<int>(cloneDict.Keys);
        foreach (int k in keys) {
            cloneDict[k] = Math.Abs(cloneDict[k]);
        }
        // LINQを使ってValueでソート
        var sortedList = cloneDict.OrderBy(x => x.Value).ToList();
        // 結果の表示（Debug）
        int i = 1;
        int rank = 0;
        float previousPlayerTime = -1;
        foreach (KeyValuePair<int, float> kvp in sortedList) {
            if (previousPlayerTime != kvp.Value) {
                rank = i;
            }
            playersResult.Add((kvp.Key, playerNumberMap[kvp.Key], rank, kvp.Value));
            i++;
            previousPlayerTime = kvp.Value;
        }
        foreach (int fingerId in falseStartedList) {
            playersResult.Add((fingerId, playerNumberMap[fingerId], -1, -1));
        }
        canvasManager.openResult(playersResult);
        resultCanvas.SetActive(true);
        destroyAd();
        resultCanvas.GetComponent<ResultCanvasManager>().openPlayerResults(playersResult);
        isOpenResult = false;
    }

    private void destroyAd() {
        GameObject ads = GameObject.FindWithTag(CommonConstant.GOOGLE_MOBILE_ADS);
        if (ads != null) {
            ads.GetComponent<GoogleMoblieAds>().DestroyAd();
        }
    }

    private void loadAd() {
        GameObject ads = GameObject.FindWithTag(CommonConstant.GOOGLE_MOBILE_ADS);
        if (ads != null) {
            ads.GetComponent<GoogleMoblieAds>().LoadAd();
        }
    }

    private void gameReset() {
        isCountDownStart = false;
        isGameEnd = false;
        isGameStart = false;
        isStopped = false;
        playerCount = 0;
        passedTime = 0;
        endTime = 0;
        standardTime = 0;
        leftTimeMap.Clear();
        playerNumberMap.Clear();
        falseStartedList.Clear();
        surpriseTimes.Clear();
        canvasManager.resetCanvas();
        GameObject.FindWithTag(CommonConstant.COLOR_INSTRUCTION).GetComponent<Image>().color = new Color32(0, 0, 0, 255);
        playersResult.Clear();
        touchingPlayerCount = 0;
        isOpenResult = false;
    }

    /// <summary>
    /// ゲーム終了後に再開する
    /// </summary>
    public void gameRetry() {
        // 指を離す色を再度決め直す
        dicideCircleColor();
        isForcedTermination = false;
        gameReset();
        loadAd();
        resultCanvas.GetComponent<ResultCanvasManager>().gemeRetry();// リセットとの相違点
    }

    IEnumerator forceTerminate() {
        yield return new WaitForSeconds(1);
        openResult();
    }
}
